using SPAproj.Models.Data;
using Microsoft.Extensions.DependencyInjection;

namespace SPAproj.Models.Service
{
    public class ConfigurationService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private static Dictionary<string, string> _cache;

        public ConfigurationService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
            if (_cache == null)
            {
                LoadParameters();
            }
        }

        private void LoadParameters()
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                _cache = dbContext.ConfigurationParameters.ToDictionary(p => p.Key, p => p.Value);
            }
        }

        public string GetParameterValue(string key)
        {
            if (_cache.ContainsKey(key))
            {
                return _cache[key];
            }
            return null; 
        }

        public void ReloadParameters()
        {
            LoadParameters();
        }
    }

}

