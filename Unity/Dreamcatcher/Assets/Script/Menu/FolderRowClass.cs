using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;

public class FolderRowClass : MonoBehaviour {
    public Toggle toggle;
    public InputField inputField;
    public Button button;

    public UnityEvent<string> openClickEvent;

    public void visibleRowState(bool value)
    {
        this.toggle.gameObject.SetActive(value);
        this.inputField.gameObject.SetActive(value);
        this.button.gameObject.SetActive(value);
    }

    public void changeEnableFolder(bool isActive)
    {
        isActive = this.toggle.isOn;
        string name = this.inputField.text;
        Debug.Log(name + " is active : " + isActive);
    }



    public void OpenClick()
    {
        if (openClickEvent != null)
            openClickEvent.Invoke(this.inputField.text);
        Debug.Log("open folder : " + this.inputField.text);
    }
}
