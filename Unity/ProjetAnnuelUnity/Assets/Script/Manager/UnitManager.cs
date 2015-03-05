using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitManager : MonoBehaviour {

    [SerializeField]
    private List<ColliderEntityClass> colliderEntityList;

    public Dictionary<Collider, EntityStatisticScript> EntityDictionary;

    public void CreateEntityDictionary()
    {
        this.EntityDictionary = new Dictionary<Collider, EntityStatisticScript>();
        foreach(ColliderEntityClass EntityClass in colliderEntityList)
        {
            this.EntityDictionary.Add(EntityClass.collider, EntityClass.script);
        }
    }
}
