namespace BitocCore1
{
    public class Ut
    {
        public static string GetConnetString()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            IConfiguration Configuration = builder.Build();
            return Configuration.GetConnectionString("SqlConnString");
        }
    }
}
