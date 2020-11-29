using System.Net;

namespace ProbesLib.Data.Exceptions
{
    public class UnknownException : SomethingWentWrongException
    {
        public UnknownException(HttpStatusCode httpCode) : base(httpCode) { }

        public override string ErrorMessageDeveloper()
        {
            return ToString();
        }

        public override string ErrorMessageUser()
        {
            return "Unknown error";
        }
    }
}
