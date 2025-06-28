namespace Web.Application.Interfaces.Caching
{
    public interface ICacheable
	{
		bool BypassCache { get; }
		string CacheKey { get; }
		TimeSpan? SlidingExpiration { get; }
	}
}
