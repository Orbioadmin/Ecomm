using Orbio.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Services.Admin.Components
{
    public interface IPriceComponentService
    {
        List<PriceComponent> GetPriceComponent();

        PriceComponent GetPriceComponentById(int Id);

        int DeletePriceComponent(int Id,string Email);

        int AddOrUpdatePriceComponent(int Id, string Name, bool IsActive, string Email);
    }
}
