using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AutocamScript : MonoBehaviour {

    private enum CameraMode
    {
        MAN,
        NEAR,
        CHOSEN
    };

    [SerializeField]
	private UnitManager unitManager;
    [SerializeField]
    private Transform camera;
    [SerializeField]
    private float autoCameraSpeed = 1.0f;
    [SerializeField]
    private float manualCameraSpeed = 100f;

    private Quaternion cameraBaseRotation;

    private CameraMode mode = CameraMode.CHOSEN;
    private Collider destCollider;

    private List<Collider> units;
    private bool changed = false;
	
    public void Initialize()
    {
        cameraBaseRotation = camera.localRotation;
    }

	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("RS_1"))
        {
                switch(mode)
                {
                    /*case CameraMode.NEAR:
                        mode = CameraMode.CHOSEN;
                        break;*/
                    case CameraMode.CHOSEN:
                        camera.localRotation = cameraBaseRotation;
                        mode = CameraMode.MAN;
                        break;
                    case CameraMode.MAN:
                        camera.localRotation = cameraBaseRotation;
                        getNearestUnitAsDest();
                        mode = CameraMode.CHOSEN;
                        break;
                }
        }
        switch (mode)
        {
            case CameraMode.NEAR:
                getNearestUnitAsDest();
                moveCam();
                break;
            case CameraMode.CHOSEN:
                semiManualCam();
                moveCam();
                break;
            case CameraMode.MAN:
                manualCam();
                break;
        }
	}

    private void moveCam()
    {
        if (destCollider)
        {
			Vector3 dest = new Vector3(destCollider.transform.position.x, transform.position.y ,destCollider.transform.position.z)
            Quaternion targetRotationSocket = Quaternion.LookRotation(dest - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotationSocket, Time.deltaTime * autoCameraSpeed);
        }
    }

    public void getNearestUnitAsDest()
    {
        units = unitManager.GetActiveUnits();
        float minDist = float.MaxValue;
        float dist = 0f;
        float angle = 0f;
        Collider destination = null;
        Vector3 direction = Vector3.zero;
        Vector3 forward = transform.forward;
        foreach (Collider c in units)
        {
            direction = c.transform.position - transform.position;
            angle = Vector3.Angle(forward, direction);
            if (angle > -75f && angle < 75f)
            {
                dist = (c.transform.position - transform.position).sqrMagnitude;
                if (dist < minDist)
                {
                    minDist = dist;
                    destination = c;
                }
            }
        }
        destCollider = destination;
    }

    private void getNextEntity(bool right)
    {
		units = unitManager.GetActiveUnits();
        float minAngle = float.MaxValue;
        float angleCurrentTarget = 0f;
        Collider destination = null;
        Vector3 direction = Vector3.zero;
        Vector3 currentDirection = transform.forward;
        foreach (Collider c in units)
        {
            direction = c.transform.position - transform.position;
            angleCurrentTarget = Vector3.Angle(currentDirection, direction);
            if ((right && angleCurrentTarget > 0f && angleCurrentTarget < minAngle) || (!right && angleCurrentTarget < 0f && angleCurrentTarget > minAngle))
            {
                minAngle = angleCurrentTarget;
                destination = c;
            }
        }
        destCollider = destination;
    }

    public void onUnitsListChange()
    {
        if(mode != CameraMode.MAN)
        { 
		    units = unitManager.GetActiveUnits();
            if(!units.Contains(destCollider))
            {
                getNearestUnitAsDest();
            }
        }
    }

    private void manualCam()
    {
        transform.Rotate(Vector3.up, Input.GetAxis("R_XAxis_1") * Time.deltaTime * manualCameraSpeed);
        camera.Rotate(Vector3.right, Input.GetAxis("R_YAxis_1") * Time.deltaTime * manualCameraSpeed);
    }

    private void semiManualCam()
    {
        if(!changed && Input.GetAxis("R_XAxis_1") > 0.5f)
        {
            changed = true;
            getNextEntity(true);
        }
        else if (!changed &&  Input.GetAxis("R_XAxis_1") < -0.5)
        {
            changed = true;
            getNextEntity(false);
        }
        else
        {
            changed = false;
        }
    }

    public Collider getDest()
    {
        return destCollider;
    }

    public bool isCamMan()
    {
        return mode == CameraMode.MAN;
    }
}
