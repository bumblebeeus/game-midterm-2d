using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class RegisterMenu : MonoBehaviour
{
    public InputField username;
    public InputField password;

    public Button registerBtn;

    public Text systemNotif;

    public void backLoginMenu() {
        SceneManager.LoadScene(6);
    }

    public void register()
    {
        StartCoroutine(DataBase.Player.signUp(username.text, password.text, (success) => {
            if (success) {
                systemNotif.text = "Register successfully.\nClick 'Back' to return Login Menu.";
            } else {
                systemNotif.text = "Register failed.\nUsername have existed, please choose another.";
            }
        }));
    }

}
