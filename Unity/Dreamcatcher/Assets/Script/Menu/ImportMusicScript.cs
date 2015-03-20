using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;

public class ImportMusicScript : MonoBehaviour {

    public GameObject DirectoryPanel;
    public RectTransform DirectoryPanelRecTransform;
    public GameObject FilePanel;
    public RectTransform FilePanelRecTransform;
    public GameObject FolderRowPrefab;
    public GameObject FileRowPrefab;

    private string _folderSource;

    private float _directoryPanelHeight;
    private float _filePanelHeight;

    private int _currentDirectoryPanelY;
    private int _currentFilePanelY;

    private GameObject[] _directoryRowInstance;
    private GameObject[] _fileRowInstance;

    void Start()
    {
        init();
    }

    public void init()
    {
        this._folderSource = Application.streamingAssetsPath;
        //
        this._directoryPanelHeight = DirectoryPanelRecTransform.sizeDelta.y;
        this._filePanelHeight = FilePanelRecTransform.sizeDelta.y;
        //
        this._currentDirectoryPanelY = 0;
        this._currentFilePanelY = 0;
        //
        loadAllRepository();
    }

    public void loadAllRepository()
    {
        string[] directory = Directory.GetDirectories(this._folderSource);
        //
        if (35 * directory.Length > DirectoryPanelRecTransform.sizeDelta.y)
        {
            DirectoryPanelRecTransform.sizeDelta += new Vector2(0, DirectoryPanelRecTransform.sizeDelta.y - (35 * directory.Length));
            DirectoryPanelRecTransform.position -= new Vector3(0, DirectoryPanelRecTransform.sizeDelta.y / 2, 0);
            //this._directoryPanelHeight = DirectoryPanelRecTransform.sizeDelta.y;
        }
        //
        string name = "";
        for (int i = 0; i < directory.Length; i++)
        {
            name = directory[i].Remove(0, this._folderSource.Length+1);
            createDirectoryEntry(name);
        }
    }

    private void createDirectoryEntry(string name)
    {
        _currentDirectoryPanelY -= 35;
        //
        Vector3 pos = new Vector3(210,_directoryPanelHeight + _currentDirectoryPanelY,DirectoryPanel.transform.position.z);
        //
        GameObject row = (GameObject)Instantiate(FolderRowPrefab);
        row.GetComponentInChildren<InputField>().text = name;
        row.transform.SetParent(DirectoryPanel.transform);
        row.transform.position = pos;
    }
}
