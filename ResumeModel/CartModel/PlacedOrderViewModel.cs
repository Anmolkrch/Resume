using System;

namespace TastehubModel.CartModel
{
    public class PlacedOrderViewModel
    {
        public Nullable<int> Items { get; set; }
        public int ShippingDetailId { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public string Email { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public string PhoneNumber { get; set; }
        public string PaymentType { get; set; }
    }
}
