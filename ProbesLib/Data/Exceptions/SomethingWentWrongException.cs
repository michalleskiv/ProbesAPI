using System;
using System.Net;

namespace ProbesLib.Data.Exceptions
{
    public abstract class SomethingWentWrongException : Exception
    {
        public HttpStatusCode HttpCode { get; }

        protected SomethingWentWrongException(HttpStatusCode httpCode)
        {
            HttpCode = httpCode;
        }

        public abstract string ErrorMessageDeveloper();

        public abstract string ErrorMessageUser();
    }
}
