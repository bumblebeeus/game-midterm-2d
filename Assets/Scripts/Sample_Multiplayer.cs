using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class Sample_Multiplayer : MonoBehaviour
{
    public GameObject mainGame;
    public GameObject player;
    public GameObject otherPlayer;
    private Button b;

    private DropdownField d;

    private TextField t;

    private Label l;

    private uint counter;
    private bool isConnected;

    private ConnectionHandler connHandler;

    private GameState gs;

    private UIDocument ui;
    // Start is called before the first frame update
    void Start()
    {
        counter = 0;
        isConnected = false;
        gs = null;
        ui = GetComponent<UIDocument>();
        var root = ui.rootVisualElement;
        b = root.Q<Button>("Go");
        l = root.Q<Label>("Status");
        t = root.Q<TextField>("IP");
        d = root.Q<DropdownField>("Type");
        d.choices = new List<string>()
        {
            "Server",
            "Client"
        };
        d.RegisterValueChangedCallback(evt =>
        {
            if (evt.newValue.Equals("Server"))
            {
                t.value = $"{IpManager.GetIPv4()}:{IpManager.GetRandomPort()}";
                t.isReadOnly = true;
            }
            else
            {
                t.value = "";
                t.isReadOnly = false;
            }
        });
        
        b.clicked += () =>
        {
            var ip = t.text;
            var ipParts = ip.Split(':');
            if (ipParts.Length != 2)
            {
                l.text = "Status: Invalid IP";
                return;
            }

            try
            {
                var conn = new CommonSocket(ipParts[0], ipParts[1],
                    d.text.Equals("Server") ? SkType.Listen : SkType.Connect);
                connHandler = new ConnectionHandler(conn);
                connHandler.CreateConnection();
            }
            catch (Exception e)
            {
                l.text = $"Status: Exception ${e}";
                return;
            }
        };
    }

    void Update()
    {
        if (isConnected)
        {
            gs.SendCurrentData();
            gs.RecvOtherData();
            
            otherPlayer.transform.position = gs.GetOtherPosition();
            gs.SetCurrentPosition(transform.position);
            return;
        }
        // use counter to simulate tick's delay
        // polling ~2s
        counter += 1;
        if (counter >= 120)
        {
            counter = 0;
            if (connHandler != null && connHandler.CheckConnection())
            {
                l.text = $"Status: Connected";
                isConnected = true;
                
                // Enable rest
                mainGame.SetActive(true);
                // this.gameObject.SetActive(false);
                ui.enabled = false;
                
                var currPlayerInfo = new PlayerInfo
                {
                    Position = player.transform.position,
                    Name = "A",
                    SkinId = 0
                };
                var otherPlayerInfo = new PlayerInfo
                {
                    Position = Vector2.zero,
                    Name = "B",
                    SkinId = 0
                };

                gs = new GameState(connHandler, currPlayerInfo, otherPlayerInfo, false, false);
            }
            else
            {
                l.text = $"Status: Waiting";
                isConnected = false;
            }
        }
    }

}
