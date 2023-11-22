using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;


namespace DataBase {
    [System.Serializable]
    public class PlayerSkin
    {
        private static string ApiUrl = "player_skin_api.php";

        public string username = null;

        public int skin_id = 1;

        private static DatabaseManager db = DatabaseManager.getInstance();

        public static IEnumerator getAllSkins(string username, System.Action<bool, PlayerSkin[]> callback)
        {
            string jsonData = $"{{\"read\": 1, \"username\": \"{username}\"}}";

            using (UnityWebRequest www = db.createWebRequest(ApiUrl, "GET", jsonData))
            {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success) {
                    Debug.LogError(www.error);
                    callback(false, null);
                } else {
                    Debug.Log("[PlayerSkin] GET get all skin request has been sent!");
                    Debug.Log(www.downloadHandler.text);

                    HttpsResponse<PlayerSkin> response = JsonUtility.FromJson<HttpsResponse<PlayerSkin>>(www.downloadHandler.text);
                    Debug.Log(response.content);
                    if (response.state == true) {
                        if (response.content.Length == 0) {
                            Debug.Log(
                                "Wrong username."
                            );
                            callback(false, null);
                        } else {
                            callback(true, response.content);
                        }

                        
                    } else {
                        Debug.Log(
                            "Error code: " + response.error_code + "\n" +
                            "Error msg: " + response.msg);
                        callback(false, null);
                    }
                }
            }
        }

        public static IEnumerator create(string username, int skin_id, System.Action<bool> callback)
        {
            string jsonData = $"{{\"create\": 1, \"username\": \"{username}\", \"skin_id\": \"{skin_id}\"}}";
            Debug.Log("[PlayerSkin] POST create request: " + jsonData);
            using (UnityWebRequest www = db.createWebRequest(ApiUrl, "POST", jsonData))
            {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success) {
                    Debug.LogError(www.error);
                    callback(false);
                } else {
                    Debug.Log("[PlayerSkin] POST create request has been sent!");
                    Debug.Log(www.downloadHandler.text);

                    HttpsResponse<PlayerSkin> response = JsonUtility.FromJson<HttpsResponse<PlayerSkin>>(www.downloadHandler.text);
                    Debug.Log(response.content);
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

    }
}