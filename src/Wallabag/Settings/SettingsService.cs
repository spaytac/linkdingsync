using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Wallabag.Settings;

public class SettingYaml
{
    public List<ExcludedDomainPattern> excludedDomains { get; set; } = new ();
}

public class ExcludedDomainPattern
{
    public string name { get; set; }
    public string pattern { get; set; }
}

public class SettingsService
{
    private const string fileName = "data/config.yml";
    
    private static SettingYaml _settings = null;

    public static SettingYaml Settings
    {
        get
        {
            if (_settings == null)
            {
                Initialize();
            }
            
            return _settings;
        }
        private set
        {
            _settings = value;
        }
    }

    private static void Initialize()
    {
        var filePath = Path.Combine(Environment.CurrentDirectory, fileName);
        var fileInfo = new FileInfo(filePath);

        if (fileInfo.Exists)
        {
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance) // see height_in_inches in sample yml 
                .Build();
            
            var yml = File.ReadAllText(fileInfo.FullName);

            Settings = deserializer.Deserialize<SettingYaml>(yml);
        }
        else
        {
            Settings = new SettingYaml();
        }
    }
}