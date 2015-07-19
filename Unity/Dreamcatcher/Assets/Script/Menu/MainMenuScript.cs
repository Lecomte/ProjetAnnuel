using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour {

    public string sceneName;
    public Text message; 

    public void loadScene()
    {
        message.text = "";
        bool ContainsOneSong=false;
        string streamingPath = Application.streamingAssetsPath;
        string[] enableFolder = System.IO.File.ReadAllText(streamingPath + "/configSong.txt").Split(';');
        foreach (string folder in enableFolder)
        {
            if (folder == "")
                continue;
            string[] fileList = Directory.GetFiles(streamingPath + "/" + folder, "*.ogg");
            if(fileList.Length > 0)
            {
                ContainsOneSong = true;
                break;
            }
        }
        if(ContainsOneSong)
            Application.LoadLevel(sceneName);
        else
            message.text = "You must import song before start";
    }

    public void closeApplication()
    {
        Application.Quit();
    }
}
