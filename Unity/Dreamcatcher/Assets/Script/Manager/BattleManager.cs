using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class BattleManager : MonoBehaviour {
	
    public UnityEvent KillEnemyEvent;

    public DamageEvent TakeDamage;

	public SpawnEvent			EnemySpawnEvent;
	public ColliderEvent		EnemyDeathEvent;

    [SerializeField]
    private UnitManager 		unitManager;

	public void Start()
	{

	}

	public void fireEnemySpawnEvent( int type)
	{
		if (this.EnemySpawnEvent != null)
			this.EnemySpawnEvent.Invoke (type);
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
        if (entityScript != null) { damage = entityScript.getDamage(); }
        if (this.TakeDamage != null && damage != -1)
            TakeDamage.Invoke(damage);
    }
}
