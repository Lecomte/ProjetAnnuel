using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FileRowClass : MonoBehaviour {
    public Text text;
    public Button removeButton;

    public void visibleRowState(bool value)
    {
        this.text.gameObject.SetActive(value);
        this.removeButton.gameObject.SetActive(value);
    }
}
