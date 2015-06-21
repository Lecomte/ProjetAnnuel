using UnityEngine;
using System.Collections;

public class MainMenuScript : MonoBehaviour {

    public string sceneName;

    public void loadScene()
    {
        Application.LoadLevel(sceneName);
    }
}
