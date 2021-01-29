using UnityEngine;
using UnityEngine.UI;

public class UserAccountLobby : MonoBehaviour
{
    public Text usernameText;

    void Start()
    {
        if(UserAccountManager.IsLoggedIn)
            usernameText.text = "Logged In As: " + UserAccountManager.playerUsername;
    }

    public void SignOut()
    {
        if(UserAccountManager.IsLoggedIn)
            UserAccountManager.instance.Logout();
    }
}
