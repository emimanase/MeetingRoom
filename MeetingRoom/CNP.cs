using EmployeeContext.Attributes;
using EmployeeContext.Exceptions;

namespace EmployeeContext
{
    public class CNP
    {
        private string _value;

        [Length(13)]
        internal string Value
        {
            get { return string.IsNullOrEmpty(_value) ? string.Empty : _value; }
            set
            {
                //there is a simple validation for mocks 
                //checks the first digit to be 1 or 2 and have 13 digits
                if ((value.StartsWith("1") || value.StartsWith("2")) && value.Length == 13)
                    _value = value;
                else
                    throw new InvalidInputException("Invalid CNP");
            }
        }

        public CNP(string value)
        {
            Value = value;
        }

        public static bool operator ==(CNP c1, CNP c2)
        {
            return c1.Value == c2.Value;
        }

        public static bool operator !=(CNP c1, CNP c2)
        {
            return !(c1.Value == c2.Value);
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
