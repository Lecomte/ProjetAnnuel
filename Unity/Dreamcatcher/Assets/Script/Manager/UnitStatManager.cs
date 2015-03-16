using UnityEngine;
using System.Collections;

public class UnitStatManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void InitStats ( EntityStatisticScript script, float levelIntensity)
	{
		script.Damage = levelIntensity * 4f * StatVariation() ;
		script.CurrentHealth = levelIntensity * 20f  * StatVariation();
		script.Resistance = levelIntensity * 2f  * StatVariation();
	}

	private float StatVariation()
	{
		return Random.Range (0.8f, 1.2f); 
	}
}

