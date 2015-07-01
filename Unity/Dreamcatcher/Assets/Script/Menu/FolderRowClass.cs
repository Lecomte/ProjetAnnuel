using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;

/**
 Warning Connu : 1.lorsque l'on reload après avoir supprimé un dossier peut fire l'event changeEnableFolder pour le remetre au valide ou invalide
 */

public class FolderRowClass : MonoBehaviour {
    public Toggle toggle;
    public InputField inputField;
    public Button OpenButton;
    public Button RemoveButton;

    public IntBoolEvent changeStateEvent = new IntBoolEvent();
    public UnityEvent<string> openClickEvent = new StringEvent();
    public UnityEvent<string> removeClickEvent = new StringEvent();

    public int Index;

    public void visibleRowState(bool value)
    {
        this.toggle.gameObject.SetActive(value);
        this.inputField.gameObject.SetActive(value);
        this.OpenButton.gameObject.SetActive(value);
        this.RemoveButton.gameObject.SetActive(value);
    }

    public void changeEnableFolder(bool isActive)
    {
        isActive = this.toggle.isOn;
        string name = this.inputField.text;
        if (changeStateEvent != null)
            changeStateEvent.Invoke(Index,isActive);
    }

    public void OpenClick()
    {
        if (openClickEvent != null)
            openClickEvent.Invoke(this.inputField.text);
    }

    public void RemoveClick()
    {
        if (removeClickEvent != null)
            removeClickEvent.Invoke(this.inputField.text);
    }
}
