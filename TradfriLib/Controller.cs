using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

using Com.AugustCellars.CoAP;
using Com.AugustCellars.CoAP.DTLS;
using Com.AugustCellars.CoAP.Net;
using Com.AugustCellars.COSE;
using PeterO.Cbor;

using TradfriLib.Data;

namespace TradfriLib
{
	/// <summary>
	/// Controls the Tradfri devices using the IKEA Tradfri LAN controller
	/// </summary>
	public static class Controller
	{
		private struct CommandConstants
		{
			public const int uniqueDevices = 15001;
			public const int groups = 15004;
			public const int moods = 15005;
		}


		#region FIELDS

		private static IPEndPoint controllerAddress;
		private static string connectionSecurityKey;

		private static CoapClient coapClient;

		#endregion

		#region EVENTS



		#endregion

		#region PROPERTIES



		#endregion

		#region CONSTRUCTOR



		#endregion

		#region METHODS

		/// <summary>
		/// Initializes the connection with the LAN controller
		/// </summary>
		/// <param name="endPoint">IP endpoint of the IKEA Tradfri LAN Controller</param>
		/// <param name="securityKey">Private security key found on the bottom of the LAN Controller</param>
		public static void InitializeConnection(IPEndPoint endPoint, string securityKey)
		{
			if (endPoint == null)
				throw new ArgumentNullException(nameof(endPoint));
			if (string.IsNullOrWhiteSpace(securityKey))
				throw new ArgumentNullException(nameof(securityKey));

			try
			{
				controllerAddress = endPoint;
				connectionSecurityKey = securityKey;

				OneKey userKey = new OneKey();
				userKey.Add(CoseKeyKeys.KeyType, GeneralValues.KeyType_Octet);
				userKey.Add(CoseKeyParameterKeys.Octet_k, CBORObject.FromObject(Encoding.UTF8.GetBytes(connectionSecurityKey)));

				CoAPEndPoint coapEndPoint = new DTLSClientEndPoint(userKey);
				coapClient = new CoapClient(new Uri($"coaps://{controllerAddress.Address}:{controllerAddress.Port}/"))
				{
					EndPoint = coapEndPoint
				};
				coapEndPoint.Start();

				// there has to be a GET operation to successfully open the connection

				coapClient.UriPath = CommandConstants.uniqueDevices + "/";
				coapClient.Get();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

		/// <summary>
		/// Closes the existing connection
		/// </summary>
		public static void DisposeConnection()
		{
			coapClient.EndPoint.Stop();
		}


		/// <summary>
		/// Queries all existing devices know by the LAN controller
		/// </summary>
		/// <returns></returns>
		public static List<TradfriDevice> GetDevices()
		{
			coapClient.UriPath = CommandConstants.uniqueDevices + "/";
			Response response = coapClient.Get();

			var deviceIds = Newtonsoft.Json.JsonConvert.DeserializeObject<List<int>>(response.PayloadString);

			List<TradfriDevice> devices = new List<TradfriDevice>();

			foreach (int id in deviceIds)
			{
				devices.Add(GetDevice(id));
			}

			return devices;
		}

		/// <summary>
		/// Queries a single device by its unique device id
		/// </summary>
		/// <param name="deviceId">Unique device id</param>
		/// <returns></returns>
		public static TradfriDevice GetDevice(int deviceId)
		{
			coapClient.UriPath = $"{CommandConstants.uniqueDevices}/{deviceId}";
			Response response = coapClient.Get();

			return TradfriDevice.Parse(response.PayloadString);
		}

		// TODO
		public static void GetGroups()
		{

		}

		/// <summary>
		/// Checks wether a device exists
		/// </summary>
		/// <param name="deviceId">Unique device id</param>
		/// <returns>True if the device exists, false if the device is unreachable or non existing</returns>
		private static bool CheckDevice(int deviceId)
		{
			var device = GetDevice(deviceId);

			if (device == null || device.Reachable == false)
			{
				System.Diagnostics.Debug.WriteLine("No such device or it is currently unavailable.");
				return false;
			}

			return true;
		}


		public static void SetLamp(Lamp modifiedLamp)
		{
			if (CheckDevice(modifiedLamp.Id) == false)
				return;

			var currentLampValue = GetDevice(modifiedLamp.Id) as Lamp;

			if (currentLampValue.Color.Name != modifiedLamp.Color.Name)
				SetColor(modifiedLamp.Id, modifiedLamp.Color);

			if (currentLampValue.Brightness != modifiedLamp.Brightness)
				SetBrightness(modifiedLamp.Id, modifiedLamp.Brightness);

			if (currentLampValue.State != modifiedLamp.State)
				SetState(modifiedLamp.Id, modifiedLamp.State);
		}


		public static Response SetBrightness(int lampId, byte newBrightness)
		{
			if (CheckDevice(lampId) == false)
				return null;

			coapClient.UriPath = $"{CommandConstants.uniqueDevices}/{lampId}";
			return coapClient.Put("{ \"3311\":[{ \"5851\": " + newBrightness + "}]}");
		}


		public static Response SetColor(int lampId, Color newColor)
		{
			if (CheckDevice(lampId) == false)
				return null;

			coapClient.UriPath = $"{CommandConstants.uniqueDevices}/{lampId}";
			return coapClient.Put("{ \"3311\":[{ \"5709\": " + newColor.Value5709 + ", \"5710\": " + newColor.Value5710 + "}]}");
		}


		public static Response SetColorByRGB(int lampId, string rgb)
		{
			if (CheckDevice(lampId) == false)
				return null;

			coapClient.UriPath = $"{CommandConstants.uniqueDevices}/{lampId}";
			return coapClient.Put("{ \"3311\":[{ \"5706\": \"" + rgb + "\"}]}");
		}


		public static Response SetState(int lampId, LampState newState)
		{
			if (CheckDevice(lampId) == false)
				return null;

			coapClient.UriPath = $"{CommandConstants.uniqueDevices}/{lampId}";
			return coapClient.Put("{ \"3311\":[{ \"5850\": " + (int)newState + "}]}");
		}


		#endregion

		#region OVERRIDES for interfaces and base types



		#endregion
	}
}
