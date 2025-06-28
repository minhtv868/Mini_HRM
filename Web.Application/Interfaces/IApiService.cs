namespace Web.Application.Interfaces
{
    public interface IApiService
    {
        Task<TResponse> GetRequestAsync<TResponse, TRequest>(string requestUri, object parameters = null, string apiKey = "");
        Task<TResponse> PostRequestAsync<TResponse, TRequest>(string requestUri, TRequest data, string apiKey = "");
    }
}
