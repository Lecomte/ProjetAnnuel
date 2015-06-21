using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FileRowClass : MonoBehaviour {
    public Text text;
    public Button removeButton;

    public StringEvent deleteClickEvent = new StringEvent();

    public void visibleRowState(bool value)
    {
        this.text.gameObject.SetActive(value);
        this.removeButton.gameObject.SetActive(value);
    }

    public void DeleteOnClick()
    {
        if (this.deleteClickEvent != null)
            this.deleteClickEvent.Invoke(text.text);
    }
}
