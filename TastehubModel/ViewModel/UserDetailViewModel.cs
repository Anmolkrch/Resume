using System;

namespace TastehubModel.ViewModel
{
    public class UserDetailViewModel
    {
        public int Id { get; set; }

        public Nullable<System.DateTime> DateOfBirth { get; set; }

        public Nullable<int> Gender { get; set; }

        public Nullable<int> CurCountry { get; set; }

        public Nullable<int> CurState { get; set; }

        public Nullable<int> CurCity { get; set; }

        public string CurZipCode { get; set; }

        public string CurAddress1 { get; set; }

        public string CurAddress2 { get; set; }

        public string ProfileUrl { get; set; }

        public Nullable<long> UserId { get; set; }

        public bool IsDefault { get; set; }
  
    }
}


