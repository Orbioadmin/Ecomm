using Orbio.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Orbio.Web.UI.Areas.Admin.Models.Sales
{
    public class GiftCardModel
    {
        public GiftCardModel()
        {
            GiftCardUsage = new List<GiftCardUsageHistoryModel>();
            GiftCardTypes = new List<SelectListItem>();
        }

        public GiftCardModel(GiftCard gc)
        {
            Id = gc.Id;
            GiftCardType=gc.GiftCardTypeId;
            InitialValue = gc.Amount;
            CouponCode = gc.GiftCardCouponCode;
            IsGiftCardActivated = gc.IsGiftCardActivated;
            RecipientName = gc.RecipientName;
            RecipientEmail = gc.RecipientEmail;
            SenderName = gc.SenderName;
            SenderEmail = gc.SenderEmail;
            Message = gc.Message;
            IsRecipientNotified = gc.IsRecipientNotified;
            CreationDate = gc.CreatedOnUtc;

            GiftCardUsage = (from uh in gc.GiftCardUsageHistories
                                    select new GiftCardUsageHistoryModel()
                                    {
                                        Id = uh.Id,
                                        UsedAmount = uh.UsedValue,
                                        OrderId = uh.Order.Id,
                                        UsedOn = uh.CreatedOnUtc,
                                    }).ToList();
            RemainingAmount = (GiftCardUsage != null) ? gc.Amount - GiftCardUsage.AsEnumerable().Sum(m => m.UsedAmount) : 0;
        }
        public int Id { get; set; }

        public int GiftCardType { get; set; }

        public decimal InitialValue { get; set; }

        public decimal RemainingAmount { get; set; }

        public string CouponCode { get; set; }

        public bool IsGiftCardActivated { get; set; }

        public string RecipientName { get; set; }

        public string RecipientEmail { get; set; }

        public string SenderName { get; set; }

        public string SenderEmail { get; set; }

        public string Message { get; set; }

        public bool IsRecipientNotified { get; set; }

        public DateTime CreationDate { get; set; }

        public IList<SelectListItem> GiftCardTypes { get; set; }

        public List<GiftCardUsageHistoryModel> GiftCardUsage { get; set; }
    }
}