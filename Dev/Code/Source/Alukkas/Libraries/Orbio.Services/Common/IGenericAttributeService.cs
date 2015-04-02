using Orbio.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Services.Common
{
    /// <summary>
    /// interface for GenericAttribute service
    /// </summary>
    public interface IGenericAttributeService
    {
        ///<summary>
        ///get all generic attributes
        ///</summary>
        ///<param name="action">the action value</param>
        ///<param name="entityId">the entityId(customerId) value</param>
        ///<param name="keyGroup">the keyGroup value</param>
        ///<param name="key">the key value</param>
        ///<param name="value">the keyValue value</param>
        ///<param name="storeId">the storeId value</param>
        GenericAttribute GetGenericAttributes(string action, int entityId, string keyGroup, string key, string value, int storeId);

        ///<summary>
        ///insert all customer generic attributes to table
        ///</summary>
        ///<param name="action">the action value</param>
        ///<param name="entityId">the entityId(customerId) value</param>
        ///<param name="keyGroup">the keyGroup value</param>
        ///<param name="key">the key value</param>
        ///<param name="value">the keyValue value</param>
        ///<param name="storeId">the storeId value</param>
        void SaveGenericAttributes(string action, int entityId, string keyGroup, string key, string value, int storeId);
    }
}
