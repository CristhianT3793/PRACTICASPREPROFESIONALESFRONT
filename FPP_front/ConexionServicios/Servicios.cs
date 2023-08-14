using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FPP_front.ConexionServicios
{
    public class Servicios
    {
        static readonly conexionServicios con = new conexionServicios();
        readonly  string url = con.url;
        public async Task<bool> GenericPost<T>(T dto, string uri)
        {
            var mycontent = JsonConvert.SerializeObject(dto);
            var stringContent = new StringContent(mycontent, UnicodeEncoding.UTF8, "application/json");
            var client = new HttpClient();
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage res = await client.PostAsync(uri, stringContent);
            if (res.IsSuccessStatusCode)
            {
                var response = res.Content.ReadAsStringAsync().Result;
                return true;
            }
            else
                return false;
        }
        public async Task<int> GenericPostId<T>(T dto, string uri)
        {
            int id = -1;
            var mycontent = JsonConvert.SerializeObject(dto);
            var stringContent = new StringContent(mycontent, UnicodeEncoding.UTF8, "application/json");
            var client = new HttpClient();
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage res = await client.PostAsync(uri, stringContent);
            if (res.IsSuccessStatusCode)
            {
                id = Convert.ToInt32(res.Content.ReadAsStringAsync().Result);
                return id;
            }
            else
                return id;
        }
        public async Task<string> GenericGet(string uri)
        {
            string error = "error";
            try
            {
                var client = new HttpClient
                {
                    BaseAddress = new Uri(url)
                };
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage res = await client.GetAsync(uri);
                if (res.IsSuccessStatusCode)
                {
                    var empResponse = res.Content.ReadAsStringAsync().Result;
                    return empResponse;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return error;
        }
        public async Task<bool> GenericPut<T>(T dto, string uri)
        {
            var mycontent = JsonConvert.SerializeObject(dto);
            var stringContent = new StringContent(mycontent, UnicodeEncoding.UTF8, "application/json");
            var client = new HttpClient();
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage res = await client.PutAsync(uri, stringContent);
            if (res.IsSuccessStatusCode)
            {
                var response = res.Content.ReadAsStringAsync().Result;
                return true;
            }
            else
                return false;
        }

    }
}