using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DAL.PostgresSQL
{
    public interface IDbAgent
    {
        void DropTables(string[] tableNames);

        IEnumerable<T> GetTable<T>(string tableName) where T : DbObj;

        T Find<T>(string tableName, long id = -1) where T : DbObj;

        IEnumerable<DbObj> Find<T>(string tableName, Expression<Func<T, bool>> predicate) where T : DbObj;

        long Insert<T>(T obj, string query) where T : DbObj;

        void Update<T>(long id, Action<T> action, string tableName = null) where T : DbObj;

        long UpdateOrInsert<T>(T obj, long? id = null, string tableName = null) where T : DbObj;
    }
}
