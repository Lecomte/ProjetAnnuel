using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {
    [SerializeField]
    private Transform camera;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, -Input.GetAxis("R_XAxis_1") * Time.deltaTime * 100);
        camera.Rotate(Vector3.left, -Input.GetAxis("R_YAxis_1") * Time.deltaTime * 100);
	}
}
