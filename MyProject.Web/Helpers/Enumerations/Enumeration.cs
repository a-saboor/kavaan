
namespace MyProject.Web.Helpers.Enumerations
{
    public class Enumeration
    {
        public const string en_ae  = "en-ae";
        public const string ar_ae = "ar-ae";

        public class Cultures{
        }

        public enum EmailTemplate
        {
            ForgotPassword = 1,
            Feedback = 2,
            Payment = 3,
        }

        public enum EmailLang
        {
            English = 1,
            Arabic = 2,
        }

        public enum UserRoles
        {
            Admin = 1,
            Operator = 2,
            Editor = 3,
            Viewer = 4,
        }

        //public enum BookingStatus
        //{
        //    Pending = 1,
        //    Confirmed = 2,
        //    AwaitingConfirmation = 3,
        //    Canceled = 4,
        //    Expired = 5,
        //}

        public enum BookingStatus
        {
            Pending = 1,
            Authorized = 2,
            Captured = 3,
            Cancelled = 4,
        }

        public enum CarApprovalStatus
        {
            Pending = 1,
            Processing = 2,
            Approved = 3,
            Rejected = 4,
        }

        public enum InvoiceStatus
        {
            Sent = 1,
            Paid = 2,
            Void = 3,
            WriteOff = 4,
            Draft = 5,
            UnPaid = 6,
            PaidViaCash = 7,
        }
        public enum ProductApprovalStatus
        {
            Pending = 1,
            Processing = 2,
            Approved = 3,
            Rejected = 4,
        }
    }
}