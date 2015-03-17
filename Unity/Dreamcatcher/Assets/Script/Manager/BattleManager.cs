using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class BattleManager : MonoBehaviour {
	
    public UnityEvent KillEnemyEvent;

    public DamageEvent TakeDamage;

	public StringEvent			EnemySpawnEvent;
	public ColliderEvent		EnemyDeathEvent;

    [SerializeField]
    private UnitManager 		unitManager;

	public void Start()
	{
		this.EnemySpawnEvent = new StringEvent ();
		this.EnemyDeathEvent = new ColliderEvent ();
		this.TakeDamage 	 = new DamageEvent();
	}

	public void fireEnemySpawnEvent( string name)
	{
		if (this.EnemySpawnEvent != null)
			this.EnemySpawnEvent.Invoke (name);
	}
	
	public void fireEnemyDeath(Collider collider)
	{
		if (this.EnemyDeathEvent != null)
			this.EnemyDeathEvent.Invoke (collider);
	}

    public void fireTakeDamageScript(Collider collider)
    {
        int damage = -1;
        EntityStatisticScript entityScript = unitManager.EntityDictionary[collider];
        if (entityScript != null) { damage = entityScript.Damage; }
        if (this.TakeDamage != null && damage != -1)
            TakeDamage.Invoke(damage);
    }
}
