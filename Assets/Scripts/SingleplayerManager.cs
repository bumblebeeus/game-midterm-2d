using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class SingleplayerManager : MonoBehaviour
{
    private DataBase.Player playerSchema = DataBase.Player.getCurrentPlayer();

    public MapInfo[] maps;
    public GameObject[] skins;
    public GameObject selectMapPopup;
    public GameObject winPopup;

    private GameObject currentPlayer;
    private bool isStart = false;

    private uint selectedMap = 100;
    private float timer = 0;
   
    private Task<uint> SelectMapHandle()
    {
        var root = selectMapPopup.GetComponent<UIDocument>().rootVisualElement;
        var tcs = new TaskCompletionSource<uint>();
        for (uint i = 0; i < 3; i++)
        {
            var i1 = i;
            root.Q<Button>($"Level{i+1}Btn").clicked += () =>
            {
                tcs.TrySetResult(i1);
                selectMapPopup.SetActive(false);
            };
        }
        root.Q<Button>("exitChooseLevelBtn").clicked += () =>
        {
            tcs.TrySetResult(100);
            Debug.Log("Exit button clicked");
            selectMapPopup.SetActive(false);
        };
        return tcs.Task;
    }
    
    IEnumerator LoadGameScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        // The Application loads the Scene in the background at the same time as the current Scene.
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(maps[selectedMap].gameSceneName, 
            LoadSceneMode.Additive);
        // Wait until the last operation fully loads to return anything
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        
        // Move the GameObject (you attach this in the Inspector) to the newly loaded Scene
        SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetSceneByName(maps[selectedMap].gameSceneName));
        // Unload the previous Scene
        SceneManager.UnloadSceneAsync(currentScene);
        StartGame();
    }

    void StartGame()
    {
        Debug.Log("Current skin: " + playerSchema.current_skin.ToString());
        currentPlayer = Instantiate(skins[playerSchema.current_skin - 1],
            maps[selectedMap].startPos, Quaternion.identity).transform.GetChild(0).gameObject;
        isStart = true;
    }
    
    IEnumerator JumpBackToMainMenu()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        yield return new WaitForSeconds(5);
        // The Application loads the Scene in the background at the same time as the current Scene.
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Menu", 
            LoadSceneMode.Additive);
        // Wait until the last operation fully loads to return anything
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        
        // Unload the previous Scene
        Destroy(gameObject);
        SceneManager.UnloadSceneAsync(currentScene);
    }
    // Start is called before the first frame update
    async void Start()
    {
        DontDestroyOnLoad(gameObject);
        selectedMap = await SelectMapHandle();
        StartCoroutine(selectedMap == 100 ? JumpBackToMainMenu() : LoadGameScene());
    }

    // Update is called once per frame
    void Update()
    {
        if (!isStart) return;
        var curPos = currentPlayer.transform.position;
        if (Vector2.SqrMagnitude((Vector2)curPos - maps[selectedMap].endPos) < 4f)
        {
           isStart = false;
           var popup = Instantiate(winPopup, Vector3.zero, Quaternion.identity);
           var root = popup.GetComponent<UIDocument>().rootVisualElement;
           var roundedTime = Mathf.FloorToInt(timer);
           root.Q<Label>("Time").text = $"Time taken: {Mathf.FloorToInt(timer/60)} minutes {timer % 60} seconds";
           StartCoroutine(JumpBackToMainMenu());
        }
    }
    private void FixedUpdate()
    {
        if (isStart)
        {
            timer += Time.deltaTime;
        }
    }
}
