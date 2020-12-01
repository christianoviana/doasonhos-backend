using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace PucMinas.Services.Charity.Domain.Results.Exceptions
{
    public class ErrorMessage
    {
        public ErrorMessage()
        {
            this.Details = new List<string>();
        }

        public ErrorMessage(int statusCode, string message, List<string> details) : this()
        {
            StatusCode = statusCode;
            Message = message;
            Details = details;
        }

        public ErrorMessage(int statusCode, string message) : this()
        {
            StatusCode = statusCode;
            Message = message;
        }

        [JsonProperty("status_code")]
        public int StatusCode { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("details")]
        public List<string> Details { get; set; }
    }
}
