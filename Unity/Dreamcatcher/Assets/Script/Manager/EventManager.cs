using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class EventManager : MonoBehaviour {

    public UnityEvent GameStartEvent;
    public UnityEvent GameEndEvent;
    public UnityEvent RoundStartEvent;
    public UnityEvent RoundEndEvent;
    public UnityEvent StopGameEvent;
    
    public void fireGameStartEvent()
    {
        if (this.GameStartEvent != null)
            GameStartEvent.Invoke();
    }

    public void fireRoundStartEvent()
    {
        if (this.RoundStartEvent != null)
            RoundStartEvent.Invoke();
    }

    public void fireRoundEndEvent()
    {
        if (this.RoundEndEvent != null)
            this.RoundEndEvent.Invoke();
    }

    public void fireGameEndEvent()
    {
        if (this.GameEndEvent != null)
            this.GameEndEvent.Invoke();
    }

    public void fireStopGameEvent()
    {
        if (this.StopGameEvent != null)
            this.StopGameEvent.Invoke();
    }
	

}
