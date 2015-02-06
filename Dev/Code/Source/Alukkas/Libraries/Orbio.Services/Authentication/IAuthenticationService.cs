using Orbio.Core.Domain.Customers;

namespace Orbio.Services.Authentication
{
    /// <summary>
    /// Authentication service interface
    /// </summary>
    public partial interface IAuthenticationService
    {
        void SignIn(Customer customer, bool createPersistentCookie);
        void SignOut();
        string GetAuthenticatedCustomerData();
    }
}
