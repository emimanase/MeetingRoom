using System;

namespace EmployeeContext.Exceptions
{
    class InvalidInputException : Exception
    {
        private string error;

        public InvalidInputException(string e)
        {
            error = e;
        }
    }
}
