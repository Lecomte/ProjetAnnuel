using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShootBulletScript : MonoBehaviour {

    [SerializeField]
    private BulletManager manager;
    [SerializeField]
    private SpawnManager spawnManager;

	// Use this for initialization
    public void Shoot(float bulletDamageCoeff) 
    {
        bool found = false;
        List<Collider> units =  spawnManager.GetActiveUnits();
        float minDist = float.MaxValue;
        float dist = 0f;
        float angle = 0f;
        Vector3 destination = Vector3.zero;
        Vector3 direction = Vector3.zero;
        Vector3 forward = transform.forward;
        foreach(Collider c in units)
        {
            direction = c.transform.position - transform.position;
            angle = Vector3.Angle(forward, direction);
            if (angle > -75f && angle < 75f)
            {
                dist = (c.transform.position - transform.position).sqrMagnitude;
                if (dist < minDist)
                {
                    found = true;
                    minDist = dist;
                    destination = c.transform.position;
                }
            }
        }
        if (found)
        {
            direction = destination - transform.position;
            manager.SpawnBullet(transform.position, direction.normalized, bulletDamageCoeff);
        }
	}
}
