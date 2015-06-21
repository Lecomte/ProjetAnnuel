using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Threading;
using System.Collections.Generic;
using System;

public class ImportMusicScript : MonoBehaviour {

    private Dictionary<int,FolderBufferClass> _allFolderList;

    public class FolderBufferClass
    {
        public string FolderName;
        public bool isActif;
    }

    public FolderRowClass[] FolderRowList;
    public FileRowClass[] FileRowList;

    [SerializeField]
    private Text indexPagesDirectoryText;

    [SerializeField]
    private Text indexPagesFileText;

    [SerializeField]
    private int _pageNumberDirectory=1;

    [SerializeField]
    private int _pageNumberFile = 1;

    [SerializeField]
    private GameObject currentPanel;

    [SerializeField]
    private GameObject previousPanel;

    private bool hasChanges = false;

    private string _folderSource;
    private string[] directory;
    private string[] filesNames;
    private string _currentRepoName;

    private string configFileName = @"\configSong.txt";
    private string[] supportedSoundExtension = new string[4] { "*.mp3", "*.mp4", "*.wma", "*.wav" };

    private string ffmegPath = "";

    void Start()
    {
        init();
    }

    public void init()
    {
        this._folderSource = UnityEngine.Application.streamingAssetsPath;
        this.ffmegPath = Application.streamingAssetsPath + "/ffmpeg.exe";
        //
        loadDirectory();
        changeIndexPagesDirectory();
        loadDisplayedRepository();
    }

    private void changeIndexPagesDirectory()
    {
        indexPagesDirectoryText.text = _pageNumberDirectory.ToString() + "/" + (Mathf.Ceil((float)directory.Length / (float)FolderRowList.Length)).ToString();
    }

    private void changeIndexPagesFile()
    {
        indexPagesFileText.text = _pageNumberFile.ToString() + "/" + (Mathf.Ceil((float)filesNames.Length / (float)FileRowList.Length)).ToString();
    }


    private void loadDirectory()
    {
        directory = Directory.GetDirectories(this._folderSource);
        this._allFolderList = new Dictionary<int,FolderBufferClass>();
        FolderBufferClass tmpRow;
        int i = 0;
        string[] enableFolder = System.IO.File.ReadAllText(this._folderSource + configFileName).Split(';');
        foreach (string folder in directory)
        {
            tmpRow = new FolderBufferClass();
            name = folder.Remove(0, this._folderSource.Length + 1);
            tmpRow.FolderName = name;
            tmpRow.isActif = ArrayContains(enableFolder,name);
            this._allFolderList.Add(i,tmpRow);
            i++;
        }
    }

    private bool ArrayContains(string[] array, string value)
    {
        for(int i=0; i < array.Length; i++)
        {
            if (array[i] == value)
                return true;
        }
        return false;
    }

    public void loadDisplayedRepository()
    {
        for (int i = 0; i < this.FolderRowList.Length; i++)
        {
            this.FolderRowList[i].inputField.text = "";
            this.FolderRowList[i].visibleRowState(false);
            this.FolderRowList[i].changeStateEvent.RemoveAllListeners();
            this.FolderRowList[i].openClickEvent.RemoveAllListeners();
        }
        //string name = "";
        int beginNumber = (_pageNumberDirectory - 1) * this.FolderRowList.Length;
        int endNumber;
        endNumber = _pageNumberDirectory * this.FolderRowList.Length < directory.Length ?
            _pageNumberDirectory * this.FolderRowList.Length :
            directory.Length;
        for (int i = beginNumber; i < endNumber; i++)
        {
            FolderBufferClass tmp = this._allFolderList[i];
            //name = directory[i].Remove(0, this._folderSource.Length+1);
            this.FolderRowList[i - beginNumber].Index = i;
            this.FolderRowList[i - beginNumber].inputField.text = tmp.FolderName;
            this.FolderRowList[i - beginNumber].toggle.isOn = tmp.isActif;
            this.FolderRowList[i - beginNumber].visibleRowState(true);
            this.FolderRowList[i - beginNumber].changeStateEvent.AddListener(changeState);
            this.FolderRowList[i - beginNumber].openClickEvent.AddListener(loadRepoContent);
        }
    }

    public void incrementPagesNumberDirectory()
    {
        if ((_pageNumberDirectory * this.FolderRowList.Length) > directory.Length)
            return;
        this._pageNumberDirectory++;
        changeIndexPagesDirectory();
        loadDisplayedRepository();
    }

    public void decrementPagesNumberDirectory()
    {
        if (_pageNumberDirectory > 1)
        {
            this._pageNumberDirectory--;
            changeIndexPagesDirectory();
            loadDisplayedRepository();
        }
    }

    public void incrementPagesNumberFile()
    {
        if ((_pageNumberFile * this.FileRowList.Length) > this.filesNames.Length)
            return;
        this._pageNumberFile++;
        changeIndexPagesFile();
        loadRepoContent(_currentRepoName);
    }

    public void decrementPagesNumberFile()
    {
        if (_pageNumberFile > 1)
        {
            this._pageNumberFile--;
            changeIndexPagesFile();
            loadRepoContent(_currentRepoName);
        }
    }

    public void loadRepoContent(string repoName)
    {
        this._currentRepoName = repoName;
        filesNames = Directory.GetFiles(this._folderSource + "\\" + repoName, "*.ogg");
        for (int i = 0; i < this.FileRowList.Length; i++)
        {
            this.FileRowList[i].text.text = "";
            this.FileRowList[i].visibleRowState(false);
            this.FileRowList[i].deleteClickEvent.RemoveAllListeners();
        }
        string name = "";
        int beginNumber = (_pageNumberFile - 1) * this.FileRowList.Length;
        int endNumber;
        endNumber = _pageNumberFile * this.FileRowList.Length < filesNames.Length ?
            _pageNumberFile * this.FileRowList.Length :
            filesNames.Length;
        for (int i = beginNumber; i < endNumber; i++)
        {
            name = filesNames[i].Remove(0, this._folderSource.Length + repoName.Length + 2);
            this.FileRowList[i - beginNumber].text.text = name;
            this.FileRowList[i - beginNumber].visibleRowState(true);
            this.FileRowList[i - beginNumber].deleteClickEvent.AddListener(deleteFile);
        }
    }

    public void ImportNewFile()
    {
        string selectedFolder = EditorUtility.OpenFolderPanel("Choose your music","","");
        if (selectedFolder == "") return;
        string destFolder = EditorUtility.OpenFolderPanel("Choose your music", this._folderSource, "");
        if (destFolder == "") return;
        List<string> importedFile = new List<string>();
        foreach(string ext in supportedSoundExtension)
        {
            string[] tmpFiles = Directory.GetFiles(selectedFolder,ext);
            foreach (string files in tmpFiles)
                importedFile.Add(files);
        }
        if (importedFile.Count > 0)
        {
            Thread t = new Thread(() => convertAudioFile(importedFile, selectedFolder, destFolder));
            t.Start();
            hasChanges = true;
        }
    }

    private void convertAudioFile(List<string> importFile, string refPath, string destFolder)
    {
        destFolder = destFolder.Replace("/", "\\");
        //
        string fileName = "";
        System.Diagnostics.Process process = new System.Diagnostics.Process();
        System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo(ffmegPath);
        //startInfo.UseShellExecute = false;
        startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
        int i = 0;
        foreach (string filePath in importFile)
        {
            string filePathClean = filePath.Replace("/", "\\");
            fileName = filePath.Replace(refPath, "");
            fileName = fileName.Substring(0, fileName.Length - 4);
            startInfo.Arguments = "-i \"" + filePathClean + "\" \"" + destFolder + fileName + ".ogg\" -n";
            process.StartInfo = startInfo;
            process.Start();
            while (!process.HasExited)
                Thread.Sleep(1000);
            i++;
            Debug.Log(i + "/" + importFile.Count + " imported");
        }
        process.Close();
        //this.Invoke("reloadFolder", 0f);
    }

    private void reloadFolder()
    {
        loadRepoContent(_currentRepoName);
    }

    public void changeState(int Index, bool isActif)
    {
        this._allFolderList[Index].isActif = isActif;
    }

    public void saveConfig()
    {
        string text = "";
        foreach(FolderBufferClass folder in this._allFolderList.Values)
        {
            if (folder.isActif)
               text += folder.FolderName + ";";
        }
        File.WriteAllText(this._folderSource + configFileName, text.Substring(0,text.Length-1));
        Debug.Log("Saved");
    }

    public void deleteFile(string fileName)
    {
        string path = this._folderSource + "/" + this._currentRepoName + "/" + fileName;
        Debug.Log("Delete :" + path);
        File.Delete(path);
        loadRepoContent(this._currentRepoName);
    }

    public void returnMenu()
    {
        if(hasChanges)
        {
            saveConfig();
        }
        this.currentPanel.SetActive(false);
        this.previousPanel.SetActive(true);
    }
}
