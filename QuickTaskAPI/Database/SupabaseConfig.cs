using Microsoft.Extensions.Configuration;

namespace QuickTaskAPI.Database
{
    public class SupabaseConfig
    {
        public string Url { get; set; }
        public string Key { get; set; }

        public static SupabaseConfig FromConfiguration(IConfiguration configuration)
        {
            return new SupabaseConfig
            {
                Url = configuration["Supabase:Url"],
                Key = configuration["Supabase:Key"]
            };
        }
    }
} 