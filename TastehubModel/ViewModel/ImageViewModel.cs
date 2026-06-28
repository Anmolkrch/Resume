using System;

namespace TastehubModel.ViewModel
{
    public class ImageCategoryViewModel
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string Description { get; set; }
    }

    public class ImageViewModel
    {
        public int ImageID { get; set; }
        public string ImageName { get; set; } = string.Empty;
        public string PathPrimary { get; set; } = string.Empty;
        public string PathSecondary { get; set; }
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }   // for display convenience
        public DateTime UploadDate { get; set; }
        public string UploadedBy { get; set; }
    }

}
