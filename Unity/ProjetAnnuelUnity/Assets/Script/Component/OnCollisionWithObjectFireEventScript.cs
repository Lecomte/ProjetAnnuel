using UnityEngine;
using UnityEngine.Events;

public class OnCollisionWithObjectFireEventScript : MonoBehaviour {

    [SerializeField]
    private byte _layer;

    [SerializeField]
    private EntityStatisticScript _entityStat;

    [SerializeField]
    private ColliderEvent eventToFire;

    void Start()
    {
        if (eventToFire == null)
            Debug.Log("Aucun event renseigné.");
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == _layer)
            if (eventToFire != null)
                eventToFire.Invoke(collision.collider);
    }
}
