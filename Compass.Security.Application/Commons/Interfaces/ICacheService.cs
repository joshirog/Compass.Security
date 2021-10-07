namespace Compass.Security.Application.Commons.Interfaces
{
    public interface ICacheService
    {
        string Template(string key);

        void Remove(string key);
    }
}