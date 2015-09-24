using Orbio.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Services.Admin.Sales
{
    public interface IGiftCardService
    {
        List<GiftCard> GetAllGiftCards(int Activated,string CouponCode);

        GiftCard GetGiftCardById(int id);

        int DeleteGiftCards(int id);

        int AddOrUpdateGiftCards(GiftCard giftCard);
    }
}
