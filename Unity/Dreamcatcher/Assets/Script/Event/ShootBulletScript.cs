using UnityEngine;
using System.Collections;

public class ShootBulletScript : MonoBehaviour {

    BulletManager manager;

	// Use this for initialization
    public void Shoot(float bulletDamageCoeff) 
    {
        // TODO get Nearest Enemy
        //spawn manager.GetActiveUnits
        Vector3 destination = new Vector3(0,0,0);
        // TODO get Nearest Enemy
        Vector3 direction = destination - transform.position;
        manager.SpawnBullet(transform.position, destination.normalized, bulletDamageCoeff);
	}
}
