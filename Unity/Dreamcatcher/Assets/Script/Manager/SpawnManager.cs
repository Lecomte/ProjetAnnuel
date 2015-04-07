using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour {

	[SerializeField]
	public UnitManager unitManager;


	public Dictionary<int, List<Collider> >			UnusedUnits;

	// Use this for initialization
	void Start () 
	{
		CreateUnusedUnitsDictionary ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void CreateUnusedUnitsDictionary()
	{
		this.UnusedUnits = new Dictionary<int, List<Collider> > ();
		
		foreach (ColliderEntityClass collider_entity_class in unitManager.colliderEntityList) 
		{
			EntityStatisticScript unit = unitManager.EntityDictionary [collider_entity_class.collider];

			if (UnusedUnits.ContainsKey(unit.EntityType) )
				UnusedUnits[unit.EntityType].Add(collider_entity_class.collider);
			else 
			{
				List<Collider> colliders = new List<Collider>();
				colliders.Add(collider_entity_class.collider);
				UnusedUnits.Add(unit.EntityType, colliders);
			}
		}
	}

	private void AddUnusedUnit(Collider collider)
	{
		EntityStatisticScript unit = unitManager.EntityDictionary [collider];
		
		if (UnusedUnits.ContainsKey (unit.EntityType)) {
			UnusedUnits [unit.EntityType].Add (unit.GetComponent<Collider> ());
		}
		else 
		{
			List<Collider> colliders = new List<Collider>();
			colliders.Add(unit.GetComponent<Collider>());
			UnusedUnits.Add(unit.EntityType, colliders);
		}
	}

	public void OnUnitDeath(Collider unitCollider)
	{
		AddUnusedUnit (unitCollider);
	}

	public Collider PullUnit(int type)
	{
		if ( UnusedUnits.ContainsKey(type) )
		{
			if( UnusedUnits[type].Count == 0)
				return null;

			Collider first = UnusedUnits[type][0];
			UnusedUnits[type].RemoveAt(0);
			return first;
		}
		
		return null;
	}
}
