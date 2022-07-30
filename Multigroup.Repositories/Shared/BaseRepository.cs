using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Multigroup.Repositories.Shared
{
    public abstract class BaseRepository
    {
        private static DatabaseProviderFactory factory = new DatabaseProviderFactory();

        public BaseRepository()
        {
            Db = factory.Create("DefaultConnection");
        }

        protected Database Db { get; private set; }

        /// <summary>
        /// Create a class instance if the default search yielded no results
        /// </summary>
        /// <typeparam name="T">generic object</typeparam>
        /// <param name="objects">List generic object</param>
        /// <returns>Default instance object type T</returns>
        public T CreateInstanceNotResult<T>(IEnumerable<T> objects) where T : new()
        {
            if (objects.Count() == 0)
                return new T();
            else
                return objects.FirstOrDefault();
        }

        public T GetFirstElement<T>(IEnumerable<T> objects) where T : new()
        {
            if (!objects.Any())
                return default(T);
            else
                return objects.FirstOrDefault();
        }
    }
}
