using AngleSharp.Html.Dom;
using System;
using System.Linq;

namespace FileProtect.Model.Parser
{
    class AppParser : IParser<string>
    {
        public string Parse(IHtmlDocument document)
        {
            try
            {
                var pItem = document.QuerySelectorAll("p").Where(item => item.ClassName != null && item.Id != null && item.ClassName.Contains("app_current_version") && item.Id.Contains("file_protect")).FirstOrDefault();
                return pItem.TextContent;
            }
            catch(Exception ex)
            {
                ErrorWriter.WriteError(ex);
                return string.Empty;
            }
        }
    }
}
