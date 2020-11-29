using System.Net;

namespace ProbesLib.Data.Exceptions
{
    /// <summary>
    /// Throw when DrMax server send 500 code
    /// </summary>
    public class DrMaxServerErrorException : SomethingWentWrongException
    {
        public DrMaxServerErrorException(HttpStatusCode httpCode) : base(httpCode) { }

        public override string ErrorMessageDeveloper()
        {
            return ToString();
        }

        public override string ErrorMessageUser()
        {
            return "DrMax internal server error. Please, contact us";
        }
    }
}
