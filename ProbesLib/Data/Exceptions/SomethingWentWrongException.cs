using System;
using System.Collections.Generic;
using System.Text;

namespace ProbesLib.Data.Exceptions
{
    public class SomethingWentWrongException : Exception
    {
        public virtual string ErrorMessageDeveloper()
        {
            return string.Empty;
        }

        public virtual string ErrorMessageUser()
        {
            return string.Empty;
        }
    }
}
