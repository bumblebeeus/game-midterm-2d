using UnityEngine;
using System.IO;

// This class manages the connection to a MySQL database using the MySqlConnector library.

namespace DataBase {
    public class DatabaseManager
    {
        // This class represents the configuration for the database connection.
        // The [System.Serializable] attribute makes this class able to be serialized.
        // Serialization is the process of converting the state of an object into a form that can be persisted or transported.
        [System.Serializable]
        public class DatabaseConfig
        {
            public string server; // The server name or IP address.
            public int port; // The port number.
            public bool pooling; // Whether connection pooling is enabled.

            private DatabaseConfig() {}

            static private DatabaseConfig instance = null;

            private static DatabaseConfig loadConfig() {
                EnvironmentConfig envConfig = EnvironmentConfig.getInstance();
                string dbConfigPath = Application.dataPath;

                if (envConfig.environment == "prod") {
                    dbConfigPath = string.Concat(dbConfigPath, "/", envConfig.databaseConfig.prod);
                } else {
                    dbConfigPath = string.Concat(dbConfigPath, "/", envConfig.databaseConfig.dev);
                }

                string json = File.ReadAllText(dbConfigPath);
                DatabaseConfig config = JsonUtility.FromJson<DatabaseConfig>(json);

                return config;
            }

            public static DatabaseConfig getInstance() {
                if (DatabaseConfig.instance is null) {
                    DatabaseConfig.instance = DatabaseConfig.loadConfig();
                }

                return DatabaseConfig.instance;
            }
        }

        private DatabaseConfig config; // The configuration for the database connection.

        private static DatabaseManager instance = null;
        
        private DatabaseManager() {}

        public static DatabaseManager getInstance() {
            if (DatabaseManager.instance is null) {
                DatabaseManager.instance = new DatabaseManager();
                DatabaseManager.instance.config = DatabaseConfig.getInstance();
            }

            return DatabaseManager.instance;
        }

        public string getApiUrl() {
            Debug.Log(config.server);
            return config.server;
        }
    }
}