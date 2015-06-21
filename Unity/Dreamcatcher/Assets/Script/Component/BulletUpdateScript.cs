using UnityEngine;
using System.Collections;

public class BulletUpdateScript : MonoBehaviour {

    [SerializeField]
    private UnitManager unitManager;
    [SerializeField]
    private float bulletSpeed = 100f;

    private Vector3 startPos;
    private Vector3 direction;
    private float bulletDamageCoeff;
	
    void Start()
    {
        startPos = transform.position;
    }

	// Update is called once per frame
    void Update()
    {
        transform.Translate(direction * Time.deltaTime * bulletSpeed);
	}

    void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.gameObject.layer == 6)
        {
            unitManager.infligeDamageToCollider(collision.collider, (int)(10 * bulletDamageCoeff));
        }
        transform.position = startPos;
        this.gameObject.SetActive(false);
    }

    public void SetDirection(Vector3 d)
    {
        direction = d;
    }
    public void SetBulletDamage(float d)
    {
        bulletDamageCoeff = d;
    }
}
