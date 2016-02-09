﻿using Orbio.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Services.Admin.ACL
{
   public interface IPermissionService
    {
       List<PermissionRecord> GetAllPermission();

       void UpdatePermissionRecord(PermissionRecord p);
        void RemovePermissionRecord(PermissionRecord p);
    }
}
