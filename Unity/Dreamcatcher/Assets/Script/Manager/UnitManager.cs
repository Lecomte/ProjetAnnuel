using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitManager : MonoBehaviour {

    [SerializeField]
    private List<ColliderEntityClass> 					colliderEntityList;
    public Dictionary<Collider, EntityStatisticScript> 	EntityDictionary;
	public Dictionary<string, List<Collider> >			UnusedUnits;

	public void Start()
	{
		CreateEntityDictionary ();
		CreateUnusedUnitsDictionary ();
	}

	public void CreateEntityDictionary()
	{
		this.EntityDictionary = new Dictionary<Collider, EntityStatisticScript>(colliderEntityList.Count);
		
		foreach(ColliderEntityClass EntityClass in colliderEntityList)
		{
			this.EntityDictionary.Add(EntityClass.collider, EntityClass.script);
		}
	}
	
	public void CreateUnusedUnitsDictionary()
	{
		this.UnusedUnits = new Dictionary<string, List<Collider> > ();
		
		foreach (ColliderEntityClass unit in colliderEntityList) 
		{
			if (UnusedUnits.ContainsKey(unit.name) )
				UnusedUnits[unit.name].Add(unit.collider);
			else 
			{
				List<Collider> colliders = new List<Collider>();
				colliders.Add(unit.collider);
				UnusedUnits.Add(unit.name, colliders);
			}
		}
	}

	private void AddUnusedUnit(Collider collider)
	{
		EntityStatisticScript unit = EntityDictionary [collider];

		if (UnusedUnits.ContainsKey(unit.name) )
			UnusedUnits[unit.name].Add(unit.collider);
		else 
		{
			List<Collider> colliders = new List<Collider>();
			colliders.Add(unit.collider);
			UnusedUnits.Add(unit.name, colliders);
		}
	}
	
	public void OnUnitDeath(Collider unitCollider)
	{
		AddUnusedUnit (unitCollider);
	}

	public ColliderEntityClass PullUnit(string name)
	{
		if ( UnusedUnits.ContainsKey(name) )
		{
			if( UnusedUnits[name].Count == 0)
				return null;
			Collider first = UnusedUnits[name][0];
			UnusedUnits[name].RemoveAt(0);
			return first;
		}

		return null;
	}

}
