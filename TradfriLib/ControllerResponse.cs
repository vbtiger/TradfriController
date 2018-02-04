using System.Collections.Generic;

using AutoMapper;

using coap = Com.AugustCellars.CoAP;

namespace TradfriLib
{
	/// <summary>
	/// Response status codes.
	/// </summary>
	public enum StatusCode
	{
		/// <summary>
		/// 2.01 Created
		/// </summary>
		Created = 65,

		/// <summary>
		/// 2.02 Deleted
		/// </summary>
		Deleted = 66,

		/// <summary>
		/// 2.03 Valid
		/// </summary>
		Valid = 67,

		/// <summary>
		/// 2.04 Changed
		/// </summary>
		Changed = 68,

		/// <summary>
		/// 2.05 Content
		/// </summary>
		Content = 69,

		/// <summary>
		/// 2.31 Continue
		/// </summary>
		Continue = 95,

		/// <summary>
		/// 3.04 Not Modified
		/// </summary>
		NotModified = 100,

		/// <summary>
		/// 4.00 Bad Request
		/// </summary>
		BadRequest = 128,

		/// <summary>
		/// 4.01 Unauthorized
		/// </summary>
		Unauthorized = 129,

		/// <summary>
		/// 4.02 Bad Option
		/// </summary>
		BadOption = 130,

		/// <summary>
		/// 4.03 Forbidden
		/// </summary>
		Forbidden = 131,

		/// <summary>
		/// 4.04 Not Found
		/// </summary>
		NotFound = 132,

		/// <summary>
		/// 4.05 Method Not Allowed
		/// </summary>
		MethodNotAllowed = 133,

		/// <summary>
		/// 4.06 Not Acceptable
		/// </summary>
		NotAcceptable = 134,

		/// <summary>
		/// 4.08 Request Entity Incomplete [RFC7959]
		/// </summary>
		RequestEntityIncomplete = 136,

		/// <summary>
		/// 4.09 Conflict [RFC8132]
		/// </summary>
		Conflict = 137,

		/// <summary>
		/// 4.12 Precondition Failed
		/// </summary>
		PreconditionFailed = 140,

		/// <summary>
		/// 4.13 Request Entity Too Large
		/// </summary>
		RequestEntityTooLarge = 141,

		/// <summary>
		/// 4.15 Unsupported Media Type
		/// </summary>
		UnsupportedMediaType = 143,

		/// <summary>
		/// 4.18 I'm a teapot
		/// </summary>
		Teapot = 146,

		/// <summary>
		/// 4.22 Unprocessable Entity [RC8132]
		/// </summary>
		UnprocessableEntity = 150,

		/// <summary>
		/// 5.00 Internal Server Error
		/// </summary>
		InternalServerError = 160,

		/// <summary>
		/// 5.01 Not Implemented
		/// </summary>
		NotImplemented = 161,

		/// <summary>
		/// 5.02 Bad Gateway
		/// </summary>
		BadGateway = 162,

		/// <summary>
		/// 5.03 Service Unavailable
		/// </summary>
		ServiceUnavailable = 163,

		/// <summary>
		/// 5.04 Gateway Timeout
		/// </summary>
		GatewayTimeout = 164,

		/// <summary>
		/// 5.05 Proxying Not Supported
		/// </summary>
		ProxyingNotSupported = 165,

		/// <summary>
		/// 5.23 Device unreachable - No such device or it is currently unavailable.
		/// </summary>
		DeviceUnreachable = 183
	}

	/// <summary>
	/// Response message from the 
	/// </summary>
	public struct ControllerResponse
	{
		/// <summary>
		/// Gets the code of this CoAP message.
		/// </summary>
		public int Code { get; internal set; }

		/// <summary>
		/// Gets the code's string representation of this CoAP message.
		/// </summary>
		public string CodeString { get; internal set; }


		/// <summary>
		/// Gets or sets a value that indicates whether this CoAP message is canceled.
		/// </summary>
		public bool IsCancelled { get; internal set; }

		/// <summary>
		/// Gets or sets a value indicating whether this message has been rejected.
		/// </summary>
		public bool IsRejected { get; internal set; }

		/// <summary>
		/// Gets or sets a value that indicates whether this CoAP message has timed out.
		/// Confirmable messages in particular might timeout.
		/// </summary>
		public bool IsTimedOut { get; internal set; }


		/// <summary>
		/// Get the payload as a string
		/// </summary>
		public string ResponseText { get; internal set; }

		/// <summary>
		/// Gets the response status code.
		/// </summary>
		public StatusCode StatusCode { get; internal set; }

		static ControllerResponse()
		{
			Mapper.Initialize(cfg => cfg.CreateMap<coap.StatusCode, StatusCode>());
		}

		internal ControllerResponse(StatusCode code)
		{
			this.Code = (int)code;
			this.CodeString = code.GetCodeString();

			this.IsCancelled = false;
			this.IsRejected = false;
			this.IsTimedOut = false;
			this.ResponseText = null;
			this.StatusCode = code;
		}

		internal ControllerResponse(coap.Response coapResponse)
		{
			this.Code = coapResponse.Code;
			this.CodeString = coapResponse.CodeString;
			this.IsCancelled = coapResponse.IsCancelled;
			this.IsRejected = coapResponse.IsRejected;
			this.IsTimedOut = coapResponse.IsTimedOut;
			this.ResponseText = coapResponse.ResponseText;
			this.StatusCode = Mapper.Map<coap.StatusCode, StatusCode>(coapResponse.StatusCode);
		}
	}

	/// <summary>
	/// Extension methods for <see cref="StatusCode"/> enum
	/// </summary>
	public static class StatusCodeExtensions
	{
		private static readonly Dictionary<StatusCode, string> dict;

		static StatusCodeExtensions()
		{
			dict = new Dictionary<StatusCode, string>
				   {
					   {StatusCode.Created, "2.01 Created"},
					   {StatusCode.Deleted, "2.02 Deleted"},
					   {StatusCode.Valid, "2.03 Valid"},
					   {StatusCode.Changed, "2.04 Changed"},
					   {StatusCode.Content, "2.05 Content"},
					   {StatusCode.Continue, "2.31 Continue"},
					   {StatusCode.NotModified, "3.04 Not Modifiede"},
					   {StatusCode.BadRequest, "4.00 Bad Request"},
					   {StatusCode.Unauthorized, "4.01 Unauthorized"},
					   {StatusCode.BadOption, "4.02 Bad Option"},
					   {StatusCode.Forbidden, "4.03 Forbidden"},
					   {StatusCode.NotFound, "4.04 Not Found"},
					   {StatusCode.MethodNotAllowed, "4.05 Method Not Allowed"},
					   {StatusCode.NotAcceptable, "4.06 Not Acceptable"},
					   {StatusCode.RequestEntityIncomplete, "4.08 Request Entity Incomplete [RFC7959]"},
					   {StatusCode.Conflict, "4.09 Conflict [RFC8132]"},
					   {StatusCode.PreconditionFailed, "4.12 Precondition Failed"},
					   {StatusCode.RequestEntityTooLarge, "4.13 Request Entity Too Large"},
					   {StatusCode.UnsupportedMediaType, "4.15 Unsupported Media Type"},
					   {StatusCode.UnprocessableEntity, "4.22 Unprocessable Entity [RC8132]"},
					   {StatusCode.InternalServerError, "5.00 Internal Server Error"},
					   {StatusCode.NotImplemented, "5.01 Not Implemented"},
					   {StatusCode.BadGateway, "5.02 Bad Gateway"},
					   {StatusCode.ServiceUnavailable, "5.03 Service Unavailable"},
					   {StatusCode.GatewayTimeout, "5.04 Gateway Timeout"},
					   {StatusCode.ProxyingNotSupported, "5.05 Proxying Not Supported"},
					   {StatusCode.DeviceUnreachable, "5.23 Device unreachable - No such device or it is currently unavailable."}
				   };
		}

		/// <summary>
		/// Decodes the status code and returns a more detailed message about the value
		/// </summary>
		/// <param name="code"></param>
		/// <returns></returns>
		public static string GetCodeString(this StatusCode code)
		{
			return dict[code];
		}
	}
}
