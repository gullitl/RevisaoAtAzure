using System.Net.Http;

namespace WebApp.Services 
{
    public interface IServiceHttpClientPaisEstado
    {
        HttpClient GetClient();
    }

    public class ServiceHttpClientPaisEstado : IServiceHttpClientPaisEstado
    {
        public readonly HttpClient _apiClient;
        public ServiceHttpClientPaisEstado(HttpClient apiClient) => _apiClient = apiClient;
        public HttpClient GetClient() =>  _apiClient;
    }
}
