using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using System.Runtime.CompilerServices;
//using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(UIDocument))]

public class MenuController : MonoBehaviour
{
    private UIDocument _doc;
    private VisualElement _mainMenuBtnWrapper;
    //private Button _playButton;
    //private Button _multiPlayerButton;
    //private Button _settingsButton;
    //private Button _exitButton;
    //private Button _muteButton;
    [Header("Mute Button")]
    [SerializeField] private Sprite _muteSprite;
    [SerializeField] private Sprite _unmuteSprite;
    private bool _isMuted = false;
    [SerializeField]
    private VisualTreeAsset _settingBtnMenu;
    private VisualElement _settingsBtns;

    public GameObject chooseLevelObject;

    public bool isDisabled = false;


    private void Start()
    {
        _doc = GetComponent<UIDocument>();
        var root = _doc.rootVisualElement;
        var _playButton = root.Q<Button>("PlayBtn");
        var _multiPlayerButton = root.Q<Button>("MultiPlayerBtn");
        var _exitButton = root.Q<Button>("ExitBtn");
        var _muteButton = root.Q<Button>("MuteBtn");
        var _settingsButton = root.Q<Button>("SettingsBtn");
        _mainMenuBtnWrapper = root.Q<VisualElement>("Buttons");
        

        _settingsBtns = _settingBtnMenu.CloneTree();
        var _backButton = _settingsBtns.Q<Button>("BackBtn");

        var _volumeSlider = _settingsBtns.Q<Slider>("VolumeSlider");
        var _toggle = _settingsBtns.Q<Toggle>("fullScreenToggle");
        _volumeSlider.highValue = 1.0f; // Set maximum slider value
        _volumeSlider.value = 0.5f;
        var currentVolume = 1.0f;

        _volumeSlider.RegisterValueChangedCallback((changeEvent) =>
        {
            var previousVolume = AudioListener.volume;
            AudioListener.volume = changeEvent.newValue;
            Debug.Log(changeEvent.newValue);
            currentVolume = AudioListener.volume;
            if (currentVolume == 0)
            {
                _isMuted = true;
                var bg = _muteButton.style.backgroundImage;
                bg.value = Background.FromSprite(_isMuted ? _muteSprite : _unmuteSprite);
                _muteButton.style.backgroundImage = bg;
            }
            if (previousVolume == 0 && currentVolume > 0)
            {
                _isMuted = false;
                var bg = _muteButton.style.backgroundImage;
                bg.value = Background.FromSprite(_isMuted ? _muteSprite : _unmuteSprite);
                _muteButton.style.backgroundImage = bg;
            }
        });

        _toggle.RegisterValueChangedCallback((changeEvent) =>
        {
            Debug.Log("Toggle changed");
            Screen.fullScreen = changeEvent.newValue;
        });

        // AudioListener.volume = 0.5f;

        _backButton.clicked += () =>
        {
            _mainMenuBtnWrapper.Clear();
            _mainMenuBtnWrapper.Add(_playButton);
            _mainMenuBtnWrapper.Add(_multiPlayerButton);
            _mainMenuBtnWrapper.Add(_settingsButton);
            _mainMenuBtnWrapper.Add(_exitButton);
        };

        _playButton.clicked += () =>
        {
            Debug.Log("Play button clicked");
            chooseLevelObject.SetActive(true);
            this.gameObject.SetActive(false);
        };
        //_multiPlayerButton.clicked += MultiPlayerButtonOnClicked;
        _settingsButton.clicked += () =>
        {
            _mainMenuBtnWrapper.Clear();
            _mainMenuBtnWrapper.Add(_settingsBtns);
        };

        _exitButton.clicked += () =>
        {
            Debug.Log("Exit button clicked");
            Application.Quit();
        };

        _muteButton.clicked += () =>
        {
            var previousVolume = AudioListener.volume;
            Debug.Log("Mute button clicked");
            _isMuted = !_isMuted;
            var bg = _muteButton.style.backgroundImage;
            bg.value = Background.FromSprite(_isMuted ? _muteSprite : _unmuteSprite);
            _muteButton.style.backgroundImage = bg;

            AudioListener.volume = _isMuted ? 0 : currentVolume;
            _volumeSlider.value = _isMuted ? 0 : currentVolume;

            if (previousVolume == 0)
            {
                _volumeSlider.value = 0.5f;
                AudioListener.volume = 0.5f;
            }


        };
    }

    // Start is called before the first frame update
    // void Start()
    // {
        
    // }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        if (!isDisabled)
        {
            return;
            
        }
        isDisabled = false;
        _doc = GetComponent<UIDocument>();
        var root = _doc.rootVisualElement;
        var _playButton = root.Q<Button>("PlayBtn");
        var _multiPlayerButton = root.Q<Button>("MultiPlayerBtn");
        var _exitButton = root.Q<Button>("ExitBtn");
        var _muteButton = root.Q<Button>("MuteBtn");
        var _settingsButton = root.Q<Button>("SettingsBtn");
        _mainMenuBtnWrapper = root.Q<VisualElement>("Buttons");


        _settingsBtns = _settingBtnMenu.CloneTree();
        var _backButton = _settingsBtns.Q<Button>("BackBtn");

        var _volumeSlider = _settingsBtns.Q<Slider>("VolumeSlider");
        var _toggle = _settingsBtns.Q<Toggle>("fullScreenToggle");
        _volumeSlider.highValue = 1.0f; // Set maximum slider value
        _volumeSlider.value = 0.5f;
        var currentVolume = 1.0f;

        _volumeSlider.RegisterValueChangedCallback((changeEvent) =>
        {
            var previousVolume = AudioListener.volume;
            AudioListener.volume = changeEvent.newValue;
            Debug.Log(changeEvent.newValue);
            currentVolume = AudioListener.volume;
            if (currentVolume == 0)
            {
                _isMuted = true;
                var bg = _muteButton.style.backgroundImage;
                bg.value = Background.FromSprite(_isMuted ? _muteSprite : _unmuteSprite);
                _muteButton.style.backgroundImage = bg;
            }
            if (previousVolume == 0 && currentVolume > 0)
            {
                _isMuted = false;
                var bg = _muteButton.style.backgroundImage;
                bg.value = Background.FromSprite(_isMuted ? _muteSprite : _unmuteSprite);
                _muteButton.style.backgroundImage = bg;
            }
        });

        _toggle.RegisterValueChangedCallback((changeEvent) =>
        {
            Debug.Log("Toggle changed");
            Screen.fullScreen = changeEvent.newValue;
        });


        // AudioListener.volume = 0.5f;

        _backButton.clicked += () =>
        {
            _mainMenuBtnWrapper.Clear();
            _mainMenuBtnWrapper.Add(_playButton);
            _mainMenuBtnWrapper.Add(_multiPlayerButton);
            _mainMenuBtnWrapper.Add(_settingsButton);
            _mainMenuBtnWrapper.Add(_exitButton);
        };

        _playButton.clicked += () =>
        {
            Debug.Log("Play button clicked");
            chooseLevelObject.SetActive(true);
            this.gameObject.SetActive(false);
        };
        //_multiPlayerButton.clicked += MultiPlayerButtonOnClicked;
        _settingsButton.clicked += () =>
        {
            _mainMenuBtnWrapper.Clear();
            _mainMenuBtnWrapper.Add(_settingsBtns);
        };

        _exitButton.clicked += () =>
        {
            Debug.Log("Exit button clicked");
            Application.Quit();
        };

        _muteButton.clicked += () =>
        {
            var previousVolume = AudioListener.volume;
            Debug.Log("Mute button clicked");
            _isMuted = !_isMuted;
            var bg = _muteButton.style.backgroundImage;
            bg.value = Background.FromSprite(_isMuted ? _muteSprite : _unmuteSprite);
            _muteButton.style.backgroundImage = bg;

            AudioListener.volume = _isMuted ? 0 : currentVolume;
            _volumeSlider.value = _isMuted ? 0 : currentVolume;

            if (previousVolume == 0)
            {
                _volumeSlider.value = 0.5f;
                AudioListener.volume = 0.5f;
            }


        };
    }

    private void OnDisable()
    {
        isDisabled = true;
    }
}
