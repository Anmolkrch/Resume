namespace Utility
{
    public class Enums
    {
        public enum AccountStatus
        {
            Active = 1,
            Inactive = 2,
            Suspended = 3,
            Deactivated = 4,
            Delete = 5
        }
        public enum Gender
        {
            Male = 1,
            Female = 2,
            Transgender = 3
        }
        public enum IsActive
        {
            Active = 1,
            Inactive = 0
        }

        public enum NotificationStatus
        {
            Initiated = 1,
            Sent = 2,
            Failed = 3

        }
        public enum MailType
        {
            AccountCreation = 1,
            AccountActivation = 2,
            SendAllDriver = 3
        }
       
        public enum BookingRequestStatus
        {
            All = 0,
            Initiated = 1,
            Modified = 2,
            Allocated = 3,
            ModifiedAllocation = 4,
            Completed = 5

        }
        public enum UserType
        {
            SuperAdmin = 1,
            Admin = 2,
            User = 3
        }
        public enum CartStatus
        {
            Addedtocart=1,
            Removedfromcart=2,
            Purchasedtheitem=3
        }
        public enum PaymentMode
        {
            PayOndelivery = 1,
            Card = 2,
            PayTm = 3,
            Wallet = 4
        }
        public enum Spices
        {
            mild=0,
            medium=1,
            indianhot=2,
            kiwihot=3
        }
    }
}
