using MySql.Data.MySqlClient;
using UnityEngine;
using System.IO;

public class DatabaseManager : MonoBehaviour
{
    [System.Serializable]
    public class DatabaseConfig
    {
        public string server;
        public int port;
        public string database;
        public string user_id;
        public string password;
        public bool pooling;
    }

    DatabaseConfig config;

    private void Start()
    {
        string configPath = Application.streamingAssetsPath + "/Configs/ProdDatabaseConfig.json";
        string json = File.ReadAllText(configPath);
        DatabaseConfig config = JsonUtility.FromJson<DatabaseConfig>(json);
        Connect(config);
    }

    private void Connect(DatabaseConfig config)
    {
        MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
        builder.Server = config.server;
        builder.Port = config.port;
        builder.UserID = config.user_id;
        builder.Password = config.password;
        builder.Database = config.database;

        try
        {
            using (MySqlConnection connection = new MySqlConnection(builder.ToString()))
            {
                connection.Open();
                print("MySQL - Opened Connection");
            }
        }
        catch (MySqlException exception)
        {
            print(exception.Message);
        }
    }
}
