using UnityEngine;
using System.Collections;

public class BulletUpdateScript : MonoBehaviour {

    [SerializeField]
    private UnitManager unitManager;

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
        if (direction != null)
        {
            transform.Translate(direction);
        }
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
