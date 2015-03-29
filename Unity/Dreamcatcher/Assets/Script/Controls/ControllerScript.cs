using UnityEngine;
using System.Collections;

public class ControllerScript : MonoBehaviour {
    
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 direction = new Vector3(Input.GetAxis("L_XAxis_1"), 0, -Input.GetAxis("L_YAxis_1"));
        if (direction.magnitude > 1.0f)
        {
            direction.Normalize();
        }
        transform.Translate(direction * Time.deltaTime * 100);
    }


}
