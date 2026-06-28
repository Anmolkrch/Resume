using System;

namespace TastehubModel.CartModel
{
   public  class OrderDetailViewModel
    {
        public int ShippingDetailId { get; set; }
        public int MemberId { get; set; }
        public int AddressId { get; set; }
        public Nullable<decimal> AmountPaid { get; set; }
        public string PaymentType { get; set; }
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public string ProductImage { get; set; }
        public decimal Price { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
    }
}
