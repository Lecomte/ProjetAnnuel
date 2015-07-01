using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;

public class YesNoPopupScript : MonoBehaviour {

    public Canvas myCanvas;
    public Text Text;

    private UnityEvent OnCloseEvent=new UnityEvent();

    public enum YesNoReturnValue { Yes = 0, No = 1 };

    public YesNoReturnValue ReturnValue;

    public void Instantiate(string text, UnityAction OnCloseCallback)
    {
        this.Text.text = text;
        this.myCanvas.gameObject.SetActive(true);
        this.OnCloseEvent.RemoveAllListeners();
        this.OnCloseEvent.AddListener(OnCloseCallback);
    }

    public void YesOnClick()
    {
        ReturnValue = YesNoReturnValue.Yes;
        this.Close();
    }

    public void NoOnClick()
    {
        ReturnValue = YesNoReturnValue.No;
        this.Close();
    }

    public void Close()
    {
        this.myCanvas.gameObject.SetActive(false);
        if (this.OnCloseEvent != null)
            this.OnCloseEvent.Invoke();
    }
}
