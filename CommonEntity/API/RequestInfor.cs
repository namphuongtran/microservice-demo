using System.Collections.Generic;

namespace Framework.Entities.API
{
    public class RequestInfor
    {
        public string UrlBase { set; get; }

        public HeaderValue HeaderValue { set; get; }

        public List<KeyValuePair<string, string>> FormValue { set; get; }
    }
}
