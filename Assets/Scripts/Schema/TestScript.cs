using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript: MonoBehaviour
{
    void Start()
    {
        StartCoroutine(DataBase.Player.registerUser("test", "test"));

    }

    void Update()
    {
    }
}
