using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Multigroup.Common
{
    public abstract class BaseRepository
    {
        private static DatabaseProviderFactory factory = new DatabaseProviderFactory();

        public BaseRepository()
        {
            Db = factory.Create("DefaultConnection");
        }

        protected Database Db { get; private set; }
    }
}
