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
		/// Queries all existing devices known by the LAN controller
		/// </summary>
		/// <returns></returns>
		public static List<TradfriDevice> GetDevices()
		{
			coapClient.UriPath = $"{CommandConstants.uniqueDevices}/";
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
		/// Queries all existing groups known by the LAN controller
		/// </summary>
		/// <returns></returns>
		public static List<TradfriGroup> GetGroups()
		{
			coapClient.UriPath = $"{CommandConstants.groups}/";
			Response response = coapClient.Get();

			var groupIds = Newtonsoft.Json.JsonConvert.DeserializeObject<List<int>>(response.PayloadString);

			List<TradfriGroup> groups = new List<TradfriGroup>();

			foreach (int id in groupIds)
			{
				groups.Add(GetGroup(id));
			}

			return groups;
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

		/// <summary>
		/// Queries a single group by its unique group id
		/// </summary>
		/// <param name="groupId"></param>
		/// <returns></returns>
		public static TradfriGroup GetGroup(int groupId)
		{
			coapClient.UriPath = $"{CommandConstants.groups}/{groupId}";
			Response response = coapClient.Get();

			return TradfriGroup.Parse(response.PayloadString);
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

		/// <summary>
		/// Sets the brightness of the target lamp to the given brightness value
		/// </summary>
		/// <param name="lampId">Target device ID</param>
		/// <param name="newBrightness">Desired brightness value</param>
		/// <returns>Controller response object</returns>
		public static ControllerResponse SetBrightness(int lampId, byte newBrightness)
		{
			if (CheckDevice(lampId) == false)
				return new ControllerResponse(StatusCode.DeviceUnreachable);

			coapClient.UriPath = $"{CommandConstants.uniqueDevices}/{lampId}";
			var result = new ControllerResponse(coapClient.Put("{ \"3311\":[{ \"5851\": " + newBrightness + "}]}"));

			if (result.StatusCode == StatusCode.Changed)
				return (GetDevice(lampId) as Lamp)?.Brightness == newBrightness ? result : new ControllerResponse(StatusCode.NotModified);
			else
				return result;
		}

		/// <summary>
		/// Sets the color of the target lamp to the given color value
		/// </summary>
		/// <param name="lampId">Target device ID</param>
		/// <param name="newColor">Desired color</param>
		/// <returns>Controller response object</returns>
		public static ControllerResponse SetColor(int lampId, Color newColor)
		{
			if (CheckDevice(lampId) == false)
				return new ControllerResponse(StatusCode.DeviceUnreachable);

			coapClient.UriPath = $"{CommandConstants.uniqueDevices}/{lampId}";
			var result = new ControllerResponse(coapClient.Put("{ \"3311\":[{ \"5709\": " + newColor.Value5709 + ", \"5710\": " + newColor.Value5710 + "}]}"));

			if (result.StatusCode == StatusCode.Changed)
				return (GetDevice(lampId) as Lamp)?.Color == newColor ? result : new ControllerResponse(StatusCode.NotModified);
			else
				return result;
		}

		/// <summary>
		/// Sets the color of the target lamp to the given RGB value
		/// </summary>
		/// <param name="lampId">Target device ID</param>
		/// <param name="rgb">Desired color value by RGB</param>
		/// <returns>Controller response object</returns>
		public static ControllerResponse SetColorByRGB(int lampId, string rgb)
		{
			if (CheckDevice(lampId) == false)
				return new ControllerResponse(StatusCode.DeviceUnreachable);

			coapClient.UriPath = $"{CommandConstants.uniqueDevices}/{lampId}";
			var result = new ControllerResponse(coapClient.Put("{ \"3311\":[{ \"5706\": \"" + rgb + "\"}]}"));

			if (result.StatusCode == StatusCode.Changed)
				return (GetDevice(lampId) as Lamp)?.Color.ValueRgb == rgb ? result : new ControllerResponse(StatusCode.NotModified);
			else
				return result;
		}

		/// <summary>
		/// Sets the state of the target lamp
		/// </summary>
		/// <param name="lampId">Target device ID</param>
		/// <param name="newState">Desired lamp state</param>
		/// <returns>Controller response object</returns>
		public static ControllerResponse SetState(int lampId, LampState newState)
		{
			if (CheckDevice(lampId) == false)
				return new ControllerResponse(StatusCode.DeviceUnreachable);

			coapClient.UriPath = $"{CommandConstants.uniqueDevices}/{lampId}";
			var result = new ControllerResponse(coapClient.Put("{ \"3311\":[{ \"5850\": " + (int)newState + "}]}"));

			if (result.StatusCode == StatusCode.Changed)
				return (GetDevice(lampId) as Lamp)?.State == newState ? result : new ControllerResponse(StatusCode.NotModified);
			else
				return result;
		}


		#endregion

		#region OVERRIDES for interfaces and base types



		#endregion
	}
}
