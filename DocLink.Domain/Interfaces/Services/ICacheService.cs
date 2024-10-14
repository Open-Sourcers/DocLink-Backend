namespace DocLink.Domain.Interfaces.Services
{
	public interface ICacheService
	{
		T? GetData<T>(string key);
		bool SetData<T>(string Key, T Value, TimeSpan ExpirationTime);
		void RemoveData(string Key);
	}
}
