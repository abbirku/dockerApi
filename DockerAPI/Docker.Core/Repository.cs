using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using System.Data.Common;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Data.SqlTypes;
using System.Reflection;

namespace Docker.Core
{
    public abstract class Repository<TEntity, TKey, TContext>
        : IRepository<TEntity, TKey, TContext>
        where TEntity : class, IEntity<TKey>
        where TContext : DbContext
    {
        protected TContext _dbContext;
        protected DbSet<TEntity> _dbSet;
        protected int CommandTimeout { get; set; }

        public Repository(TContext context)
        {
            _dbContext = context;
            _dbSet = _dbContext.Set<TEntity>();
        }

        public virtual void Add(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        public virtual void Remove(TKey id)
        {
            var entityToDelete = _dbSet.Find(id);
            Remove(entityToDelete);
        }

        public virtual void Remove(TEntity entityToDelete)
        {
            if (_dbContext.Entry(entityToDelete).State == EntityState.Detached)
            {
                _dbSet.Attach(entityToDelete);
            }
            _dbSet.Remove(entityToDelete);
        }

        public virtual void Remove(Expression<Func<TEntity, bool>> filter)
        {
            _dbSet.RemoveRange(_dbSet.Where(filter));
        }

        public virtual void Edit(TEntity entityToUpdate)
        {
            //Find the tracking object in local cache
            var local = _dbSet.Local.FirstOrDefault(e => e.Id.Equals(entityToUpdate.Id));

            // check if local is not null 
            if (local != null) {
                //Then detach
                _dbContext.Entry(local).State = EntityState.Detached;
            }

            // set Modified flag in your entry
            _dbContext.Entry(entityToUpdate).State = EntityState.Modified;

            //_dbSet.Attach(entityToUpdate);
            //_dbContext.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public virtual int GetCount(Expression<Func<TEntity, bool>> filter = null)
        {
            IQueryable<TEntity> query = _dbSet;
            var count = 0;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            count = query.Count();
            return count;
        }

        public virtual IList<TEntity> Get(Expression<Func<TEntity, bool>> filter)
        {
            IQueryable<TEntity> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return query.ToList();
        }

        public virtual IList<TEntity> GetAll()
        {
            IQueryable<TEntity> query = _dbSet;
            return query.ToList();
        }

        public virtual TEntity GetById(TKey id)
        {
            return _dbSet.Find(id);
        }

        public virtual (IList<TEntity> data, int total, int totalDisplay) Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "", int pageIndex = 1, int pageSize = 10, bool isTrackingOff = false)
        {
            IQueryable<TEntity> query = _dbSet;
            var total = query.Count();
            var totalDisplay = query.Count();

            if (filter != null)
            {
                query = query.Where(filter);
                totalDisplay = query.Count();
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                var result = orderBy(query).Skip((pageIndex - 1) * pageSize).Take(pageSize);
                if (isTrackingOff)
                    return (result.AsNoTracking().ToList(), total, totalDisplay);
                else
                    return (result.ToList(), total, totalDisplay);
            }
            else
            {
                var result = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
                if (isTrackingOff)
                    return (result.AsNoTracking().ToList(), total, totalDisplay);
                else
                    return (result.ToList(), total, totalDisplay);
            }
        }

        public virtual (IList<TEntity> data, int total, int totalDisplay) GetDynamic(
            Expression<Func<TEntity, bool>> filter = null,
            string orderBy = null,
            string includeProperties = "", int pageIndex = 1, int pageSize = 10, bool isTrackingOff = false)
        {
            IQueryable<TEntity> query = _dbSet;
            var total = query.Count();
            var totalDisplay = query.Count();

            if (filter != null)
            {
                query = query.Where(filter);
                totalDisplay = query.Count();
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                var result = query.OrderBy(orderBy).Skip((pageIndex - 1) * pageSize).Take(pageSize);
                if (isTrackingOff)
                    return (result.AsNoTracking().ToList(), total, totalDisplay);
                else
                    return (result.ToList(), total, totalDisplay);
            }
            else
            {
                var result = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
                if (isTrackingOff)
                    return (result.AsNoTracking().ToList(), total, totalDisplay);
                else
                    return (result.ToList(), total, totalDisplay);
            }
        }

        public virtual IList<TEntity> Get(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "", bool isTrackingOff = false)
        {
            IQueryable<TEntity> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                var result = orderBy(query);

                if (isTrackingOff)
                    return result.AsNoTracking().ToList();
                else
                    return result.ToList();
            }
            else
            {
                if (isTrackingOff)
                    return query.AsNoTracking().ToList();
                else
                    return query.ToList();
            }
        }

        public virtual IList<TEntity> GetDynamic(Expression<Func<TEntity, bool>> filter = null,
            string orderBy = null,
            string includeProperties = "", bool isTrackingOff = false)
        {
            IQueryable<TEntity> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                var result = query.OrderBy(orderBy);

                if (isTrackingOff)
                    return result.AsNoTracking().ToList();
                else
                    return result.ToList();
            }
            else
            {
                if (isTrackingOff)
                    return query.AsNoTracking().ToList();
                else
                    return query.ToList();
            }
        }

        protected virtual async Task<TReturn> ExecuteScalarAsync<TReturn>(string storedProcedureName, IDictionary<string, object> parameters = null)
        {
            DbCommand command = CreateCommand(storedProcedureName, parameters);
            bool connectionOpened = false;
            if (command.Connection.State == ConnectionState.Closed)
            {
                await command.Connection.OpenAsync();
                connectionOpened = true;
            }
            TReturn result;
            try
            {
                result = await ExecuteScalarAsync<TReturn>(command);
            }
            finally
            {
                if (connectionOpened)
                    await command.Connection.CloseAsync();
            }

            return result;
        }

        protected virtual IDictionary<string, object> ExecuteStoredProcedure(string storedProcedureName, IDictionary<string, object> parameters = null, IDictionary<string, Type> outParameters = null)
        {
            var dbNull = ConvertNullToDbNull(CreateCommand(storedProcedureName, parameters, outParameters));
            bool flag = false;
            if (dbNull.Connection.State == ConnectionState.Closed)
            {
                dbNull.Connection.Open();
                flag = true;
            }
            try
            {
                dbNull.ExecuteNonQuery();
            }
            finally
            {
                if (flag)
                    dbNull.Connection.Close();
            }
            return CopyOutParams(dbNull, outParameters);
        }

        protected virtual async Task<IDictionary<string, object>> ExecuteStoredProcedureAsync(string storedProcedureName, IDictionary<string, object> parameters = null, IDictionary<string, Type> outParameters = null)
        {
            var command = CreateCommand(storedProcedureName, parameters, outParameters);
            command = ConvertNullToDbNull(command);
            bool connectionOpened = false;

            if (command.Connection.State == ConnectionState.Closed)
            {
                await command.Connection.OpenAsync();
                connectionOpened = true;
            }
            try
            {
                int num = await command.ExecuteNonQueryAsync();
            }
            finally
            {
                if (connectionOpened)
                    await command.Connection.CloseAsync();
            }
            
            var dictionary = this.CopyOutParams(command, outParameters);
            
            return dictionary;
        }

        protected virtual async Task<(IList<TReturn> result, IDictionary<string, object> outValues)> QueryWithStoredProcedureAsync<TReturn>(string storedProcedureName,IDictionary<string, object> parameters = null,IDictionary<string, Type> outParameters = null)
            where TReturn : class, new()
        {
            var command = CreateCommand(storedProcedureName, parameters, outParameters);
            bool connectionOpened = false;

            if (command.Connection.State == ConnectionState.Closed)
            {
                await command.Connection.OpenAsync();
                connectionOpened = true;
            }
            IList<TReturn> result = null;
            try
            {
                result = await ExecuteQueryAsync<TReturn>(command);
            }
            finally
            {
                if (connectionOpened)
                    await command.Connection.CloseAsync();
            }

            (IList<TReturn>, IDictionary<string, object>) valueTuple = (result, CopyOutParams(command, outParameters));
            return valueTuple;
        }

        private DbCommand CreateCommand( string storedProcedureName, IDictionary<string, object> parameters = null, IDictionary<string, Type> outParameters = null)
        {
            DbCommand command = _dbContext.Database.GetDbConnection().CreateCommand();
            command.CommandText = storedProcedureName;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = CommandTimeout;
            if (parameters != null)
            {
                foreach (KeyValuePair<string, object> parameter in parameters)
                    command.Parameters.Add((object)CreateParameter(parameter.Key, parameter.Value));
            }
            if (outParameters != null)
            {
                foreach (KeyValuePair<string, Type> outParameter in outParameters)
                    command.Parameters.Add((object)CreateOutputParameter(outParameter.Key, outParameter.Value));
            }
            return command;
        }

        private DbParameter CreateParameter(string name, object value) => (DbParameter)new SqlParameter(name, CorrectSqlDateTime(value));

        private DbParameter CreateOutputParameter(string name, DbType dbType)
        {
            SqlParameter sqlParameter = new SqlParameter(name, CorrectSqlDateTime((object)dbType));
            sqlParameter.Direction = ParameterDirection.Output;
            return sqlParameter;
        }

        private DbParameter CreateOutputParameter(string name, Type type)
        {
            SqlParameter sqlParameter = new SqlParameter(name, GetDbTypeFromType(type));
            sqlParameter.Direction = ParameterDirection.Output;
            return sqlParameter;
        }

        private SqlDbType GetDbTypeFromType(Type type)
        {
            if (type == typeof(int) || type == typeof(uint) || (type == typeof(short) || type == typeof(ushort)))
                return SqlDbType.Int;
            if (type == typeof(long) || type == typeof(ulong))
                return SqlDbType.BigInt;
            if (type == typeof(double) || type == typeof(Decimal))
                return SqlDbType.Decimal;
            if (type == typeof(string))
                return SqlDbType.NVarChar;
            if (type == typeof(DateTime))
                return SqlDbType.DateTime;
            if (type == typeof(bool))
                return SqlDbType.Bit;
            if (type == typeof(Guid))
                return SqlDbType.UniqueIdentifier;
            int num = type == typeof(char) ? 1 : 0;
            return SqlDbType.NVarChar;
        }

        private object ChangeType(Type propertyType, object itemValue)
        {
            switch (itemValue)
            {
                case DBNull _:
                    return null;
                case Decimal _:
                    if (propertyType == typeof(double))
                        return Convert.ToDouble(itemValue);
                    break;
            }
            return itemValue;
        }

        private object CorrectSqlDateTime(object parameterValue) => parameterValue != null && parameterValue.GetType().Name == "DateTime" && Convert.ToDateTime(parameterValue) < SqlDateTime.MinValue.Value ? (object)SqlDateTime.MinValue.Value : parameterValue;

        private async Task<IList<TReturn>> ExecuteQueryAsync<TReturn>(DbCommand command)
        {
            var reader = await command.ExecuteReaderAsync();
            var result = new List<TReturn>();
            while (true)
            {
                if (await reader.ReadAsync())
                {
                    Type type = typeof(TReturn);
                    object obj = type.GetConstructor(new Type[0]).Invoke(new object[0]);
                    for (int ordinal = 0; ordinal < reader.FieldCount; ++ordinal)
                    {
                        PropertyInfo property = type.GetProperty(reader.GetName(ordinal));
                        property.SetValue(obj, ChangeType(property.PropertyType, reader.GetValue(ordinal)));
                    }
                    result.Add((TReturn)obj);
                }
                else
                    break;
            }
            return result;
        }

        private async Task<TReturn> ExecuteScalarAsync<TReturn>(DbCommand command)
        {
            command = ConvertNullToDbNull(command);
            if (command.Connection.State != ConnectionState.Open)
                command.Connection.Open();
            object obj = await command.ExecuteScalarAsync();
            return obj != DBNull.Value ? (TReturn)obj : default(TReturn);
        }

        private DbCommand ConvertNullToDbNull(DbCommand command)
        {
            for (int index = 0; index < command.Parameters.Count; ++index)
            {
                if (command.Parameters[index].Value == null)
                    command.Parameters[index].Value = DBNull.Value;
            }
            return command;
        }

        private IDictionary<string, object> CopyOutParams(
          DbCommand command,
          IDictionary<string, Type> outParameters)
        {
            Dictionary<string, object> dictionary = null;
            if (outParameters != null)
            {
                dictionary = new Dictionary<string, object>();
                foreach (KeyValuePair<string, Type> outParameter in outParameters)
                    dictionary.Add(outParameter.Key, command.Parameters[outParameter.Key].Value);
            }
            return dictionary;
        }
    }
}
