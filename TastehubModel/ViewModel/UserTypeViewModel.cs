namespace TastehubModel.ViewModel
{
    public partial class UserTypeViewModel
    {
        public UserTypeViewModel()
        {
           
        }
        public long Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public bool IsActive { get; set; }
    }
}
