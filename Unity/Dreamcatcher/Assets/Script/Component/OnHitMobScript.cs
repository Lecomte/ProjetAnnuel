using UnityEngine;
using UnityEngine.Events;

public class OnHitMobScript : MonoBehaviour
{

    [SerializeField]
    private byte _layer;
    [SerializeField]
    private byte _weaponLayer;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private PlayerStatisticScript stats; // les stats du joueur pour pouvoir enlever les degats
    [SerializeField]
    private UnitManager unitManager;
    [SerializeField]
    private HitMobEvent eventToFire;

    void OnCollisionEnter(Collision collision)
    {
		Debug.Log ("contact du poing du joueur avec un objet");
        if (collision.gameObject.layer == _layer || collision.gameObject.layer == _weaponLayer)
        {
			Debug.Log ("contact du poing du joueur avec un objet compatible");
            if (eventToFire != null)
            {
				Debug.Log("Collision fire " + collision.gameObject.layer + " == " + _layer + " - Damage : " + (int)animator.GetFloat("DamageCoeff") * stats.getDamage());

                eventToFire.Invoke(collision.collider, (int)animator.GetFloat("DamageCoeff") * stats.getDamage());
                if (animator.GetInteger("ActionPoints")<10)
                    animator.SetInteger("ActionPoints", animator.GetInteger("ActionPoints")+1);
            }
        }
        stats.AddComboPoints(animator.GetInteger("comboPoints"));
    }
}
