using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Sprotify.Web.Services
{
    public abstract class ApiServiceBase
    {
        private readonly SprotifyHttpClient _sprotifyclient;

        protected ApiServiceBase(SprotifyHttpClient sprotifyclient)
        {
            _sprotifyclient = sprotifyclient;
        }

        protected async Task<T> Get<T>(string resource)
        {
            var client = await _sprotifyclient.GetClient();
            var response = await client.GetAsync(resource).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                var errData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                throw new Exception(errData);
            }

            var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return JsonConvert.DeserializeObject<T>(json);
        }

        protected async Task<T> Post<T>(string resource, object data)
        {
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");

            var client = await _sprotifyclient.GetClient();
            var response = await client.PostAsync(resource, content).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                var errData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                throw new Exception(errData);
            }

            var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
