using AngleSharp.Html.Dom;

namespace FileProtect.Model.Parser
{
    interface IParser<T> where T : class
    {
        T Parse(IHtmlDocument document);
    }
}
