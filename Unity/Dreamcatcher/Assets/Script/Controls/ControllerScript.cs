using UnityEngine;
using System.Collections;

public class ControllerScript : MonoBehaviour {

    [SerializeField]
    private float playerSpeed = 10f;
    [SerializeField]
    private Animator animmator;

    private enum RunState
    {
        WALKING, 
        RUNNING,
        STANDING
    };

    private RunState state;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        Vector3 direction = new Vector3(Input.GetAxis("L_XAxis_1"), 0, -Input.GetAxis("L_YAxis_1"));
        float magnitude = direction.magnitude;
        if (magnitude > 1.0f)
        { 
            direction.Normalize();
        }
		
        if (magnitude > 0.5f)
        {
            if (state != RunState.RUNNING)
            {
                state = RunState.RUNNING;
                animmator.SetBool("isRunning", true);
                animmator.SetBool("isWalking", false);
            }
        }
        else if (magnitude > 0f)
        {
            if (state != RunState.WALKING)
            {
                state = RunState.WALKING;
                animmator.SetBool("isRunning", false);
                animmator.SetBool("isWalking", true);
            }
        }
        else
        {
            if (state != RunState.STANDING)
            {
                state = RunState.STANDING;
                animmator.SetBool("isRunning", false);
                animmator.SetBool("isWalking", false);
            }
        }
        transform.Translate(direction * Time.deltaTime * playerSpeed);
    }


}
