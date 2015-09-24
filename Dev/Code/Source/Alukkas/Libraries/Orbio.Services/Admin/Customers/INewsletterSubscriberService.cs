using Orbio.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Services.Admin.Customers
{
    public interface INewsletterSubscriberService
    {
        List<NewsLetterSubscription> GetAllSubscribers(string Email);

        int UpdateSubscribers(int id, string email, bool active);

        int AddSubscribers(List<string> Emails);
    }
}
