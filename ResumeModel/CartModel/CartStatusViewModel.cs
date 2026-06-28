using System;

namespace TastehubModel.ViewModel
{
   public class CategoryViewModel
    {
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public bool? IsActive { get; set; }

        public bool? IsDelete { get; set; }

        public string Image { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }
}
