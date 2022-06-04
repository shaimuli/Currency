using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;

namespace server.Controllers
{
    [ApiController]
    [EnableCors("AllowAllPolicy")]
    [Route("api/currency")]
    public class CurrencyController : ControllerBase
    {
        private readonly ILogger<CurrencyController> _logger;
        private readonly IConfiguration _config;

        public CurrencyController(ILogger<CurrencyController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        [HttpGet]
        public CurrenciesList Get()
        {
            return GetList();
        }

        private CurrenciesList GetList()
        {
            string url = _config.GetValue<string>("Data:Url");
            CurrenciesList currencies = ReadXmlToJson(url);
            if (currencies != null)
            {
                string jsonReader = string.Empty;
                using (StreamReader r = new StreamReader("./Currency.json"))
                {
                    jsonReader = r.ReadToEnd();
                }
            }
            else
            {
                //call the data from memory cache
            }
            return currencies;
        }

        private static CurrenciesList ReadXmlToJson(string url)
        {
            CurrenciesList currencies = null;
            WebRequest webRequest = HttpWebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();
            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
            {
                XmlDocument doc = new XmlDocument();
                string xml = sr.ReadToEnd();
                doc.LoadXml(xml);
                foreach (XmlNode node in doc)
                {
                    if (node.NodeType == XmlNodeType.XmlDeclaration)
                    {
                        doc.RemoveChild(node);
                    }
                }
                string jsonText = JsonConvert.SerializeXmlNode(doc);
                var json = JsonConvert.DeserializeObject(jsonText);
                System.IO.File.WriteAllText("Currency.json", jsonText, Encoding.UTF8);
                currencies = JsonConvert.DeserializeObject<CurrenciesList>(jsonText);
            }
            return currencies;
        }
    }
}