using UnityEngine;
using UnityEngine.Networking;
using System.Collections;


namespace DataBase {
    [System.Serializable]
    public class Skin
    {
        private static string ApiUrl = "skins_api.php";

        public int id = 0;

        public int price = 0;

        public string asset_path = null;

        private static DatabaseManager db = DatabaseManager.getInstance();

        public static IEnumerator listShopSkins(System.Action<bool, Skin[]> callback)
        {
            string jsonData = $"{{\"read\": 1}}";

            using (UnityWebRequest www = db.createWebRequest(ApiUrl, "GET", jsonData))
            {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success) {
                    Debug.LogError(www.error);
                    callback(false, null);
                } else {
                    Debug.Log("[Skin] GET list shop skins request has been sent!");
                    Debug.Log(www.downloadHandler.text);

                    HttpsResponse<Skin> response = JsonUtility.FromJson<HttpsResponse<Skin>>(www.downloadHandler.text);
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

    }
}