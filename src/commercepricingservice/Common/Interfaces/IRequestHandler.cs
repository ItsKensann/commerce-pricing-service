namespace commercepricingservice.Common.Interfaces
{
    public interface IRequestHandler<in TRequest, TResult>
    {
        Task<TResult> HandleAsync(TRequest request);
    }
}
