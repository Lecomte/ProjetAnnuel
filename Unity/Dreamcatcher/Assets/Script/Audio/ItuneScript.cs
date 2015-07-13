using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using UnityEngine.UI;
using UnityEngine.Events;

public class ItuneScript : MonoBehaviour {

    public AudioSource source;
    private string streamingPath;

    private string configFolderName = @"\configSong.txt";
    private List<AudioClip> itunes;

    private bool MusicStart = false;

    public Text MessageImportation;
    public UnityEvent OnFinishImport;

    //WWW ne peut être call que du même thread
    public void StartLoadSong()
    {
        StartCoroutine(loadAllEnableSound());
    }

    public IEnumerator loadAllEnableSound()
    {
        MessageImportation.text = "Start Importation";
        streamingPath = Application.streamingAssetsPath;
        itunes = new List<AudioClip>();
        AudioClip clip;
        int numberImported=1;
        string[] enableFolder = System.IO.File.ReadAllText(streamingPath + configFolderName).Split(';');
        foreach(string folder in enableFolder)
        {
            string[] fileList = Directory.GetFiles(streamingPath + "/" + folder, "*.ogg");
            foreach(string fileName in fileList)
            {
                MessageImportation.text = "Folder : " + folder + " import music " + numberImported + "/" + fileList.Length;
                WWW dowloader = new WWW("file://" + fileName);
                while (!dowloader.isDone) { Thread.Sleep(1000); }
                clip = dowloader.GetAudioClip(false,false);
                if(clip.loadState == AudioDataLoadState.Loaded)
                    itunes.Add(clip);
                numberImported++;
                yield return (0);
            }
        }
        OnFinishImport.Invoke();
    }

    public void StartMusic()
    {
        MusicStart = true;
    }

    public void StopMusic()
    {
        MusicStart = false;
        source.Stop();
    }

    void Update()
    {
        if (MusicStart && !source.isPlaying && itunes.Count > 0)
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
