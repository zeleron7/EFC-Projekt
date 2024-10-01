using Microsoft.Extensions.Configuration;

namespace Configuration;

public sealed class csAppConfig
{
    //configuration storage
    public const string Appsettingfile = "appsettings.json";

    public const string UserSecretId = "18a32a17-88a5-4e2f-a78b-37c90621261b";
                                

    #region Singleton design pattern
    private static readonly object instanceLock = new();

    private static csAppConfig _instance = null;
    private IConfigurationRoot _configuration = null;
    #endregion

    #region Configuration data structures
    private  DbSetDetail _dbSetActive = new DbSetDetail();
    private List<DbSetDetail> _dbSets = new List<DbSetDetail>();
    #endregion

    private csAppConfig()
    {
#if DEBUG
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");
#else        
        //Ensure that also docker environment has Development/Production detection
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Production");
#endif

        //Create final ConfigurationRoot, _configuration which includes also AzureKeyVault
        var builder = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory());
        
        builder = builder.AddUserSecrets(UserSecretId, reloadOnChange: true);

        //override with any locally set configuration from appsettings.json
        builder = builder.AddJsonFile(Appsettingfile, optional: true, reloadOnChange: true);
        _configuration = builder.Build();

        //get DbSet details, Note: Bind need the NuGet package Microsoft.Extensions.Configuration.Binder
        _configuration.Bind("DbSets", _dbSets); 

        //Set the active db set and fill in location and server into Login Details
        var i = int.Parse(_configuration["DbSetActiveIdx"]);
        _dbSetActive = _dbSets[i];
        _dbSetActive.DbLogins.ForEach(i =>
        {
            i.DbLocation = _dbSetActive.DbLocation;
            i.DbServer = _dbSetActive.DbServer;
        });
    }

    public static string ASPNETCOREEnvironment
    {
        get
        {
            //Just to ensure environment variable is set, by instance creation
            var t = Instance;
            
            return Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        }
    }
    #region Singleton design pattern
    private static csAppConfig Instance
    {
        get
        {
            lock (instanceLock)
            {
                if (_instance == null)
                {
                    _instance = new csAppConfig();
                }
                return _instance;
            }
        }
    }
    #endregion
    
    public static IConfigurationRoot ConfigurationRoot => Instance._configuration;
    public static DbSetDetail DbSetActive => Instance._dbSetActive;
    public static DbLoginDetail DbLoginDetails (string DbLogin)
    {
        if (string.IsNullOrEmpty(DbLogin) || string.IsNullOrWhiteSpace(DbLogin))
            throw new ArgumentNullException();

        var conn = Instance._dbSetActive.DbLogins.First(m => m.DbUserLogin.Trim().ToLower() == DbLogin.Trim().ToLower());
        if (conn == null)
            throw new ArgumentException("Database connection not found");

        return conn;
    }
}

#region types instaniated with configuration content
public class DbSetDetail
{
    public string DbLocation { get; set; }
    public string DbServer { get; set; }

    public List<DbLoginDetail> DbLogins { get; set; }
}

public class DbLoginDetail
{
    //set after reading in the active DbSet
    
    public string DbLocation { get; set; } = null;
    public string DbServer { get; set; } = null;

    public string DbUserLogin { get; set; }
    public string DbConnection { get; set; }
    public string DbConnectionString => csAppConfig.ConfigurationRoot.GetConnectionString(DbConnection);
}
#endregion

