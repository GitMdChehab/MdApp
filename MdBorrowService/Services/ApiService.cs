namespace MdBorrowService.Services
{
    using System;
    using System.Net.Http;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;

    public class ApiService()
    {
        public static async Task<T?> GetAsync<T>(string url, string token = null)
        {
            try
            {
                var _httpClient = new HttpClient();
                if (!string.IsNullOrEmpty(token))
                {
                    _httpClient.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                }

                HttpResponseMessage response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<T>();
                }
            }
            catch (Exception ex)
            {

            }
            return Activator.CreateInstance<T>();
        }
        public static async Task<T?> PostAsync<T>(string url, T payload, string token = null)
        {
            try
            {
                var _httpClient = new HttpClient();
                if (!string.IsNullOrEmpty(token))
                {
                    _httpClient.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                }

                string jsonPayload = JsonSerializer.Serialize(payload);
                HttpContent content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<T>();
                }
            }
            catch (Exception ex)
            {

            }
            return Activator.CreateInstance<T>();
        }
    }
}
