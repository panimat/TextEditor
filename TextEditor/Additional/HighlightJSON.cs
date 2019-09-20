using System.Text.RegularExpressions;


namespace TextEditor.Additional
{
    static class HighlightJSON
    {
        public static string SyntaxHighlightJson(string original)
        {
            return Regex.Replace(
              original,
              @"(¤(\\u[a-zA-Z0-9]{4}|\\[^u]|[^\\¤])*¤(\s*:)?|\b(true|false|null)\b|-?\d+(?:\.\d*)?(?:[eE][+\-]?\d+)?)".Replace('¤', '"'),
              match => {
                  var cls = "number";
                  if (Regex.IsMatch(match.Value, @"^¤".Replace('¤', '"')))
                  {
                      if (Regex.IsMatch(match.Value, ":$"))
                      {
                          cls = "key";
                      }
                      else
                      {
                          cls = "string";
                      }
                  }
                  else if (Regex.IsMatch(match.Value, "true|false"))
                  {
                      cls = "boolean";
                  }
                  else if (Regex.IsMatch(match.Value, "null"))
                  {
                      cls = "null";
                  }
                  return "<span class=\"" + cls + "\">" + match + "</span>";
              });
        }
    }
}
