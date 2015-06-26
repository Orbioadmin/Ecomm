using Orbio.Core.Domain.Orders;

namespace Orbio.Services.Checkout
{
    public interface ITransientCartService
    {
        TransientCart GetTransientCart(int id, int customerId);
        int UpdateTransientCart(int id, int customerId, TransientCart cart);
    }
}
