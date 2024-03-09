namespace AppService1.Models
{
    public class Configuration : System.Collections.Generic.Dictionary<string, object>
    {
        public static string env = Environment.GetEnvironmentVariable("ENVIRONMENT") ?? "Development";

        public Configuration()
        {
            int[] array = { 1, 2, 3};
            this.Add("array", array);
            this.Add("number", 6);
            this.Add("string", "six");
        }

        public bool Validate(Models.Configuration configuration)
        {
            return true;
        }
    }
}
