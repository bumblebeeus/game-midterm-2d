using System.Collections;
using System.Collections.Generic;
using Server;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class Sample_Multiplayer : MonoBehaviour
{
    private Button b;

    private DropdownField d;

    private TextField t;

    private Label l;
    // Start is called before the first frame update
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
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
            t.value = evt.newValue.Equals("Server") ? $"{IpManager.GetIPv4()}:{IpManager.GetRandomPort()}" : "";
        });
    }

}
