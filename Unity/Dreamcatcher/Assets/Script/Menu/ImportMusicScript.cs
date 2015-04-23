using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;

public class ImportMusicScript : MonoBehaviour {

    public FolderRowClass[] FolderRowList;
    public FileRowClass[] FileRowList;

    [SerializeField]
    private Text indexPagesText;

    [SerializeField]
    private int _pageNumber=1;

    private string _folderSource;
    private string[] directory;

    void Start()
    {
        init();
    }

    public void init()
    {
        this._folderSource = Application.streamingAssetsPath;
        //
        loadDirectory();
        changeIndexPages();
        loadDisplayedRepository();
    }

    private void changeIndexPages()
    {
        indexPagesText.text = _pageNumber.ToString() + "/" + (Mathf.Ceil((float)directory.Length / (float)FolderRowList.Length)).ToString();
    }

    private void loadDirectory()
    {
        directory = Directory.GetDirectories(this._folderSource);
    }

    public void loadDisplayedRepository()
    {
        for (int i = 0; i < this.FolderRowList.Length; i++)
        {
            this.FolderRowList[i].inputField.text = "";
            this.FolderRowList[i].visibleRowState(false);
        }
        string name = "";
        int beginNumber = (_pageNumber - 1) * this.FolderRowList.Length;
        int endNumber;
        endNumber = _pageNumber * this.FolderRowList.Length < directory.Length ? 
            _pageNumber * this.FolderRowList.Length :
            directory.Length;
        for (int i = beginNumber; i < endNumber; i++)
        {
            name = directory[i].Remove(0, this._folderSource.Length+1);
            this.FolderRowList[i - beginNumber].inputField.text = name;
            this.FolderRowList[i - beginNumber].visibleRowState(true);
        }
    }

    public void incrementPagesNumber()
    {
        if ((_pageNumber * this.FolderRowList.Length) > directory.Length)
            return;
        this._pageNumber++;
        changeIndexPages();
        loadDisplayedRepository();
    }

    public void decrementPagesNumber()
    {
        if (_pageNumber > 1)
        {
            this._pageNumber--;
            changeIndexPages();
            loadDisplayedRepository();
        }
    }

    public void loadRepoContent(string repoName)
    {
        string[] filesNames = Directory.GetFiles(this._folderSource + "\\" + repoName);
        for (int i = 0; i < this.filesNames.Length; i++)
        {
            this.filesNames[i].inputField.text = "";
            this.filesNames[i].visibleRowState(false);
        }
        string name = "";
        int beginNumber = (_pageNumber - 1) * this.FolderRowList.Length;
        int endNumber;
        endNumber = _pageNumber * this.FolderRowList.Length < directory.Length ?
            _pageNumber * this.FolderRowList.Length :
            directory.Length;
        for (int i = beginNumber; i < endNumber; i++)
        {
            name = directory[i].Remove(0, this._folderSource.Length + 1);
            this.FolderRowList[i - beginNumber].inputField.text = name;
            this.FolderRowList[i - beginNumber].visibleRowState(true);
        }
    }
}
