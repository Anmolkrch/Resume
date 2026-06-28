using System;

namespace TastehubModel.ViewModel
{
   public class ShippingDetailsViewModel
    {
        public int ShippingDetailId { get; set; }

        public int MemberId { get; set; }

        public int AddressId { get; set; }

        public Nullable<decimal> AmountPaid { get; set; }

        public string PaymentType { get; set; }

    }
}
