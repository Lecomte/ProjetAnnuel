using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitManager : MonoBehaviour {
	
	[SerializeField]
	public List<ColliderEntityClass> 					colliderEntityList;

	[SerializeField]
	public List<ColliderEntityClass> 					colliderPlayerList;

	public Dictionary<Collider, EntityStatisticScript> 	EntityDictionary;
	public Dictionary<Collider, EntityStatisticScript>  PlayerDictionary;

	public Dictionary<EntityStatisticScript, Collider> 	ColliderDictionary;
	
	[SerializeField]
	public LevelManager levelManager;
	
	private Dictionary<int, List<Collider> >			UnusedUnits;
	private List <Collider> 							ActiveUnits;
	
	public void Initialize()
	{
		CreateEntityDictionary ();
		CreateUnusedUnitsDictionary ();
	}
	
	public void CreateEntityDictionary()
	{
		this.PlayerDictionary = new Dictionary<Collider, EntityStatisticScript>(colliderEntityList.Count);
		this.EntityDictionary = new Dictionary<Collider, EntityStatisticScript>(colliderPlayerList.Count);
		this.ColliderDictionary = new Dictionary<EntityStatisticScript, Collider>(colliderEntityList.Count);
		
		foreach(ColliderEntityClass EntityClass in colliderEntityList)
		{
			this.EntityDictionary.Add(EntityClass.collider, EntityClass.script);
			this.ColliderDictionary.Add(EntityClass.script, EntityClass.collider);
		}

		foreach(ColliderEntityClass EntityClass in colliderPlayerList)
		{
			this.PlayerDictionary.Add(EntityClass.collider, EntityClass.script);
			this.ColliderDictionary.Add(EntityClass.script, EntityClass.collider);
		}

	}
	
	public void CreateUnusedUnitsDictionary() {
		ActiveUnits = new List<Collider> ();
		this.UnusedUnits = new Dictionary<int, List<Collider> > ();
		
		foreach (ColliderEntityClass collider_entity_class in this.colliderEntityList) 
		{
			EntityStatisticScript unit = this.EntityDictionary [collider_entity_class.collider];
			
			if (UnusedUnits.ContainsKey(unit.getEntityType()) )
				UnusedUnits[unit.getEntityType()].Add(collider_entity_class.collider);
			else 
			{
				List<Collider> colliders = new List<Collider>();
				colliders.Add(collider_entity_class.collider);
				UnusedUnits.Add(unit.getEntityType(), colliders);
			}
		}
	}
	
	private void AddUnusedUnit(Collider collider)
	{
		EntityStatisticScript unit = EntityDictionary [collider];
		
		if (UnusedUnits.ContainsKey (unit.getEntityType())) {
			UnusedUnits [unit.getEntityType()].Add (unit.GetComponent<Collider> ());
		}
		else 
		{
			List<Collider> colliders = new List<Collider>();
			colliders.Add(unit.GetComponent<Collider>());
			UnusedUnits.Add(unit.getEntityType(), colliders);
		}
	}
	
	public List <Collider> GetActiveUnits() {
		return ActiveUnits;
	}
	public void OnUnitDeath(EntityStatisticScript script)
	{
		if ( ! ColliderDictionary.ContainsKey (script)) {	// si l'unité qui vient de mourir n'est pas une unité connue du tableau
			// TODO : gérer les cas de script joueur
			return;
		}
		
		Collider unitCollider = ColliderDictionary[script];	// on récupére le collider correspondant au script reçu 
		levelManager.battleManager.fireEnemyDeath(unitCollider); // on prévient les intéréssés de la mort
		unitCollider.gameObject.SetActive (false);	// on désactive l'unité 
		AddUnusedUnit (unitCollider);
		ActiveUnits.Remove (unitCollider);
	}
	
	public Collider PullUnit(int type)
	{
		if ( UnusedUnits.ContainsKey(type) )
		{
			if( UnusedUnits[type].Count == 0)
				return null;
			
			Collider first = UnusedUnits[type][0];
			UnusedUnits[type].RemoveAt(0);
			ActiveUnits.Add(first);
			return first;
		}
		
		return null;
	}
	
	public EntityStatisticScript getEntityStatisticScript(Collider collider)
	{
		return this.EntityDictionary[collider];
	}

	public EntityStatisticScript getPlayerStatisticScript(Collider collider)
	{
		return this.PlayerDictionary[collider];
	}
	
	public void infligeDamageToCollider(Collider collider, int damage)
	{
		Debug.Log("infligeDamageToCollider : " + collider.name);
		this.EntityDictionary[collider].TakeDamage(damage);
	}

	public void infligeDamageToPlayerCollider(Collider collider, int damage)
	{
		Debug.Log("infligeDamageToPlayerCollider : " + collider.name);
		this.PlayerDictionary[collider].TakeDamage(damage);
	}
}
