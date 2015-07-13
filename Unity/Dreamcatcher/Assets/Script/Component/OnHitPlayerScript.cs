using UnityEngine;
using UnityEngine.Events;

public class OnHitPlayerScript : MonoBehaviour {

    [SerializeField]
    private byte _layer;
    [SerializeField]
    private byte _weaponLayer;

    [SerializeField]
    private UnitManager unitManager;

	
	[SerializeField]
	private MobStatisticScript attacker_script;

    [SerializeField]
	private HitMobEvent eventToFire;

	void OnCollisionEnter(Collision collision)
    {
		if (!attacker_script.isAttacking) // l'attaquant n'attaque pas, pas de degats
			return;
		Collider collider = collision.collider;
        if (collider.gameObject.layer == _layer || collision.gameObject.layer == _weaponLayer)
		if (eventToFire != null) {
			Debug.Log("player being hurt " + collider.name + " by " + name);
			eventToFire.Invoke (collider, attacker_script.getDamage() );
		}
    }
}
