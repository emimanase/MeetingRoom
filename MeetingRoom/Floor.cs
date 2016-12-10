namespace EmployeeContext
{
    public class Floor
    {

        private int _number;

        internal int Number
        {
            get { return _number; }
            set { _number = value; }
        }

        public Floor(int number)
        {
            _number = number;
        }

        public static bool operator ==(Floor f1, Floor f2)
        {
            return f1.Number == f2.Number;
        }

        public static bool operator !=(Floor f1, Floor f2)
        {
            return !(f1.Number == f2.Number);
        }

        public override string ToString()
        {
            return _number.ToString();
        }
    }
}
