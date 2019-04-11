using System.Data.Objects;

namespace Vueling.XXX.DB.Infrastructure.Repositories
{
    public class ContextBase
    {

        private ObjectContext _context;

        public ObjectContext Context 
        {
            get{ return _context; }
            set { _context = value; }
        }

    }
}
