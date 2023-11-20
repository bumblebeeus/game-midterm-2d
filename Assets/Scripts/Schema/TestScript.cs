using System.Collections;
using System.Collections.Generic;
using DataBase;
using UnityEngine;
using UnityEngine.Networking;

public class TestScript: MonoBehaviour
{
    void Start()
    {
        StartCoroutine(test());
    }

    public IEnumerator test() {
        DatabaseManager db = DatabaseManager.getInstance();
        using (UnityWebRequest www = db.createWebRequest("db_connection.php", "GET", "{}")) {
            yield return www.SendWebRequest();
            Debug.Log(www.downloadHandler.text);
        }
    }

    void Update()
    {
    }
}
