using System;

namespace EmployeeContext
{
    public class TimetableEntry
    {
        private DateTime _start;
        private DateTime _end;

        public int Id { get; private set; }

        public DateTime From
        {
            get { return _start == DateTime.MinValue ? DateTime.Now : _start; }
            private set
            {
                _start = value;
            }
        }

        public DateTime To
        {
            get { return _end == DateTime.MinValue ? DateTime.Now : _end; }
            private set
            {
                _end = value;
            }
        }

        public CNP OwnerCNP { get; private set; }

        internal TimetableEntry(int id, DateTime from, DateTime to, CNP ocnp)
        {
            Id = id;
            From = from;
            To = to;
            OwnerCNP = ocnp;
        }

        public void SetFrom(DateTime from)
        {
            _start = from;
        }
        public void SetTo(DateTime to)
        {
            _end = to;
        }

        public void SetOwnerCNP(CNP cnp)
        {
            OwnerCNP = cnp;
        }

    }
}
