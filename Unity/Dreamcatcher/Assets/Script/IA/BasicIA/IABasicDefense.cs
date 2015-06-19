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

	float time = 2f;
	public override void SetOptionsFromDecision(IADecision decision)
	{
		time = decision.Time;
	}

	public override float Act()
	{
		stats.TemporaryIncreaseResistance ( 10, time);
		return 2f;
	}
}
