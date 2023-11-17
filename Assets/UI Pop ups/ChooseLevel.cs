using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]

public class ChooseLevel : MonoBehaviour
{
    private UIDocument _doc;
    public GameObject mainMenuObject;

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        _doc = GetComponent<UIDocument>();
        var root = _doc.rootVisualElement;
        var _exitBtn = root.Q<Button>("exitChooseLevelBtn");
        _exitBtn.clicked += () =>
        {
            Debug.Log("Exit button clicked");
            mainMenuObject.SetActive(true);
            this.gameObject.SetActive(false);
        };
    }
}
