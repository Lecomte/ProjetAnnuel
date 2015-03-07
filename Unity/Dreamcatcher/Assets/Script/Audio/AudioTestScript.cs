using UnityEngine;
using System.Collections;

public class AudioTestScript : MonoBehaviour {

    public AudioSource audio;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKeyDown(KeyCode.E))
            audio.Play();
	}
}
