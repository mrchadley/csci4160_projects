using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }

    public SceneSwitcher sw;
    public void NewGame()
    {
        sw.SwitchScene();
    }
    public void LoadGame()
    {
        //set scene name, do whatever info loading that's necessary
        sw.sceneName = "MainMenu";
        sw.SwitchScene();
    }
    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}
