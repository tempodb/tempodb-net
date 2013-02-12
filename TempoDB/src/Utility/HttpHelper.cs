using RestSharp;
using System.Collections.Generic;


namespace TempoDB.Utility
{
    internal static class HttpHelper
    {
        internal static string GetLinkFromHeaders(string name, IRestResponse response)
        {
            /// Get the next link from the Link header
            Parameter header = null;
            foreach(Parameter h in response.Headers)
            {
                if(h.Name.ToLower().Equals("link"))
                {
                    header = h;
                    break;
                }
            }

            Dictionary<string, Dictionary<string, string>> links = new Dictionary<string, Dictionary<string, string>>();
            if(header != null)
            {
                List<Dictionary<string, string>> l = ParseHeaderLinks(header.Value as string);
                foreach(Dictionary<string, string> link in l)
                {
                    string key = link.ContainsKey("rel") ? link["rel"] : link["url"];
                    links.Add(key, link);
                }
            }

            Dictionary<string, string> namedLink = new Dictionary<string, string>();
            string linkUrl = null;
            if(links.TryGetValue(name, out namedLink))
            {
                namedLink.TryGetValue("url", out linkUrl);
            }
            return linkUrl;
        }

        internal static List<Dictionary<string, string>> ParseHeaderLinks(string header)
        {
            char[] replaceChars = {' ', '\'', '"'};
            char[] replaceUrlChars = {'<', '>', ' ', '\'', '"'};
            List<Dictionary<string, string>> links = new List<Dictionary<string, string>>();
            foreach(string val in header.Split(','))
            {
                string[] items = val.Split(';');
                if(items.Length > 0)
                {
                    string url = items[0];
                    string parameters = items.Length > 1 ? items[1] : "";
                    Dictionary<string, string> link = new Dictionary<string, string>();
                    link.Add("url", url.Trim(replaceUrlChars));
                    foreach(string param in parameters.Split(';'))
                    {
                        string[] keys = param.Split('=');
                        if(keys.Length < 1) break;
                        string key = keys[0];
                        string item = keys[1];
                        link.Add(key.Trim(replaceChars).ToLower(), item.Trim(replaceChars));
                    }
                    links.Add(link);
                }
            }
            return links;
        }
    }
}
