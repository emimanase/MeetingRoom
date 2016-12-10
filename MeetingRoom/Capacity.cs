using EmployeeContext.Exceptions;

namespace EmployeeContext
{
    public class Capacity
    {
        private int _number;

        internal int Number
        {
            get { return _number; }
            set
            {
                if(value < 0)
                    throw new InvalidInputException("Capacity has to be a positive integer");
                _number = value;
            }
        }

        public Capacity(int number)
        {
            _number = number;
        }

        public override string ToString()
        {
            return _number.ToString();
        }
    }
}
