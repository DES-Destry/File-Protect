namespace FileProtect.Model.Parser
{
    class AppSettings : IParserSettings
    {
        public string BaseUrl { get; set; } = "https://des-destry.github.io/index.html";
        public string Prefix { get; set; } = string.Empty;
        public int StartPoint { get; set; }
        public int EndPoint { get; set; }
    }
}
