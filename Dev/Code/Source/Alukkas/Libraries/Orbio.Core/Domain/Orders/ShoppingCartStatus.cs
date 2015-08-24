
namespace Orbio.Core.Domain.Orders
{
    public enum ShoppingCartStatus : int
    {
        /// <summary>
        /// Cart
        /// </summary>
        Cart = 10,
        /// <summary>
        /// WishList
        /// </summary>
        WishList = 20,
        /// <summary>
        /// Address
        /// </summary>
        Address = 30,
        /// <summary>
        /// Payment
        /// </summary>
        Payment = 40
    }
}
