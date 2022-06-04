using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
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

        //call to the url currency and convert xml to json
        [HttpGet]
        public CurrenciesList Get()
        {
            string url = _config.GetValue<string>("Data:Url");
            CurrenciesList currencies = null;
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
                    currencies = JsonConvert.DeserializeObject<CurrenciesList>(jsonText);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return currencies;
        }
    }
}