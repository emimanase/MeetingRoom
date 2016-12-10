using DAL;

namespace EmployeeContext.Repositories
{
    public abstract class AbstractRepository
    {
        private static Dal _dal;
        public Dal Dal { get { return _dal ?? new Dal(); } }

        public AbstractRepository()
        {
            _dal = _dal ?? new Dal();
        }
    }
}
