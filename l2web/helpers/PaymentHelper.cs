
using l2web.helpers.contracts;
using Microsoft.Extensions.Configuration;
using PayPalCheckoutSdk.Core;
using PayPalCheckoutSdk.Orders;
using PayPalHttp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;


namespace l2web.helpers
{
    public class PaymentHelper:IPaymentHelper
    {
        private readonly IConfiguration _config;

        public PaymentHelper(IConfiguration config) {
            _config = config;
        }

        

       
    }
}
