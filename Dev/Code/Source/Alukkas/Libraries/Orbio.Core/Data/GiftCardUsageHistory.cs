//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Orbio.Core.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class GiftCardUsageHistory
    {
        public int Id { get; set; }
        public int GiftCardId { get; set; }
        public int UsedWithOrderId { get; set; }
        public decimal UsedValue { get; set; }
        public System.DateTime CreatedOnUtc { get; set; }
    
        public virtual GiftCard GiftCard { get; set; }
        public virtual Order Order { get; set; }
    }
}