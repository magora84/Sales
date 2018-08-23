
namespace Sales.Services {
    using Newtonsoft.Json;
    using Sales.Common.Models;
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    public class ApiService {
        public async Task<Response> GetList<T>(string urlBase, string prefix, string controller) {

            try {
                var cliente = new HttpClient();
                cliente.BaseAddress = new Uri(urlBase);
                var url = $"{prefix}{controller}";
                //var url =  string.Format("{0}{1}", prefix, controller);
                var response = await cliente.GetAsync(url);
                var answer = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode) {
                    return new Response {
                        IsSuccess = false,
                        Message = answer,
                    };
                }
                var list = JsonConvert.DeserializeObject<List<T>>(answer);
                return new Response {
                    IsSuccess= true,
                    Result = list,
                };
            }
            catch (Exception ex) {

                return new Response {
                    IsSuccess= false,
                    Message= ex.Message,

                };
            }

        
        }
    }

}
