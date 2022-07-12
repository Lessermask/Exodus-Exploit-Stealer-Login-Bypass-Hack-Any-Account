using Exodus.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace Exodus.API.Models
{
    public class API_Response<T>
    {
        public API_Response() { }
        public API_Response(T Data, string Time)
        {
            this.ErrorCode  = EN_ErrorCodes.NoException;
            this.Time = Time;
            this.Data = Data;
            this.RequestStatus = HttpStatusCode.OK;
        }

        public API_Response(T Data, string Time, EN_ErrorCodes ErrorCode, string ErrorMessage = "")
        {
            this.ErrorMessage = ErrorMessage;
            this.ErrorCode = ErrorCode;
            this.Time = Time;
            this.Data = Data;
            this.RequestStatus = HttpStatusCode.InternalServerError;
        }

        public API_Response(T Data, string Time, EN_ErrorCodes ErrorCode, HttpStatusCode RequestStatus, string ErrorMessage = "")
        {
            this.ErrorMessage = ErrorMessage;
            this.ErrorCode = ErrorCode;
            this.Time = Time;
            this.Data = Data;
            this.RequestStatus = RequestStatus;
        }

        public string ErrorMessage { get; set; } = "";
        public EN_ErrorCodes ErrorCode { get; set; }
        public HttpStatusCode RequestStatus { get; set; }
        public T Data { get; set; }
        public string Time { get; set; } = "";
    }
}