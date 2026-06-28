namespace TastehubModel.ViewModel
{
   public class MainMenuViewModel
    {
        public int Id { get; set; }

        public string MainMenu { get; set; }

        public string MenuDescription { get; set; }

        public bool IsActive { get; set; }

        public System.DateTime CreatedDate { get; set; }

        public System.DateTime ModifiedDate { get; set; }

        public string CreatedBy { get; set; }

        public string ModifiedBy { get; set; }
        
        //public virtual ICollection<tblSubMenu> tblSubMenus { get; set; }
    }
}
