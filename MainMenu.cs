using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // This function will quit the application.
    public void ApplicationQuit()
    {
        // If the game is running in the editor, this line won't work.
        // You need to stop play mode manually.
        Application.Quit();

#if UNITY_EDITOR
        // For testing in the Unity Editor, stop the play mode.
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    // This function will change the scene based on the scene name provided.
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}

