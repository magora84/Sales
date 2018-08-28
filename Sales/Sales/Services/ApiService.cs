
namespace Sales.Services {
    using Newtonsoft.Json;
    using Plugin.Connectivity;
    using Common.Models;
    using Helpers;
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    public class ApiService {

        public async Task<Response> CheckConnection() {

            if (!CrossConnectivity.Current.IsConnected) {
                return new Response {
                    IsSuccess = false,
                    Message = Languages.TurnOnInternet,
                };
            }
            var isReachable = await CrossConnectivity.Current.IsRemoteReachable("http://www.google.com");
            if (!isReachable) {
                return new Response { IsSuccess = false, Message = Languages.NoInternet, };
            }
            return new Response {
                IsSuccess = true,
        };
        }



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
