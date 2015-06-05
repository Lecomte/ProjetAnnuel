using UnityEngine;
using System.Collections;

public class GrapnelScript : MonoBehaviour {

    private Collider target;
    private bool movePlayer;
    [SerializeField]
    private AutocamScript camScript;

	// Use this for initialization
	void Start () {
        movePlayer = false;
        	
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        if (Input.GetButtonDown("LB_1"))
        {
            if (camScript.isCamMan())
                camScript.getNearestUnitAsDest();
            target = camScript.getDest();
            movePlayer = true;
        }
        if (Input.GetButtonDown("RB_1"))
        {
            if (camScript.isCamMan())
                camScript.getNearestUnitAsDest();
            target = camScript.getDest();
            movePlayer = false;
        }
        if(target != null)
        {
            Vector3 dir = target.transform.position - transform.position;
            float dirSpeed = dir.sqrMagnitude;
            if (dirSpeed > 1.0f)
            {
                if(movePlayer)
                {
                    transform.Translate((dir.normalized) * (Time.deltaTime * 10), Space.World);
                }
                else
                {
                    target.transform.Translate(-(dir.normalized) * (Time.deltaTime * 10), Space.World);
                }
            }
            else
            {
                target = null;
            }
        }

	}
}
