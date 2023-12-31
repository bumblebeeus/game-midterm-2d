using UnityEngine;
using UnityEngine.Networking;
using System.Collections;


namespace DataBase {
    [System.Serializable]
    public class Player
    {
        private static string ApiUrl = "players_api.php";

        public string username = null;

        private string password = null;

        public int current_skin = 1;

        public int coins = 0;
        
        private static DatabaseManager db = DatabaseManager.getInstance();

        private static Player currentPlayer = new DataBase.Player();

        private void  saveCurrentInfo(Player other) {
            this.username = other.username;
            this.password = other.password;
            this.current_skin = other.current_skin;
            this.coins = other.coins;
        }

        public IEnumerator login(string username, string password, System.Action<bool> callback)
        {
            string jsonData = $"{{\"read\": 1, \"username\": \"{username}\", \"password\": \"{password}\" }}";

            using (UnityWebRequest www = db.createWebRequest(ApiUrl, "GET", jsonData))
            {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success) {
                    Debug.LogError(www.error);
                    callback(false);
                } else {
                    Debug.Log("[Player] GET login request has been sent!");
                    Debug.Log(www.downloadHandler.text);

                    HttpsResponse<Player> response = JsonUtility.FromJson<HttpsResponse<Player>>(www.downloadHandler.text);
                    Debug.Log(response.content);
                    if (response.state == true) {
                        if (response.content.Length == 0) {
                            Debug.Log(
                                "Wrong username or password."
                            );
                            callback(false);
                        } else {
                            // Https response does not include password
                            Debug.Log(response.content[0].current_skin);
                            currentPlayer.saveCurrentInfo(response.content[0]);
                            currentPlayer.password = password;

                            callback(true);
                        }

                        
                    } else {
                        Debug.Log(
                            "Error code: " + response.error_code + "\n" +
                            "Error msg: " + response.msg);
                        callback(false);
                    }
                }
            }
        }

        public bool loggedIn() {
            return username is not null;
        }

        public void logOut() {
            username = null;
            password = null;
        }

        public IEnumerator updateBasicInfo(System.Action<bool> callback) {
            string jsonData = $"{{\"update\": 1, \"username\": \"{this.username}\", \"current_skin\": \"{this.current_skin}\", \"coins\": \"{this.coins}\"}}";
            Debug.Log(jsonData);
            using (UnityWebRequest www = db.createWebRequest(ApiUrl, "POST", jsonData))
            {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success) {
                    Debug.LogError(www.error);
                    callback(false);
                } else {
                    Debug.Log("[Player] POST update basic info request has been sent!");
                    Debug.Log(www.downloadHandler.text);

                    HttpsResponse<Player> response = JsonUtility.FromJson<HttpsResponse<Player>>(www.downloadHandler.text);
                    if (response.state == true) {
                        callback(true);
                    } else {
                        Debug.Log(
                            "Error code: " + response.error_code + "\n" +
                            "Error msg: " + response.msg);

                        callback(false);
                    }
                }
            }
        }

        public static IEnumerator signUp(string username, string password, System.Action<bool> callback)
        {
            string jsonData = $"{{\"create\": 1, \"username\": \"{username}\", \"password\": \"{password}\" }}";
            using (UnityWebRequest www = db.createWebRequest(ApiUrl, "POST", jsonData))
            {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success) {
                    Debug.LogError(www.error);
                    callback(false);
                } else {
                    Debug.Log("POST sign up request has been sent!");
                    Debug.Log(www.downloadHandler.text);

                    HttpsResponse<Player> response = JsonUtility.FromJson<HttpsResponse<Player>>(www.downloadHandler.text);
                    
                    if (response.state == true) {
                        callback(true);
                    } else {
                        Debug.Log(
                            "Error code: " + response.error_code + "\n" +
                            "Error msg: " + response.msg);

                        callback(false);
                    }
                }
            }           
        }

        public static Player getCurrentPlayer() {
            if (Player.currentPlayer == null) {
                Player.currentPlayer = new Player();
            }

            return Player.currentPlayer;
        }

    }
}