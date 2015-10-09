using Orbio.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Services.Admin.Customers
{
    public class NewsletterSubscriberService : INewsletterSubscriberService
    {
        public List<NewsLetterSubscription> GetAllSubscribers(string Email)
        {
            using (var context = new OrbioAdminContext())
            {
                var result = context.NewsLetterSubscriptions.OrderByDescending(m => m.Id).ToList();
                if (!string.IsNullOrEmpty(Email))
                    result = result.Where(m => m.Email.Contains(Email)).ToList();
                return result;
            }
        }

        public int UpdateSubscribers(int id, string email, bool active)
        {
            using (var context = new OrbioAdminContext())
            {
                try
                {
                    var result = context.NewsLetterSubscriptions.Where(m => m.Id == id).FirstOrDefault();
                    if (result != null)
                    {
                        result.Email = email;
                        result.Active = active;
                        context.SaveChanges();
                    }
                    return 1;
                }
                catch(Exception)
                {
                    return 0;
                }
            }
        }

        public  int AddSubscribers(List<string> Emails)
        {
            using (var context = new OrbioAdminContext())
            {
                try
                {
                    foreach (var email in Emails)
                    {

                        var result = context.NewsLetterSubscriptions.Where(m => m.Email == email).FirstOrDefault();
                        if (result == null)
                        {
                            var query = context.NewsLetterSubscriptions.FirstOrDefault();
                            query.Email = email;
                            query.Active = true;
                            query.CreatedOnUtc = DateTime.UtcNow;
                            context.NewsLetterSubscriptions.Add(query);
                            context.SaveChanges();
                        }
                    }
                    return 1;
                }

                catch (Exception)
                {
                    return 0;
                }
            }
        }

        public void Delete(int id)
        {
            using (var context = new OrbioAdminContext())
            {
                  var result = context.NewsLetterSubscriptions.Where(m => m.Id == id).FirstOrDefault();
                  if (result != null)
                  {
                      context.NewsLetterSubscriptions.Remove(result);
                      context.SaveChanges();
                  }
            }
        }
    }
}

                       