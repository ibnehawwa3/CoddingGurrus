namespace CoddingGurrus.Core.Interface
{
    public interface IBaseHandler
    {
        Task<TResponse> PostAsync<TRequest, TResponse>(TRequest request, string apiEndpoint);
        Task<TResponse> GetAsync<TResponse>(string apiEndpoint);
        Task<TResponse> DeleteAsync<TResponse>(string apiEndpoint);
        Task<TResponse> GetByIdAsync<TResponse>(string apiEndpoint, long id);
        Task<TResponse> GetByGuidIdAsync<TResponse>(string apiEndpoint, string id);
    }
}
