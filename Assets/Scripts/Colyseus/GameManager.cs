using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public GameObject popup;

    public GameObject mainGame;
    public GameObject currentPlayer;
    public GameObject otherPlayerTemplate;
    private Multiplayer multiplayer;

    private List<GameObject> otherPlayers;
    private bool isStart;
    private void UIHandle()
    {
        var root = popup.GetComponent<UIDocument>().rootVisualElement;
        var button = root.Q<Button>("Go");
        var label = root.Q<Label>("Status");
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
                    break;
                }
                default:
                    // Start the game!
                    // Map select pop up open!
                    // And set the initial position
                    await multiplayer.SendData("initPos", new {x=-2.0, y=0, isFlip=false});
                    await multiplayer.SendData("start", new {});
                    break;
            }
            multiplayer.OnStart(StartGame);
        };

    }

    private void StartGame()
    {
       mainGame.SetActive(true);
       popup.SetActive(false);
       
       // Instantiate others
       for (int i = 0; i < multiplayer.GetNumberOfPlayer(); i++)
       {
           var other = Instantiate(otherPlayerTemplate, new Vector2(-2, 0), Quaternion.identity);
           otherPlayers.Add(other);
       }
       
       // Setup Callback
       multiplayer.OnChange((state, isFirst) =>
       {
           if (!isFirst)
           {
               int counter = 0;
               state.players.ForEach((sessionId, playerData) =>
               {
                   if (sessionId.Equals(multiplayer.GetCurrentSessionId())) return;
                   // otherPlayers[counter].transform.position = 
                   //     Vector2.Lerp(otherPlayers[counter].transform.position, 
                   //         new Vector2(playerData.x, playerData.y), 0.2f);
                   otherPlayers[counter].transform.position = 
                           new Vector2(playerData.x, playerData.y);
                   counter += 1;
                   // TODO: Use flip
               });
           }
       });
       
       isStart = true;


    }
    // Start is called before the first frame update
    void Start()
    {
        multiplayer = new Multiplayer();
        otherPlayers = new List<GameObject>();
        isStart = false;
        UIHandle();
    }

    // Update is called once per frame
    async void Update()
    {
        if (isStart)
        {
            await multiplayer.SendData("move", (Vector2)currentPlayer.transform.position);
        }
    }

    private void OnApplicationQuit()
    {
       multiplayer.Disconnect(); 
    }
}
