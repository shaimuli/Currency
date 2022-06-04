using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using server.Helper;
using System;
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
            ReadXmlToJson(); //call from Offline(task) function to update the json file to update the json every x time.
            return GetList();
        }

        private CurrenciesList GetList()
        {
            CurrencyCache cache = new CurrencyCache();
            double duration = 60;
            CurrenciesList currencies = null;
            if (duration > 0 && cache.Exists())
            {
                currencies = cache.Get();
            }
            else
            {
                string jsonReader = string.Empty;
                using (StreamReader r = new StreamReader("./Currency.json"))
                {
                    jsonReader = r.ReadToEnd();
                    currencies = JsonConvert.DeserializeObject<CurrenciesList>(jsonReader);
                    cache.Set(currencies);
                }
            }
            return currencies;
        }

        private void ReadXmlToJson()
        {
            string url = _config.GetValue<string>("Data:Url");
            try
            {
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
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
    }
}