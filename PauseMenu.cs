using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public BuildingSystem buildingSystem;
    public GameObject MainUI;
    public PublicBoolForPauseMenuOpen publicBoolForPauseMenuOpen;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(!buildingSystem.isBuildingEnabled && !publicBoolForPauseMenuOpen.Check())
            {
                if(MainUI.activeSelf == false)
                {
                    MainUI.SetActive(true);
                }
                else
                {
                    MainUI.SetActive(false);
                }
            }
        }
    }

    public void Return()
    {
        MainUI.SetActive(false);
    }

    public void QuitToMenu(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitToDesktop()
    {
        Application.Quit();

#if UNITY_EDITOR
        // For testing in the Unity Editor, stop the play mode.
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
