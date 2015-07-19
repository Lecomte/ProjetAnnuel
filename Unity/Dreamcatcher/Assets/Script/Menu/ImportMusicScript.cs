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

using System.Runtime.InteropServices;

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

    [SerializeField]
    private YesNoPopupScript popupHandler;

    [SerializeField]
    private InputField createFolderNameInput;

    [DllImport("user32.dll")]
    private static extern void FolderBrowserDialog();

    private bool hasChanges = false;

    private string _folderSource;
    private string[] directory;
    private string[] filesNames;
    private string _currentRepoName;

    private string configFileName = @"\configSong.txt";
    private string[] supportedSoundExtension = new string[5] { "*.mp3", "*.mp4", "*.wma", "*.wav", "*.m4a" };

    private string ffmegPath = "";

    private bool OneLastEnter;
    private bool isImporting;
    private int NumberImported;
    private int ImportLength;

    public Text LabelImport;

    public void init()
    {
        hasChanges = false;
        this._folderSource = UnityEngine.Application.streamingAssetsPath;
        this.ffmegPath = Application.streamingAssetsPath + "/ffmpeg.exe";
        //
        loadDirectory();
        changeIndexPagesDirectory();
        loadDisplayedRepository();
    }

    void Update()
    {
        if (isImporting || OneLastEnter)
        {
            this.LabelImport.text = "Imported : " + NumberImported + " / " + ImportLength;
            if (!isImporting)
            {
                reloadFolder();
                OneLastEnter = false;
            }
        }
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
            this.FolderRowList[i].removeClickEvent.RemoveAllListeners();
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
            this.FolderRowList[i - beginNumber].removeClickEvent.AddListener(removeRepo);
        }
    }

    public void incrementPagesNumberDirectory()
    {
        if ((_pageNumberDirectory * this.FolderRowList.Length) >= directory.Length)
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
        if (this.filesNames == null) return;
        if ((_pageNumberFile * this.FileRowList.Length) >= this.filesNames.Length)
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

    private string removeSavePath;
    private string repoSaveName;
    public void removeRepo(string repoName)
    {
        repoSaveName = repoName;
        removeSavePath = this._folderSource + "/" + repoName;
        popupHandler.Instantiate("Are you sure to want to delete this repo ?", removeRepoReturnValue);
    }

    private void removeRepoReturnValue()
    {
        if (popupHandler.ReturnValue == YesNoPopupScript.YesNoReturnValue.No) { return; }
        string[] fileList = Directory.GetFiles(removeSavePath);
        foreach (string file in fileList)
        {
            File.Delete(file);
            Debug.Log("Delete File : " + file);
        }
        Directory.Delete(removeSavePath);
        Debug.Log("Delete Folder : " + removeSavePath);

        string newContent="";
        string[] contentConfig = File.ReadAllLines(this._folderSource + configFileName);
        foreach(string folder in contentConfig[0].Split(';'))
        {
            if (folder != repoSaveName)
                newContent += folder + ";";
        }
        File.WriteAllText(this._folderSource + configFileName, newContent);
        init();
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
        changeIndexPagesFile();
    }

    public void ImportNewFile()
    {
        string selectedFolder="";
        string destFolder="";
        //
        System.Windows.Forms.FolderBrowserDialog sfd = new System.Windows.Forms.FolderBrowserDialog();
        sfd.RootFolder = Environment.SpecialFolder.MyMusic;
        if (sfd.ShowDialog() != System.Windows.Forms.DialogResult.OK)
            return;
        selectedFolder = sfd.SelectedPath;
        //
        sfd.RootFolder = Environment.SpecialFolder.MyComputer;
        sfd.SelectedPath = this._folderSource + "/";
        if (sfd.ShowDialog() != System.Windows.Forms.DialogResult.OK)
            return;
        destFolder = sfd.SelectedPath;
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
        }else{
            this.LabelImport.text = "No File Found";
        }
    }

    private void convertAudioFile(List<string> importFile, string refPath, string destFolder)
    {
        ImportLength = importFile.Count;
        isImporting = true;
        destFolder = destFolder.Replace("/", "\\");
        //
        string fileName = "";
        System.Diagnostics.Process process = new System.Diagnostics.Process();
        System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo(ffmegPath);
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
            NumberImported = i;
        }
        process.Close();
        OneLastEnter = true;
        isImporting = false;
    }

    private void reloadFolder()
    {
        loadRepoContent(_currentRepoName);
    }

    public void changeState(int Index, bool isActif)
    {
        hasChanges = true;
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
        hasChanges = false;
    }

    public void deleteFile(string fileName)
    {
        string path = this._folderSource + "/" + this._currentRepoName + "/" + fileName;
        File.Delete(path);
        loadRepoContent(this._currentRepoName);
    }

    public void returnMenu()
    {
        if (hasChanges)
        {
            popupHandler.Instantiate("Unsaved Changes has been detected, do you want to save ?", checkChangeNotSaved);
            return;
        }
        checkChangeNotSaved();
    }

    private void checkChangeNotSaved()
    {
        if (hasChanges && this.popupHandler.ReturnValue == YesNoPopupScript.YesNoReturnValue.Yes)
        {
            saveConfig();
        }
        this.currentPanel.SetActive(false);
        this.previousPanel.SetActive(true);
    }

    public void CreateNewFolder()
    {
        string folderName = this.createFolderNameInput.text;
        if (folderName != "")//TODO Check invalide character // Path.GetInvalidFileNameChars
        {
            Directory.CreateDirectory(this._folderSource + "/" + folderName);
            init();
        }
    }
}
