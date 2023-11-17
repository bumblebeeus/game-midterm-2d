using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Camera camera;

    void Start()
    {
        camera = Camera.main;
    }

    void Update()
    {
        if (camera == null) // Heavy loaded operation
        {
            camera = Camera.main;
        }
        var position = camera.transform.position;
        position = new Vector3(
            position.x,
            Mathf.Lerp(position.y, transform.position.y + 1, 0.01f),
            -10
        );
        camera.transform.position = position;
    }
}
