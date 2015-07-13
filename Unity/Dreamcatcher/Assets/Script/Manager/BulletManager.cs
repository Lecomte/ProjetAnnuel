using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BulletManager : MonoBehaviour
{

    [SerializeField]
    private List<BulletUpdateScript> bullets;
    private IEnumerator<BulletUpdateScript> enumBullets;

    public void Initialize()
    {
        enumBullets = bullets.GetEnumerator();
    }

    public void SpawnBullet(Vector3 position, Vector3 direction, float bulletDamageCoeff)
    {
        if(!enumBullets.MoveNext())
        {
            enumBullets.Reset();
            enumBullets.MoveNext();
        }
        enumBullets.Current.transform.position = position;
        enumBullets.Current.SetDirection(direction);
        enumBullets.Current.SetBulletDamage(bulletDamageCoeff);
        enumBullets.Current.gameObject.SetActive(true);
    }
}
