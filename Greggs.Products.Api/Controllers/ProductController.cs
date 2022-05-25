using System;
using System.Collections.Generic;
using System.Linq;
using Greggs.Products.Api.DataAccess;
using Greggs.Products.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Greggs.Products.Api.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IConfiguration _config;
        private static readonly string[] Products = new[]
        {
        "Sausage Roll", "Vegan Sausage Roll", "Steak Bake", "Yum Yum", "Pink Jammie"
    };

        private readonly ILogger<ProductController> _logger;
        private readonly IDataAccess<Product> _productAccess;

        public ProductController(ILogger<ProductController> logger, IConfiguration config, IDataAccess<Product> productAccess)
        {
            _logger = logger;
            _config = config;
            _productAccess = productAccess;
        }

        [HttpGet]
        public IEnumerable<Product> Get(int pageStart = 0, int pageSize = 5)
        {
            if (pageSize > Products.Length)
                pageSize = Products.Length;

            var exchangeRate = 1;

            if (!String.IsNullOrEmpty(_config["ExchangeRate"]))
            {
                exchangeRate = int.Parse(_config["ExchangeInfo:ConversionRate"]);
            }


            return _productAccess.List(null,null).Reverse().Select(x => new Product
            {
                Name = x.Name,
                PriceInPounds = x.PriceInPounds * exchangeRate
            }).Skip(pageStart).Take(pageSize).ToList();

        }
    }
}