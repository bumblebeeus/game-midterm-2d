using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ASyncLoad : MonoBehaviour
{
    public GameObject loadingScene;
    public Slider loadingSlider;

    public void loadScene(int sceneId)
    {
        StartCoroutine(loadLevelAsync(sceneId));
    }

    private IEnumerator loadLevelAsync(int sceneId)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneId);

        loadingScene.SetActive(true);

        while (!loadOperation.isDone)
        {
            float progress = Mathf.Clamp01(loadOperation.progress / .9f);
            
            Debug.Log("Loading progress: " + progress);
            loadingSlider.value = progress;
            yield return null;
        }
    }
}
