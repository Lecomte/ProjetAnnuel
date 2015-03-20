using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FolderRowClass : MonoBehaviour {
    public Toggle toggle;
    public InputField inputField;
    public Button button;

    public void changeEnableFolder(bool isActive)
    {
        isActive = this.toggle.isOn;
        string name = this.inputField.text;
        Debug.Log(name + " is active : " + isActive);
    }

    public void OpenClick()
    {
        Debug.Log("open folder : " + this.inputField.text);
    }
}
