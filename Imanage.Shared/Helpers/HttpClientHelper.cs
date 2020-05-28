using Dafmis.Shared.ViewModels;
using Imanage.Shared.ViewModels;
using System.Net.Http;
using System.Threading.Tasks;

namespace Imanage.Shared.Helpers
{
    public static class HttpClientHelper
    {
        public static async Task<ApiResponse<TResult>> PostAsJsonAsync<TModel, TResult>(this HttpClient client, string requestUri, TModel model)
        {
            var response = await client.PostAsJsonAsync(requestUri, model);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<ApiResponse<TResult>>();
        }

        public static async Task<ApiResponse<TResult>> DeleteAsync<TResult>(this HttpClient client, string requestUri)
        {
            var response = await client.DeleteAsync(requestUri);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<ApiResponse<TResult>>();
        }

        public static async Task<ApiResponse<TResult>> PutAsync<TModel, TResult>(this HttpClient client, string requestUri, TModel model)
        {
            var response = await client.PutAsJsonAsync(requestUri, model);
            return await response.Content.ReadAsAsync<ApiResponse<TResult>>();
        }

        public static async Task<ApiResponse<TResult>> GetAsync<TResult>(this HttpClient client, string requestUri)
        {
            var response = await client.GetAsync(requestUri);

            response.EnsureSuccessStatusCode();

            var str = response.Content.ReadAsStringAsync();
            return await response.Content.ReadAsAsync<ApiResponse<TResult>>();
        }
    }
}