using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Reflection;
using Nop.Core;
using Orbio.Core.Domain.Orders;
using Orbio.Core.Domain.Catalog;
namespace Nop.Data
{
    /// <summary>
    /// Object context
    /// </summary>
    public class NopPlainObjectContext : DbContext, IDbContext
    {
        #region Ctor

        public NopPlainObjectContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            //((IObjectContextAdapter) this).ObjectContext.ContextOptions.LazyLoadingEnabled = true;
            
        }
        
        #endregion

        #region Utilities

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    //dynamically load all configuration
        //    //System.Type configType = typeof(LanguageMap);   //any of your configuration classes here
        //    //var typesToRegister = Assembly.GetAssembly(configType).GetTypes()

        //    //var typesToRegister = Assembly.GetExecutingAssembly().GetTypes()
        //    //.Where(type => !String.IsNullOrEmpty(type.Namespace))
        //    //.Where(type => type.BaseType != null && type.BaseType.IsGenericType && type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>));
        //    //foreach (var type in typesToRegister)
        //    //{
        //    //    dynamic configurationInstance = Activator.CreateInstance(type);
        //    //    modelBuilder.Configurations.Add(configurationInstance);
        //    //}
        //    //...or do it manually below. For example,
        //    //modelBuilder.Configurations.Add(new LanguageMap());

            

        //    base.OnModelCreating(modelBuilder);
        //}

        /// <summary>
        /// Attach an entity to the context or return an already attached entity (if it was already attached)
        /// </summary>
        /// <typeparam name="TEntity">TEntity</typeparam>
        /// <param name="entity">Entity</param>
        /// <returns>Attached entity</returns>
        protected virtual TEntity AttachEntityToContext<TEntity>(TEntity entity) where TEntity : BaseEntity, new()
        {
            //little hack here until Entity Framework really supports stored procedures
            //otherwise, navigation properties of loaded entities are not loaded until an entity is attached to the context
            var alreadyAttached = Set<TEntity>().Local.FirstOrDefault(x => x.Id == entity.Id);
            if (alreadyAttached == null)
            {
                //attach new entity
                Set<TEntity>().Attach(entity);
                return entity;
            }
            else
            {
                //entity is already loaded.
                return alreadyAttached;
            }
        }
        
        #endregion

        #region Methods

        /// <summary>
        /// Create database script
        /// </summary>
        /// <returns>SQL to generate database</returns>
        public string CreateDatabaseScript()
        {
            return ((IObjectContextAdapter)this).ObjectContext.CreateDatabaseScript();
        }

        /// <summary>
        /// Get DbSet
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <returns>DbSet</returns>
        public new IDbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity
        {
            return base.Set<TEntity>();
        }
        
        /// <summary>
        /// Execute stores procedure and load a list of entities at the end
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <param name="commandText">Command text</param>
        /// <param name="parameters">Parameters</param>
        /// <returns>Entities</returns>
        public IList<TEntity> ExecuteStoredProcedureList<TEntity>(string commandText, params object[] parameters) where TEntity : BaseEntity, new()
        {
            //HACK: Entity Framework Code First doesn't support doesn't support output parameters
            //That's why we have to manually create command and execute it.
            //just wait until EF Code First starts support them
            //
            //More info: http://weblogs.asp.net/dwahlin/archive/2011/09/23/using-entity-framework-code-first-with-stored-procedures-that-have-output-parameters.aspx

            bool hasOutputParameters = false;
            if (parameters != null)
            {
                foreach (var p in parameters)
                {
                    var outputP = p as DbParameter;
                    if (outputP == null)
                        continue;

                    if (outputP.Direction == ParameterDirection.InputOutput ||
                        outputP.Direction == ParameterDirection.Output)
                        hasOutputParameters = true;
                }
            }

            

            var context = ((IObjectContextAdapter)(this)).ObjectContext;
            if (!hasOutputParameters)
            {
                //no output parameters
                var result = this.Database.SqlQuery<TEntity>(commandText, parameters).ToList();
                for (int i = 0; i < result.Count; i++)
                    result[i] = AttachEntityToContext(result[i]);
                        
                return result;
                
                //var result = context.ExecuteStoreQuery<TEntity>(commandText, parameters).ToList();
                //foreach (var entity in result)
                //    Set<TEntity>().Attach(entity);
                //return result;
            }
            else
            {

                //var connection = context.Connection;
                var connection = this.Database.Connection;
                //Don't close the connection after command execution


                //open the connection for use
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                //create a command object
                using (var cmd = connection.CreateCommand())
                {
                    //command to execute
                    cmd.CommandText = commandText;
                    cmd.CommandType = CommandType.StoredProcedure;

                    // move parameters to command object
                    if (parameters != null)
                        foreach (var p in parameters)
                            cmd.Parameters.Add(p);

                    //database call
                    var reader = cmd.ExecuteReader();                    
                    //return reader.DataReaderToObjectList<TEntity>();
                    var result = context.Translate<TEntity>(reader).ToList();
                    for (int i = 0; i < result.Count; i++)
                        result[i] = AttachEntityToContext(result[i]);
                    //close up the reader, we're done saving results
                    reader.Close();
                    return result;
                }

            }
        }

        /// <summary>
        /// Creates a raw SQL query that will return elements of the given generic type.  The type can be any type that has properties that match the names of the columns returned from the query, or can be a simple primitive type. The type does not have to be an entity type. The results of this query are never tracked by the context even if the type of object returned is an entity type.
        /// </summary>
        /// <typeparam name="TElement">The type of object returned by the query.</typeparam>
        /// <param name="sql">The SQL query string.</param>
        /// <param name="parameters">The parameters to apply to the SQL query string.</param>
        /// <returns>Result</returns>
        public IEnumerable<TElement> SqlQuery<TElement>(string sql, params object[] parameters)
        {
            return this.Database.SqlQuery<TElement>(sql, parameters);
        }
    
        /// <summary>
        /// Executes the given DDL/DML command against the database.
        /// </summary>
        /// <param name="sql">The command string</param>
        /// <param name="doNotEnsureTransaction">false - the transaction creation is not ensured; true - the transaction creation is ensured.</param>
        /// <param name="timeout">Timeout value, in seconds. A null value indicates that the default value of the underlying provider will be used</param>
        /// <param name="parameters">The parameters to apply to the command string.</param>
        /// <returns>The result returned by the database after executing the command.</returns>
        public int ExecuteSqlCommand(string sql, bool doNotEnsureTransaction = false, int? timeout = null, params object[] parameters)
        {
            int? previousTimeout = null;
            if (timeout.HasValue)
            {
                //store previous timeout
                previousTimeout = ((IObjectContextAdapter) this).ObjectContext.CommandTimeout;
                ((IObjectContextAdapter) this).ObjectContext.CommandTimeout = timeout;
            }

            var transactionalBehavior = doNotEnsureTransaction
                ? TransactionalBehavior.DoNotEnsureTransaction
                : TransactionalBehavior.EnsureTransaction;
            var result = this.Database.ExecuteSqlCommand(transactionalBehavior, sql, parameters);

            if (timeout.HasValue)
            {
                //Set previous timeout back
                ((IObjectContextAdapter) this).ObjectContext.CommandTimeout = previousTimeout;
            }

            //return result
            return result;
        }

        #endregion


        /// <summary>
        /// executes a stored procedure and returns specified result
        /// </summary>
        /// <typeparam name="T">type of result</typeparam>
        /// <param name="commandText">sp name</param>
        /// <param name="parameters">any parameters if required</param>
        /// <returns>object result of type T</returns>
        public List<T> ExecuteFunction<T>(string commandText, params object[] parameters)
        {
            //var result = ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<T>(commandText, (parameters==null || parameters.Count()==0)? new ObjectParameter[] { }:parameters.Cast<ObjectParameter>().ToArray());

            //return result.ToList();

            //HACK: Entity Framework Code First doesn't support doesn't support output parameters
            //That's why we have to manually create command and execute it.
            //just wait until EF Code First starts support them
            //
            //More info: http://weblogs.asp.net/dwahlin/archive/2011/09/23/using-entity-framework-code-first-with-stored-procedures-that-have-output-parameters.aspx

            bool hasOutputParameters = false;
            if (parameters != null)
            {
                foreach (var p in parameters)
                {
                    var outputP = p as DbParameter;
                    if (outputP == null)
                        continue;

                    if (outputP.Direction == ParameterDirection.InputOutput ||
                        outputP.Direction == ParameterDirection.Output)
                        hasOutputParameters = true;
                }
            }



            var context = ((IObjectContextAdapter)(this)).ObjectContext;
            //if (!hasOutputParameters)
            //{
            //    //no output parameters
            //    var result = this.Database.SqlQuery<T>(commandText, (parameters == null || parameters.Count() == 0) ? new object[] { } : parameters).ToList();
             
            //    return result;

            //    //var result = context.ExecuteStoreQuery<TEntity>(commandText, parameters).ToList();
            //    //foreach (var entity in result)
            //    //    Set<TEntity>().Attach(entity);
            //    //return result;
            //}
            //else
            //{

                //var connection = context.Connection;
                var connection = this.Database.Connection;
                //Don't close the connection after command execution


                //open the connection for use
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                //create a command object
                using (var cmd = connection.CreateCommand())
                {
                    //command to execute
                    cmd.CommandText = commandText;
                    cmd.CommandType = CommandType.StoredProcedure;

                    // move parameters to command object
                    if (parameters != null)
                        foreach (var p in parameters)
                            cmd.Parameters.Add(p);

                    //database call
                    var reader = cmd.ExecuteReader();
                    //return reader.DataReaderToObjectList<TEntity>();
                    var result = context.Translate<T>(reader).ToList();
                
                    //close up the reader, we're done saving results
                    reader.Close();
                    connection.Close();
                    return result;
                }

            //}
        }

        public List<ShoppingCartItem> ExecuteFunctionModel(string commandText, params object[] parameters)
        {
            //var result = ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<T>(commandText, (parameters==null || parameters.Count()==0)? new ObjectParameter[] { }:parameters.Cast<ObjectParameter>().ToArray());

            //return result.ToList();

            //HACK: Entity Framework Code First doesn't support doesn't support output parameters
            //That's why we have to manually create command and execute it.
            //just wait until EF Code First starts support them
            //
            //More info: http://weblogs.asp.net/dwahlin/archive/2011/09/23/using-entity-framework-code-first-with-stored-procedures-that-have-output-parameters.aspx

            bool hasOutputParameters = false;
            if (parameters != null)
            {
                foreach (var p in parameters)
                {
                    var outputP = p as DbParameter;
                    if (outputP == null)
                        continue;

                    if (outputP.Direction == ParameterDirection.InputOutput ||
                        outputP.Direction == ParameterDirection.Output)
                        hasOutputParameters = true;
                }
            }



            var context = ((IObjectContextAdapter)(this)).ObjectContext;
            //if (!hasOutputParameters)
            //{
            //    //no output parameters
            //    var result = this.Database.SqlQuery<T>(commandText, (parameters == null || parameters.Count() == 0) ? new object[] { } : parameters).ToList();

            //    return result;

            //    //var result = context.ExecuteStoreQuery<TEntity>(commandText, parameters).ToList();
            //    //foreach (var entity in result)
            //    //    Set<TEntity>().Attach(entity);
            //    //return result;
            //}
            //else
            //{

            //var connection = context.Connection;
            var connection = this.Database.Connection;
            //Don't close the connection after command execution


            //open the connection for use
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            //create a command object
            using (var cmd = connection.CreateCommand())
            {
                //command to execute
                cmd.CommandText = commandText;
                cmd.CommandType = CommandType.StoredProcedure;

                // move parameters to command object
                if (parameters != null)
                    foreach (var p in parameters)
                        cmd.Parameters.Add(p);

                //database call
                var reader = cmd.ExecuteReader();
                //return reader.DataReaderToObjectList<TEntity>()
                List<ShoppingCartItem> result = new List<ShoppingCartItem>();
                List<ProductPicture> pict = new List<ProductPicture>();
                while (reader.Read())
                {
                    ShoppingCartItem re = new ShoppingCartItem();
                    Orbio.Core.Domain.Catalog.ProductPicture pic = new Orbio.Core.Domain.Catalog.ProductPicture();
                    re.CartId = Convert.ToInt32(reader["CartId"].ToString());
                    re.Id = Convert.ToInt32(reader["Id"].ToString());
                    re.Name = reader["Name"].ToString();
                    re.Price = Convert.ToDecimal(reader["Price"].ToString());
                    if (reader["RelativeUrl"] != null)
                    {
                        string RelativeUrl = reader["RelativeUrl"].ToString();
                        pic.RelativeUrl = RelativeUrl;
                    }
                    pict.Add(pic);
                    re.ProductPictures = pict;
                    re.CurrencyCode = reader["CurrencyCode"].ToString();
                    re.DisplayStockAvailability = Convert.ToBoolean(reader["DisplayStockAvailability"].ToString());
                    re.DisplayStockQuantity = Convert.ToBoolean(reader["DisplayStockQuantity"].ToString());
                    re.StockQuantity = Convert.ToInt32(reader["StockQuantity"].ToString());
                    re.OrderMinimumQuantity = Convert.ToInt32(reader["OrderMinimumQuantity"].ToString());
                    re.OrderMaximumQuantity = Convert.ToInt32(reader["OrderMaximumQuantity"].ToString());
                    re.AllowedQuantities = reader["AllowedQuantities"].ToString();
                    re.Quantity = Convert.ToInt32(reader["Quantity"].ToString());
                    //re.ItemCount = Convert.ToInt32(reader["Itemcount"].ToString());
                    re.TotalPrice = Convert.ToDecimal(reader["Totalprice"].ToString());
                    result.Add(re);
                }
                //var result = context.Translate<T>(reader).ToList();

                //close up the reader, we're done saving results
                reader.Close();
                return result;
            }

            //}
        }
    }
}