using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu]
public class MapInfo : ScriptableObject
{
    public Vector2 startPos;
    public Vector2 endPos;
    public string gameSceneName;

}
