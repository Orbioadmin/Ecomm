using System;
using System.Linq;
using Orbio.Core;
using Nop.Core.Infrastructure;
using Nop.Data;
using Nop.Core;
using Orbio.Core.Domain.Customers;

namespace Orbio.Services.Common
{
    public static class GenericAttributeExtentions
    {
        /// <summary>
        /// Get an attribute of an entity
        /// </summary>
        /// <typeparam name="TPropType">Property type</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="key">Key</param>
        /// <param name="genericAttributeService">GenericAttributeService</param>
        /// <param name="storeId">Load a value specific for a certain store; pass 0 to load a value shared for all stores</param>
        /// <returns>Attribute</returns>
        public static TPropType GetAttribute<TPropType>(this Customer customer, string action,string key, int storeId)
        {
            var genericAttributeService = EngineContext.Current.Resolve<IGenericAttributeService>();

            var props = genericAttributeService.GetGenericAttributes(action, customer.Id, "", key, "", storeId);
            //little hack here (only for unit testing). we should write ecpect-return rules in unit tests for such cases

            if (props == null || string.IsNullOrEmpty(props.Value))
                return default(TPropType);

            return CommonHelper.To<TPropType>(props.Value);
        }
    }
}
