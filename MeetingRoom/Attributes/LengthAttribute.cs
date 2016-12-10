using System.ComponentModel.DataAnnotations;
using EmployeeContext.Exceptions;

namespace EmployeeContext.Attributes
{
    public class LengthAttribute : ValidationAttribute
    {
        private readonly int _length = 13;

        public LengthAttribute(int l)
        {
            _length = l;
        }

        public override bool IsValid(object value)
        {
            bool result = true;
            var cnpValue = (CNP)value;

            if (cnpValue.Value.Length != _length)
            {
                result = false;
                throw new InvalidInputException("Invalid CNP");
            }
            return result;


        }

    }
}
