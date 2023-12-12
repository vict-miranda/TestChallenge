using Newtonsoft.Json;
using System.Text;

namespace RestServerCustomers.Tests
{
    public class ContentHelper
    {
        public static StringContent GetStringContent(object obj)
           => new StringContent(JsonConvert.SerializeObject(obj), Encoding.Default, "application/json");
    }
}
