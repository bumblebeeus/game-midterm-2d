using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginMenu : MonoBehaviour
{

    public InputField username;
    public InputField password;

    public Button LoginBtn;

    public Text systemNotif;

    public void goToRegister() {
        SceneManager.LoadScene("RegisterMenu");
    }

    public void goToMainMenu() {
        SceneManager.LoadScene("Menu");
    }

    public void login()
    {
        DataBase.Player currentPlayer = DataBase.Player.getCurrentPlayer();
        StartCoroutine(currentPlayer.login(username.text, password.text, (success) => {
            if (success) {
                goToMainMenu();
            } else {
                systemNotif.text = "Wrong username or password. Try again.";
            }
        }));
    }
}
