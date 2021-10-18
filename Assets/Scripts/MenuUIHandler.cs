using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

// Sets the script to be executed later than all default scripts
// This is helpful for UI, since other things may need to be initialized before setting the UI
[DefaultExecutionOrder(1000)]

public class MenuUIHandler : MonoBehaviour
{
    public InputField txtInputField;
    
    public void StartGame()
    {
        Debug.Log("From the MENU scene (MenuUIHandler.cs): " + txtInputField.text);
        PersistencyManager.Instance.userName = txtInputField.text;
        Debug.Log("From the MENU scene (PersistencyManager.Instance.userName): " + PersistencyManager.Instance.userName);
        SceneManager.LoadScene(1);
    }
    public void ExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit(); // original code to quit Unity player
#endif
    }
}