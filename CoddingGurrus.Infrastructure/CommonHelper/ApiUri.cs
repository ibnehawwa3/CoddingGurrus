
namespace CoddingGurrus.Infrastructure.CommonHelper
{
    public class ApiUri
    {
        public static AppSettings Info_API = GetAPIUrl();
        public static AppSettings GetAPIUrl()
        {
            AppSettings model = new AppSettings();
            string filePath = LogHelper.GetBaseDiretory() + "/appsettings.json";

            if (System.IO.File.Exists(filePath))
            {
                string json = System.IO.File.ReadAllText(filePath);
                model = Newtonsoft.Json.JsonConvert.DeserializeObject<JsonModel>(json).AppSettings;
            }
            return model;
        }
    }

    public class JsonModel
    {
        public AppSettings AppSettings { get; set; }
    }

    public class AppSettings
    {
        public string APIUrl { get; set; }
        public int Timeout { get; set; }
    }
}
