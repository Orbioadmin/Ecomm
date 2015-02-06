using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Core.Domain.Customers
{
    public enum PasswordFormat : int
    {
        Clear = 0,
        Hashed = 1,
        Encrypted = 2
    }
}
