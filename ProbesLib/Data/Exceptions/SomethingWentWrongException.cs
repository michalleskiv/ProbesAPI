using System;
using System.Collections.Generic;
using System.Text;

namespace ProbesLib.Data.Exceptions
{
    public abstract class SomethingWentWrongException : Exception
    {
        public abstract string ErrorMessageDeveloper();

        public abstract string ErrorMessageUser();
    }
}
