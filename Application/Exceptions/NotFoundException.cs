using System;
using System.Net;

namespace Application.Exceptions
{
    public class WebApiException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }
    }
}
