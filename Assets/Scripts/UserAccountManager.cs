using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DatabaseControl;
using UnityEngine.SceneManagement;

public class UserAccountManager : MonoBehaviour
{
    public static UserAccountManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(this);
        
#if UNITY_EDITOR
        Debug.unityLogger.logEnabled = true;
#else
  Debug.unityLogger.logEnabled = false;
#endif
        
        Debug.Log("Game Started!");

    }

    public static string playerUsername { get; protected set; }
    private static string playerPassword = "";
    public static string playerData { get; protected set; }
    public static bool IsLoggedIn { get; protected set; }

    public string OnLogInSceneName = "MatchSearchScene";
    public string OnLogOutSceneName = "UACscene";

    public void Logout()
    {
        playerUsername = "";
        playerPassword = "";

        IsLoggedIn = false;
        Debug.Log("User logged out!");

        SceneManager.LoadScene(OnLogOutSceneName);
    }

    public void Login(string username, string password)
    {
        playerUsername = username;
        playerPassword = password;
        IsLoggedIn = true;
        Debug.Log("User logged in!");

        SceneManager.LoadScene(OnLogInSceneName);
    }

    public void GetUserData()
    {
        if(IsLoggedIn)
            StartCoroutine(GetData());
    }
    IEnumerator GetData()
    {
        IEnumerator e = DCF.GetUserData(playerUsername, playerPassword); // << Send request to get the player's data string. Provides the username and password
        while (e.MoveNext())
        {
            yield return e.Current;
        }
        string response = e.Current as string; // << The returned string from the request

        if (response == "Error")
        {
            //There was another error. Automatically logs player out. This error message should never appear, but is here just in case.
            //ResetAllUIElements();
            //playerUsername = "";
            //playerPassword = "";
            //loginParent.gameObject.SetActive(true);
            //loadingParent.gameObject.SetActive(false);
            //Login_ErrorText.text = "Error: Unknown Error. Please try again later.";
            Logout();
        }
        else
        {
            //The player's data was retrieved. Goes back to loggedIn UI and displays the retrieved data in the InputField
            //loadingParent.gameObject.SetActive(false);
            //loggedInParent.gameObject.SetActive(true);
            //LoggedIn_DataOutputField.text = response;
            playerData = response;
        }
    }

    public void SetUserData(string data)
    {
        if (IsLoggedIn)
            StartCoroutine(SetData(data));
    }
    IEnumerator SetData(string data)
    {
        IEnumerator e = DCF.SetUserData(playerUsername, playerPassword, data); // << Send request to set the player's data string. Provides the username, password and new data string
        while (e.MoveNext())
        {
            yield return e.Current;
        }
        string response = e.Current as string; // << The returned string from the request

        if (response == "Success")
        {
            //The data string was set correctly. Goes back to LoggedIn UI
            //loadingParent.gameObject.SetActive(false);
            //loggedInParent.gameObject.SetActive(true);
        }
        else
        {
            //There was another error. Automatically logs player out. This error message should never appear, but is here just in case.
            //ResetAllUIElements();
            //playerUsername = "";
            //playerPassword = "";
            //loginParent.gameObject.SetActive(true);
            //loadingParent.gameObject.SetActive(false);
            //Login_ErrorText.text = "Error: Unknown Error. Please try again later.";
            Logout();
        }
    }
}
