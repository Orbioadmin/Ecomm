using Orbio.Core.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Services.Common
{
    public interface IPdfService
    {
        void PrintPackagingSlipsToPdf(Stream stream, IList<Shipment> shipments);
    }
}
