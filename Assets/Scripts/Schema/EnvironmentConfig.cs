using UnityEngine;
using System.IO;

namespace DataBase {
    [System.Serializable]
    public class EnvironmentConfig
    {
        public string environment;

        [System.Serializable]
        public class DatabasePath {
            public string prod;
            public string dev;
        }

        public DatabasePath databaseConfig;

        private EnvironmentConfig() {}

        private static EnvironmentConfig instance = null;

        public static EnvironmentConfig getInstance() {
            // JsonUtility does not support properties
            // https://stackoverflow.com/a/40161433
            if (EnvironmentConfig.instance is null) {
                string json = File.ReadAllText(Application.dataPath + "/Scripts/Configs/Environment.json");
                Debug.Log(json);
                EnvironmentConfig.instance = JsonUtility.FromJson<EnvironmentConfig>(json);
            }

            return EnvironmentConfig.instance;
        }

    }
}