using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace ProbesLib.Data.Exceptions
{
    public class ProbeNotFoundException : SomethingWentWrongException
    {
        public int IdProbe { get; set; }
        public string Url { get; set; }
        public HttpResponseMessage Response { get; set; }

        public ProbeNotFoundException(int idProbe, string url, HttpResponseMessage response)
        {
            IdProbe = idProbe;
            Url = url;
            Response = response;
        }
    }
}
