using Orbio.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Services.Admin.Components
{
    public interface IProductComponentService
    {
        List<ProductComponent> GetProductComponent();

        ProductComponent GetProductComponentById(int Id);

        int DeleteProductComponent(int Id, string Email);

        int AddOrUpdateProductComponent(int Id, string Name, bool IsActive, bool IsVariablePrice, string Email);
    }
}
