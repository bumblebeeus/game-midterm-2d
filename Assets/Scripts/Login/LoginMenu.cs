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

    public void backLoginMenu() {
        SceneManager.LoadScene(6);
    }

    public void goToRegister() {
        SceneManager.LoadScene(7);
    }

    private void goToMainMenu() {
        SceneManager.LoadScene(0);
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
