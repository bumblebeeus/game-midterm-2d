using UnityEngine;
using UnityEngine.Networking;
using System.Collections;


namespace DataBase {
    [System.Serializable]
    public class Player
    {
        private const string ApiUrl = "players.php";

        public string username;

        private string password;

        public int currentSkin;

        public int coins;

        // TODO: add access token
        // public string accessToken;
        
        private static DatabaseManager db = DatabaseManager.getInstance();
        private static Player instance = null;

        private Player() {}

        public static Player getInstance() {
            if (Player.instance is null) {
                Player.instance = new Player();
            }

            return Player.instance;
        }

        public bool loggedIn() {
            return true;
        }


        public void logOut() {

        }

        public class CertificateWhore : CertificateHandler
        {
            protected override bool ValidateCertificate(byte[] certificateData)
            {
                return true;
            }
        }

        private static UnityWebRequest createWebRequest(string url, string jsonData) {
            UnityWebRequest www = new UnityWebRequest(url, "POST");
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            // TODO: allow this only in testing
            // Hotfix: Set the custom certificate handler
            www.certificateHandler = new CertificateWhore();

            return www;
        }


        public static IEnumerator registerUser(string username, string password)
        {
            using (UnityWebRequest www = createWebRequest(db.getApiUrl() + "/" + ApiUrl, $"{{\"create\": 1, \"username\": \"{username}\", \"password\": \"{password}\" }}"))
            {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success) {
                    Debug.LogError(www.error);
                } else {
                    Debug.Log("POST request has been sent!");
                    Debug.Log(www.downloadHandler.text);

                    HttpsResponse<Player> response = JsonUtility.FromJson<HttpsResponse<Player>>(www.downloadHandler.text);

                }
            }           
        }
    }
}