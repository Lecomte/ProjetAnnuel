using UnityEngine;
using UnityEngine.Events;

public class OnHitPlayerScript : MonoBehaviour {

    [SerializeField]
    private byte _layer;

    [SerializeField]
    private UnitManager unitManager;

    [SerializeField]
    private DamageEvent eventToFire;



    void Start()
    {
        if (eventToFire == null)
            Debug.Log("Aucun event renseigné.");
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == _layer)
            if (eventToFire != null)
                eventToFire.Invoke(unitManager.getEntityStatisticScript(collision.collider).Damage);
    }
}
