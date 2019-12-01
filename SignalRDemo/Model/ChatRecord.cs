namespace SignalRDemo.Model
{
    public class ChatRecord
    {
        public string Sender { get; set; }
        public string Receiver { get; set; }
        public string Message { get; set; }
        public string Date { get; set; }
        public bool IsSend { get; set; }
    }
}
