namespace MyProject.Service.Helpers.OTP
{
    public class Messages
    {
        public Message[] messages { get; set; }
    }

    public class Message
    {
        public string clientRef { get; set; }
        public string number { get; set; }
        public string mask { get; set; }
        public string text { get; set; }
        public string campaignName { get; set; }

    }
}