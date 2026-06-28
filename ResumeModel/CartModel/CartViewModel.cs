using System;

namespace TastehubModel.ViewModel
{
   public class CartViewModel
    {
        public CartViewModel()
        {
            this.Tbl_Product = new ProductViewModel();
        }
        public int CartId { get; set; }

        public int? ProductId { get; set; }

        public int? MemberId { get; set; }

        public int? CartStatusId { get; set; }

        public DateTime? AddedOn { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public int? ShippingDetailId { get; set; }

        public virtual ProductViewModel Tbl_Product { get; set; }
        public string Spices { get; set; }
    }
    public  class CartSpiceModel
    {
        public int CartId { get; set; }

        public string Spices { get; set; }
    }
}
