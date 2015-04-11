using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShootBulletScript : MonoBehaviour {

    BulletManager manager;
    //SpawnManager spawnManager;

	// Use this for initialization
    public void Shoot(float bulletDamageCoeff) 
    {
        // TODO get Nearest Enemy
        //List<Collider> units =  spawnManager.GetActiveUnits();
        List<Collider> units = new List<Collider>();
        float minDist = float.MaxValue;
        float dist = 0f;
        Vector3 destination = Vector3.zero;
        foreach(Collider c in units)
        {
            dist = (c.transform.position - transform.position).sqrMagnitude;
            if(dist < minDist)
            {
                minDist = dist;
                destination = c.transform.position;
            }
        }
        Vector3 direction = destination - transform.position;
        manager.SpawnBullet(transform.position, destination.normalized, bulletDamageCoeff);
	}
}
