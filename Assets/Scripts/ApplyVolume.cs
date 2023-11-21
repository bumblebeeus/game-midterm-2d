using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ApplyVolume : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AudioListener.volume = SettingsManager.Instance.Volume;
    }
}
