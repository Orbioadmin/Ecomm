using Orbio.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Services.Admin.Sales
{
    public class GiftCardService : IGiftCardService
    {
        public List<GiftCard> GetAllGiftCards(int Activated, string CouponCode)
        {
            using(var context= new OrbioAdminContext())
            {
                bool Activate = (Activated == 2) ? false : true;

                var query = context.GiftCards.Include("GiftCardUsageHistories.Order").OrderBy(m=>m.Id).ToList();
                if (Activated >0)
                    query = query.Where(m => m.IsGiftCardActivated == Activate).OrderBy(m => m.Id).ToList();
                if(!string.IsNullOrEmpty(CouponCode))
                    query = query.Where(m => m.GiftCardCouponCode == CouponCode).OrderBy(m => m.Id).ToList();
                return query;
            }
        }

        public  int DeleteGiftCards(int id)
        {
            using (var context = new OrbioAdminContext())
            {
                try
                {
                    var query = context.GiftCards.Where(m => m.Id == id).FirstOrDefault();
                    if (query != null)
                    {
                        context.GiftCards.Remove(query);
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

        public GiftCard GetGiftCardById(int id)
        {
            using (var context = new OrbioAdminContext())
            {
                var query = context.GiftCards.Include("GiftCardUsageHistories.Order").Where(m => m.Id==id).FirstOrDefault();
                return query;
            }
        }

        public  int AddOrUpdateGiftCards(GiftCard giftCard)
        {
            try
            {
                using (var context = new OrbioAdminContext())
                {
                    var query = context.GiftCards.Where(m => m.Id == giftCard.Id).FirstOrDefault();
                    if (query == null)
                    {
                        query = context.GiftCards.FirstOrDefault();
                        query.GiftCardTypeId = giftCard.GiftCardTypeId;
                        query.Amount = giftCard.Amount;
                        query.IsGiftCardActivated = giftCard.IsGiftCardActivated;
                        query.GiftCardCouponCode = giftCard.GiftCardCouponCode;
                        query.RecipientName = giftCard.RecipientName;
                        query.RecipientEmail = giftCard.RecipientEmail;
                        query.SenderName = giftCard.SenderName;
                        query.SenderEmail = giftCard.SenderEmail;
                        query.Message = giftCard.Message;
                        query.IsRecipientNotified = giftCard.IsRecipientNotified;
                        query.CreatedOnUtc = giftCard.CreatedOnUtc;
                        context.GiftCards.Add(query);
                        context.SaveChanges();
                    }
                    else
                    {
                        query.GiftCardTypeId = giftCard.GiftCardTypeId;
                        query.Amount = giftCard.Amount;
                        query.IsGiftCardActivated = giftCard.IsGiftCardActivated;
                        query.GiftCardCouponCode = giftCard.GiftCardCouponCode;
                        query.RecipientName = giftCard.RecipientName;
                        query.RecipientEmail = giftCard.RecipientEmail;
                        query.SenderName = giftCard.SenderName;
                        query.SenderEmail = giftCard.SenderEmail;
                        query.Message = giftCard.Message;
                        query.IsRecipientNotified = giftCard.IsRecipientNotified;
                        context.SaveChanges();
                    }
                }
                return 1;
            }
            catch(Exception)
            {
                return 0;
            }
        }
    }
}

                