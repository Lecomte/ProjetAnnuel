using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class BattleManager : MonoBehaviour {

    public UnityEvent SpawnEnnemyEvent;
    public UnityEvent KillEnnemyEvent;

    public DamageEvent TakeDamage;
    [SerializeField]
    private UnitManager unitManager;

    public void fireSpawnEnnemyEvent()
    {
        if (this.SpawnEnnemyEvent != null)
            this.SpawnEnnemyEvent.Invoke();
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
