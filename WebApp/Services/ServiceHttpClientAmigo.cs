using System.Net.Http;

namespace WebApp.Services 
{
    public interface IServiceHttpClientAmigo
    {
        HttpClient GetClient();
    }
    public class ServiceHttpClientAmigo : IServiceHttpClientAmigo
    {
        public readonly HttpClient _apiClient;
        public ServiceHttpClientAmigo(HttpClient apiClient) => _apiClient = apiClient;
        public HttpClient GetClient() =>  _apiClient;
    }
}
