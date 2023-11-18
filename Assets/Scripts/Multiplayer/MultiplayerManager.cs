using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class MultiplayerManager : MonoBehaviour
{
    public MapInfo[] maps;
    public GameObject[] skins;
    public GameObject multiplayerPopup;
    public GameObject selectMapPopup;
    public GameObject winPopup;
    private Multiplayer multiplayer;
    private GameObject currentPlayer;
    private List<GameObject> otherPlayers = new();
    private float timer = 0;

    private uint selectedMap = 0;
    private bool isStart = false;
    
    private void MultiplayerHandle()
    {
        var root = multiplayerPopup.GetComponent<UIDocument>().rootVisualElement;
        var button = root.Q<Button>("Go");
        var textField = root.Q<TextField>("IP");
        var dropdownField = root.Q<DropdownField>("Type");
        
        dropdownField.choices = new List<string>()
        {
            "Server",
            "Client"
        };
        dropdownField.RegisterValueChangedCallback(async evt =>
        {
            if (evt.newValue.Equals("Server"))
            {
                var r = await multiplayer.CreateRoom();
                textField.value = r;
                textField.isReadOnly = true;
            }
            else
            {
                textField.value = "";
                textField.isReadOnly = false;
            }
        });

        button.clicked += async () =>
        {
            switch (dropdownField.text)
            {
                case "":
                    return; // Not selected anything
                case "Client":
                {
                    var roomId = textField.text;
                    await multiplayer.JoinRoom(roomId);
                    // TODO: replace with real skinId
                    await multiplayer.SendData("changeSkin", new { skinId = 0 });
                    break;
                }
                default:
                    // Start the game!
                    // Map select pop up open!
                    var mapId = await SelectMapHandle();
                    if (mapId == 100) return;
                    // And set the initial position
                    await multiplayer.SendData("changeMap", new { mapId = mapId });
                    // TODO: replace with real skinId
                    await multiplayer.SendData("changeSkin", new { skinId = 0 });
                    var startPos = maps[mapId].startPos;
                    await multiplayer.SendData("initPos", new {x=startPos.x, y=startPos.y, isFlip=false});
                    await multiplayer.SendData("start", new {});
                    break;
            }
            
            multiplayer.OnMapChange(mapId =>
            {
                selectedMap = mapId;
            });
            multiplayer.OnStart(SwitchScene);
        };

    }

    void SwitchScene()
    {
        StartCoroutine(LoadGameScene());
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

    void StartGame()
    {
        multiplayer.LoopThrough((sessionId, playerData) =>
        {
            // Instantiate current player first
            if (sessionId == multiplayer.GetCurrentSessionId())
            {
                currentPlayer = Instantiate(skins[playerData.skinId],
                    maps[selectedMap].startPos, Quaternion.identity).transform.GetChild(0).gameObject;
            }
            // then the rest
            else
            {
                var other = Instantiate(skins[playerData.skinId],
                    maps[selectedMap].startPos, Quaternion.identity);
                Destroy(other.transform.GetChild(1).gameObject);
                var t = other.transform.GetChild(0).gameObject;
                Destroy(t.GetComponent<Animator>());
                Destroy(t.GetComponent<CapsuleCollider2D>());
                Destroy(t.GetComponent<Rigidbody2D>());
                Destroy(t.GetComponent<Player>());
                Destroy(t.GetComponent<CameraFollow>());
                otherPlayers.Add(t);
            }
            
        });
       
       // Setup Callback
       multiplayer.OnChange((state, isFirst) =>
       {
           if (isFirst) return;
           int counter = 0;
           state.players.ForEach((sessionId, playerData) =>
           {
               if (sessionId.Equals(multiplayer.GetCurrentSessionId())) return;
               otherPlayers[counter].transform.position = 
                   new Vector2(playerData.x, playerData.y);
               otherPlayers[counter].transform.localScale = playerData.isFlip ? 
                   new Vector3(-1, 1, 1) : new Vector3(1, 1, 1);
               counter += 1;
           });
       });
       
       // check win
       multiplayer.OnWin((sessionIdWin) =>
       {
           if (sessionIdWin.Length == 0) return;
           isStart = false;
           var popup = Instantiate(winPopup, Vector3.zero, Quaternion.identity);
           var root = popup.GetComponent<UIDocument>().rootVisualElement;
           root.Q<Label>("Win").text = sessionIdWin == multiplayer.GetCurrentSessionId() ? "YOU WIN" : "OTHER WIN";
           var roundedTime = Mathf.FloorToInt(timer);
           root.Q<Label>("Time").text = $"Time taken: {Mathf.FloorToInt(timer/60)} minutes {timer % 60} seconds";
           StartCoroutine(JumpBackToMainMenu());
       });
       
       isStart = true;
    }
    
    private Task<uint> SelectMapHandle()
    {
        selectMapPopup.SetActive(true);
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

    // Start is called before the first frame update
    void Start()
    {
        multiplayer = new Multiplayer();
        DontDestroyOnLoad(gameObject);
        MultiplayerHandle();
    }

    // Update is called once per frame
    async void Update()
    {
        if (isStart)
        {
            var curPos = currentPlayer.transform.position;
            var isFlip = currentPlayer.transform.localScale.x < 0;
            await multiplayer.SendData("move", new {x = curPos.x, y=curPos.y, isFlip=isFlip});
            // Check win
            if (Vector2.SqrMagnitude((Vector2)curPos - maps[selectedMap].endPos) < 4f)
            {
                await multiplayer.SendData("declareWin", new {});
            }
        }
    }

    private void FixedUpdate()
    {
        if (isStart)
        {
            timer += Time.deltaTime;
        }
    }


    private void OnApplicationQuit()
    {
       multiplayer.Disconnect(); 
    }
}
