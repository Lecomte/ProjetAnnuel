using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour {

	[SerializeField]
	public UnitManager unitManager;


	public Dictionary<string, List<Collider> >			UnusedUnits;

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
		this.UnusedUnits = new Dictionary<string, List<Collider> > ();
		
		foreach (ColliderEntityClass unit in unitManager.colliderEntityList) 
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
		EntityStatisticScript unit = unitManager.EntityDictionary [collider];
		
		if (UnusedUnits.ContainsKey(unit.name) )
			UnusedUnits[unit.name].Add(unit.GetComponent<Collider>());
		else 
		{
			List<Collider> colliders = new List<Collider>();
			colliders.Add(unit.GetComponent<Collider>());
			UnusedUnits.Add(unit.name, colliders);
		}
	}

	public void OnUnitDeath(Collider unitCollider)
	{
		AddUnusedUnit (unitCollider);
	}

	public Collider PullUnit(string name)
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
