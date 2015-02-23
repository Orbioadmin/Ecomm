using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Core.Domain.Customers
{
    public enum CustomerRegistrationResult : int
    {  
        /// <summary>
        /// Existing User
        /// </summary>
        ExistingUser = 2,

        /// <summary>
        /// New User
        /// </summary>
        NewUser =1,

        /// <summary>
        /// Search Engine
        /// </summary>
        SearchEngine = 3,

        /// <summary>
        /// Background Task
        /// </summary>
        BackgroundTask = 4,

        /// <summary>
        ///  Valid Email
        /// </summary>
        InvalidEmail = 5,

        /// <summary>
        /// Provide Password
        /// </summary>
        ProvidePassword = 6,
    }
}
