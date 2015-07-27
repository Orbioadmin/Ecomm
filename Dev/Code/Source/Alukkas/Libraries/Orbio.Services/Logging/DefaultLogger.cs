using Orbio.Core;
using Orbio.Core.Data;
using DM = Orbio.Core.Domain.Customers;
using Orbio.Core.Domain.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Services.Logging
{
    /// <summary>
    /// Default logger
    /// </summary>
    public partial class DefaultLogger : ILogger
    {
        #region Fields
        private readonly IWebHelper webHelper;
        
        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="webHelper">Web helper</param>>
        public DefaultLogger(IWebHelper webHelper)
        {

            this.webHelper = webHelper;

        }

        #endregion

        #region Methods

        /// <summary>
        /// Determines whether a log level is enabled
        /// </summary>
        /// <param name="level">Log level</param>
        /// <returns>Result</returns>
        public virtual bool IsEnabled(LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Debug:
                    return false;
                default:
                    return true;
            }
        }

        /// <summary>
        /// Deletes a log item
        /// </summary>
        /// <param name="log">Log item</param>
        public virtual void DeleteLog(Log log)
        {
            if (log == null)
                throw new ArgumentNullException("log");

            using (var db = new OrbioAdminContext())
            {
                db.Logs.Remove(log);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Clears a log
        /// </summary>
        public virtual void ClearLog()
        {
            using (var db = new OrbioAdminContext())
            {
                db.Database.ExecuteSqlCommand("TRUNCATE TABLE [Log]");
            }
        }



        /// <summary>
        /// Gets a log item
        /// </summary>
        /// <param name="logId">Log item identifier</param>
        /// <returns>Log item</returns>
        public virtual Log GetLogById(int logId)
        {
            if (logId == 0)
                return null;

            Log log = null;

            using(var db = new OrbioAdminContext())
            {
                log = (from l in db.Logs
                       where l.Id == logId
                       select l).FirstOrDefault();
            }
            return log;
        }

        /// <summary>
        /// Get log items by identifiers
        /// </summary>
        /// <param name="logIds">Log item identifiers</param>
        /// <returns>Log items</returns>
        public virtual IList<Log> GetLogByIds(int[] logIds)
        {
            if (logIds == null || logIds.Length == 0)
                return new List<Log>();
            var sortedLogItems = new List<Log>();
            using (var db = new OrbioAdminContext())
            {
                var query = from l in db.Logs
                            where logIds.Contains(l.Id)
                            select l;
                var logItems = query.ToList();
                //sort by passed identifiers

                foreach (int id in logIds)
                {
                    var log = logItems.Find(x => x.Id == id);
                    if (log != null)
                        sortedLogItems.Add(log);
                }
            }
            return sortedLogItems;
        }

        /// <summary>
        /// Inserts a log item
        /// </summary>
        /// <param name="logLevel">Log level</param>
        /// <param name="shortMessage">The short message</param>
        /// <param name="fullMessage">The full message</param>
        /// <param name="customer">The customer to associate log record with</param>
        /// <returns>A log item</returns>
        public virtual Log InsertLog(LogLevel logLevel, string shortMessage, string fullMessage = "", DM.Customer customer = null)
        {
            var log = new Log()
            {
                LogLevelId = (int)logLevel,
                ShortMessage = shortMessage,
                FullMessage = fullMessage,
                IpAddress = webHelper.GetCurrentIpAddress(),
                Customer = customer!=null?new Customer { Id=customer.Id, CreatedOnUtc = customer.CreatedOnUtc, LastActivityDateUtc = customer.LastActivityDateUtc,
                   LastLoginDateUtc = null}:null,
                PageUrl = webHelper.GetThisPageUrl(true),
                ReferrerUrl = webHelper.GetUrlReferrer(),
                CreatedOnUtc = DateTime.UtcNow
            };

            using (var db = new OrbioAdminContext())
            {
                db.Logs.Add(log);
                db.SaveChanges();
            }

            return log;
        }

        #endregion
    }
}
