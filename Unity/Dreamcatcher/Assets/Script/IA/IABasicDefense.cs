using UnityEngine;
using System.Collections;

public class IABasicDefense : IABehaviour {


	[SerializeField]
	MobStatisticScript stats;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override float Act()
	{
		stats.TemporaryIncreaseResistance ( 10, 2f);
		return 2f;
	}
}
