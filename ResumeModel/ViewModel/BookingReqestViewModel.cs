using System;

namespace TastehubModel.ViewModel
{
    public class BookingReqestViewModel
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string PhoneNo { get; set; }

        public int People { get; set; }

        public int Status { get; set; }

        public DateTime Date { get; set; }

        public string Time { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}
