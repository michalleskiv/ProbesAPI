using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using Newtonsoft.Json;

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

        public override string ErrorMessageDeveloper()
        {
            return JsonConvert.SerializeObject(this);
        }

        public override string ErrorMessageUser()
        {
            return $"Probe with uniqueId: {IdProbe} doesn't exist";
        }
    }
}
