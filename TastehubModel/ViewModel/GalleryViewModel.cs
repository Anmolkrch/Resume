using System.ComponentModel.DataAnnotations;

namespace TastehubModel.ViewModel
{
   public class GalleryViewModel
    {
        public long id { get; set; }
        [Required(ErrorMessage = "Description is required")]
        public string GalleryName { get; set; }

        public string GalleryPath { get; set; }

        public long TypeId { get; set; }

        public bool IsActive { get; set; }

        public System.DateTime CreatedDate { get; set; }

        public System.DateTime ModifiedDate { get; set; }

        public string CreatedBy { get; set; }

        public string ModifiedBy { get; set; }
        public string AlbumName { get; set; }
        public string Title { get; set; }
    }
}
