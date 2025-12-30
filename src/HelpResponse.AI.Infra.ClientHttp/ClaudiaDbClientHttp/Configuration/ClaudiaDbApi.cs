namespace HelpResponse.AI.Infra.ClientHttp.ClaudiaDbClientHttp.Configuration
{
    public class ClaudiaDbApi
    {
        public string Url { get; set; }
        public string Token { get; set; }
        public bool Count { get; set; }
        public string Select { get; set; }
        public int Top { get; set; }
        public string Filter { get; set; }
        public string Fields { get; set; }
        public string Kind { get; set; }
    }
}