using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class ItuneScript : MonoBehaviour {

    public AudioSource source;
    public bool forceChange;


    private string configFolderName = @"\configSong.txt";
    private List<AudioClip> itunes;

    void Awake()
    {
        itunes = new List<AudioClip>();
    }

    public void loadAllEnableSound()
    {
        string[] enableFolder = System.IO.File.ReadAllText(Application.streamingAssetsPath + configFolderName).Split(';');
        foreach(string folder in enableFolder)
        {
            string[] fileList = Directory.GetFiles(Application.streamingAssetsPath + "/" + folder,"*.ogg");
            foreach(string fileName in fileList)
            {
                WWW dowloader = new WWW(Application.streamingAssetsPath + "/" + folder + "/" + fileName);
                itunes.Add(dowloader.GetAudioClip(false,false,AudioType.OGGVORBIS));
            }
        }
        Debug.Log("All download : " + itunes.Count);
    }

    void FixedUpdate()
    {
        if (forceChange || (source.clip.loadState == AudioDataLoadState.Loaded && !source.isPlaying && itunes.Count > 0))
        {
            source.clip = GetNextSong();
            source.Play();
            forceChange = false;
            Debug.Log("play new sound");
        }
    }

    public AudioClip GetNextSong()
    {
        return itunes[Random.Range(0, itunes.Count-1)];
    }


}
