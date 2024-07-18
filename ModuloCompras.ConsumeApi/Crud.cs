using Newtonsoft.Json;
using System.Text;

namespace ModuloCompras.ConsumeApi
{

    public static class Crud<T>
    {
        public static T Create(string urlApi, T data)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(urlApi);
                client.DefaultRequestHeaders.Accept.Add(
                    System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/json")
                    );

                var json = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                var request = new HttpRequestMessage(HttpMethod.Post, urlApi);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = client.SendAsync(request);
                response.Wait();

                json = response.Result.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<T>(json);

                return result;
            }
        }
        //Modificado
        public static T[] Read(string urlApi)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var response = client.GetStringAsync(urlApi);
                    response.Wait();

                    var json = response.Result;
                    var result = JsonConvert.DeserializeObject<T[]>(json);
                    return result;
                }
            }
            catch (AggregateException ex)
            {
                foreach (var innerEx in ex.InnerExceptions)
                {
                    Console.WriteLine(innerEx.Message);
                }
                throw;
            }
            catch (HttpRequestException httpEx)
            {
                Console.WriteLine(httpEx.Message);
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        //* Hasta aqui

        public static T Read_ById(string urlApi, int id)
        {
            using (HttpClient client = new HttpClient())
            {
                urlApi = urlApi + "/" + id;
                var response = client.GetStringAsync(urlApi);
                response.Wait();

                var json = response.Result;
                var result = JsonConvert.DeserializeObject<T>(json);
                return result;
            }
        }

        public static bool Update(string urlApi, int id, T data)
        {
            using (HttpClient client = new HttpClient())
            {
                urlApi += "/" + id;
                client.BaseAddress = new Uri(urlApi);
                client.DefaultRequestHeaders.Accept.Add(
                    System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/json")
                );

                var json = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                var request = new HttpRequestMessage(HttpMethod.Put, urlApi);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = client.SendAsync(request);
                response.Wait();

                json = response.Result.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<T>(json);

                return true;
            }
        }

        public static bool Delete(string urlApi, int id)
        {
            using (HttpClient client = new HttpClient())
            {
                urlApi = urlApi + "/" + id;
                client.BaseAddress = new Uri(urlApi);
                client.DefaultRequestHeaders.Accept.Add(
                    System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/json"));

                var response = client.DeleteAsync(urlApi);
                response.Wait();
                return true;
            }
        }
    }
}