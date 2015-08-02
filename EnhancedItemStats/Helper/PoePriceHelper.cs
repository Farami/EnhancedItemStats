namespace EnhancedItemStats.Helper {
    using System;
    using System.Globalization;
    using System.Net;
    using System.Text;
    using System.Web;

    using EnhancedItemStats.Models;

    using HtmlAgilityPack;

    public class PoePriceHelper {
        public ChaosValue GetItemValue(string itemText) {
            var request = (HttpWebRequest)WebRequest.Create("http://www.poeprice.info/query");
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            var sb = new StringBuilder();
            AppendParameter(sb, "itemtext", itemText);

            var bytearray = Encoding.UTF8.GetBytes(sb.ToString());

            using (var requestStream = request.GetRequestStream()) {
                requestStream.Write(bytearray, 0, bytearray.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();

            return ParseMedianValue(response);
        }

        private ChaosValue ParseMedianValue(HttpWebResponse response) {
            var numberFormatInfo = new NumberFormatInfo { NumberDecimalSeparator = "." };

            var doc = new HtmlDocument();
            doc.Load(response.GetResponseStream());

            var valueTable = doc.DocumentNode.SelectSingleNode("//table[contains(@class, 'price')]");
            // TODO change logic to something more stable
            try {
                var minValue = double.Parse(valueTable.ChildNodes[1].ChildNodes[1].ChildNodes[0].InnerText.Split(new[] { " ~ " }, StringSplitOptions.RemoveEmptyEntries)[0], NumberStyles.AllowDecimalPoint, numberFormatInfo);
                var maxValue = double.Parse(valueTable.ChildNodes[1].ChildNodes[1].ChildNodes[0].InnerText.Split(new[] { " ~ " }, StringSplitOptions.RemoveEmptyEntries)[1], NumberStyles.AllowDecimalPoint, numberFormatInfo);
                var medianValue = double.Parse(valueTable.ChildNodes[3].ChildNodes[3].ChildNodes[0].InnerText.Split(' ')[1], NumberStyles.AllowDecimalPoint, numberFormatInfo);
                var meanValue = double.Parse(valueTable.ChildNodes[5].ChildNodes[3].ChildNodes[0].InnerText.Split(' ')[1], NumberStyles.AllowDecimalPoint, numberFormatInfo);
                return new ChaosValue { Min = minValue, Max = maxValue, Mean = meanValue, Median = medianValue };
            } catch (Exception) {
                return new ChaosValue();
            }
        }

        /// <summary>
        /// Append a url parameter to a string builder, url-encodes the value
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        protected void AppendParameter(StringBuilder sb, string name, string value) {
            var encodedValue = HttpUtility.UrlEncode(value);
            sb.AppendFormat("{0}={1}&", name, encodedValue);
        }
    }
}
