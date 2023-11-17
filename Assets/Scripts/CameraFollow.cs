using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private GameObject player;

    void Start()
    {
        player = GameObject.Find("Character");
        if (player == null)
        {
            player = GameObject.Find("Character2");
        }
    }

    void Update()
    {
        var position = transform.position;
        position = new Vector3(
            position.x,
            Mathf.Lerp(position.y, player.gameObject.transform.position.y + 1, 0.01f),
            -10
        );
        transform.position = position;
    }
}
