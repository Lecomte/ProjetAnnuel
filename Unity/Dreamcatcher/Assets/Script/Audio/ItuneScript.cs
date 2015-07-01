using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Threading;

public class ItuneScript : MonoBehaviour {

    public AudioSource source;
    private string streamingPath;

    private string configFolderName = @"\configSong.txt";
    private List<AudioClip> itunes;

    void Awake()
    {
        streamingPath = Application.streamingAssetsPath;
        itunes = new List<AudioClip>();
    }

    public void StartLoadSong()
    {
        Thread t = new Thread(loadAllEnableSound);
        t.Start();
    }

    public void loadAllEnableSound()
    {
        AudioClip clip;
        string[] enableFolder = System.IO.File.ReadAllText(streamingPath + configFolderName).Split(';');
        foreach(string folder in enableFolder)
        {
            string[] fileList = Directory.GetFiles(streamingPath + "/" + folder, "*.ogg");
            foreach(string fileName in fileList)
            {
                WWW dowloader = new WWW("file://" + fileName);
                while (!dowloader.isDone) { Thread.Sleep(1000); }
                clip = dowloader.GetAudioClip(false,false);
                if(clip.loadState == AudioDataLoadState.Loaded)
                    itunes.Add(clip);
            }
        }
    }

    void Update()
    {
        if (!source.isPlaying && itunes.Count > 0)
        {
            source.clip = GetNextSong();
            source.Play();
        }
    }

    public AudioClip GetNextSong()
    {
        return itunes[Random.Range(0, itunes.Count-1)];
    }


}
