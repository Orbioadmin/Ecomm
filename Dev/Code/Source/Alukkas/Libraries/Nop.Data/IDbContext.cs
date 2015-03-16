using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using Nop.Core;
using Orbio.Core.Domain.Orders;

namespace Nop.Data
{
    public interface IDbContext
    {
        /// <summary>
        /// Get DbSet
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <returns>DbSet</returns>
        IDbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity;

        /// <summary>
        /// Save changes
        /// </summary>
        /// <returns></returns>
        int SaveChanges();

        /// <summary>
        /// Execute stores procedure and load a list of entities at the end
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <param name="commandText">Command text</param>
        /// <param name="parameters">Parameters</param>
        /// <returns>Entities</returns>
        IList<TEntity> ExecuteStoredProcedureList<TEntity>(string commandText, params object[] parameters)
            where TEntity : BaseEntity, new();
       
          

        /// <summary>
        /// Creates a raw SQL query that will return elements of the given generic type.  The type can be any type that has properties that match the names of the columns returned from the query, or can be a simple primitive type. The type does not have to be an entity type. The results of this query are never tracked by the context even if the type of object returned is an entity type.
        /// </summary>
        /// <typeparam name="TElement">The type of object returned by the query.</typeparam>
        /// <param name="sql">The SQL query string.</param>
        /// <param name="parameters">The parameters to apply to the SQL query string.</param>
        /// <returns>Result</returns>
        IEnumerable<TElement> SqlQuery<TElement>(string sql, params object[] parameters);

        /// <summary>
        /// Executes the given DDL/DML command against the database.
        /// </summary>
        /// <param name="sql">The command string</param>
        /// <param name="doNotEnsureTransaction">false - the transaction creation is not ensured; true - the transaction creation is ensured.</param>
        /// <param name="timeout">Timeout value, in seconds. A null value indicates that the default value of the underlying provider will be used</param>
        /// <param name="parameters">The parameters to apply to the command string.</param>
        /// <returns>The result returned by the database after executing the command.</returns>
        int ExecuteSqlCommand(string sql, bool doNotEnsureTransaction = false, int? timeout = null, params object[] parameters);

        /// <summary>
        /// executes a stored procedure and returns specified result
        /// </summary>
        /// <typeparam name="T">type of result</typeparam>
        /// <param name="commandText">sp name</param>
        /// <param name="parameters">any parameters if required</param>
        /// <returns>object result of type T</returns>
        List<T> ExecuteFunction<T>(string commandText, params object[] parameters);

        ///// <summary>
        ///// executes storeprocedure and returns 3 multiple result set
        ///// </summary>
        ///// <typeparam name="T1">first result type</typeparam>
        ///// <typeparam name="T2">second result type</typeparam>
        ///// <typeparam name="T3">3rd result type</typeparam>
        ///// <param name="commandText">sp name</param>
        ///// <param name="parameters">any parameters if required</param>
        ///// <returns>object result of type T</returns>
        //Tuple<T1,T2,T3> ExecuteMultipleResult<T1,T2,T3>(string commandText, params object[] parameters);

        List<ShoppingCartItem> ExecuteFunctionModel(string commandText, params object[] parameters);
    }
}
