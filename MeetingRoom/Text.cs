namespace EmployeeContext
{
    public class Text
    {
        private string _continut;

        internal string Continut
        {
            get { return string.IsNullOrEmpty(_continut) ? string.Empty : _continut; }
            set { _continut = value; }
        }

        public Text(string continut)
        {
            _continut = continut;
        }

        public static bool operator ==(Text t1, Text t2)
        {
            return t1.Continut == t2.Continut;
        }

        public static bool operator !=(Text t1, Text t2)
        {
            return !(t1.Continut == t2.Continut);
        }

        public override string ToString()
        {
            return Continut;
        }
    }
}
