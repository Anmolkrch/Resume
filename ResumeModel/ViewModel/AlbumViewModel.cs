namespace TastehubModel.ViewModel
{
   public class AlbumViewModel
    {
        public long id { get; set; }

        public string AlbumName { get; set; }

        public bool IsActive { get; set; }

        public System.DateTime CreatedDate { get; set; }

        public System.DateTime ModifiedDate { get; set; }

        public string CreatedBy { get; set; }

        public string ModifiedBy { get; set; }

        public string AlbumImage { get; set; }

    }
}
