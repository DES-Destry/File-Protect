using AngleSharp.Html.Parser;
using System;

namespace FileProtect.Model.Parser
{
    class ParserWorker<T> where T : class
    {
        private IParser<T> parser;
        private IParserSettings settings;
        private HtmlLoader loader;
        private bool isActive;

        public bool IsActive { get { return isActive; } }
        public IParser<T> Parser
        {
            get
            {
                return parser;
            }
            set
            {
                parser = value;
            }
        }
        public IParserSettings Settings
        {
            get
            {
                return settings;
            }
            set
            {
                settings = value;
                loader = new HtmlLoader(value);
            }
        }

        public event Action<object, T> OnNewData;
        public event Action<object> OnCompleted;

        public ParserWorker(IParser<T> parser)
        {
            this.Parser = parser;
        }

        public ParserWorker(IParser<T> parser, IParserSettings settings) : this(parser)
        {
            this.Settings = settings;
        }

        public void Start()
        {
            isActive = true;
            Worker();
        }

        public void Abort()
        {
            isActive = false;
        }

        private async void Worker()
        {
            if (!isActive)
            {
                OnCompleted?.Invoke(this);
                return;
            }

            var source = await loader.GetSource();
            var docParser = new HtmlParser();

            var document = await docParser.ParseDocumentAsync(source);
            var result = parser.Parse(document);

            OnNewData(this, result);
            OnCompleted(this);
        }
    }
}
