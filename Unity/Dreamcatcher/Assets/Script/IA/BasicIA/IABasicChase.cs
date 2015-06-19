using UnityEngine;
using System.Collections;

public class IABasicChase : IABehaviour 
{
	[SerializeField]
	public Animator weaponAnimator;
	
	[SerializeField]
	protected MobStatisticScript stats;

	public float time = -1; // -1 = temps non determiné(jusqu'à objectif atteint), > 0 temps max de poursuite
	public float speed = 10;
	public float dist_to_reach = 6;

	public Collider target;
	
	// Use this for initialization
	void Start () {
		
	}

	public override void SetOptionsFromDecision(IADecision decision) 
	{

		if (decision is IADecisionChase)
		{
			time = (decision as IADecisionChase).Time;
			dist_to_reach = (decision as IADecisionChase).Distance;
		}
		else 
		{
			time = -1;
			dist_to_reach = 6;
		}

	}

	public override float Act()
	{
		Vector3 direction = (target.transform.position - transform.position);
		direction.y = 0;
		
		float dist = direction.sqrMagnitude;
		
		direction.Normalize ();
		
		if (dist > dist_to_reach) {
			this.transform.position += direction * Time.deltaTime * speed;
			this.transform.LookAt (target.transform.position);
		} 
		else 
		{
			return 0;		
		}

		if (time > 0)
		{
			time -= Time.deltaTime;
			if (time < 0)
			{
				time = 0;
				return 0;
			}

		}

		return -1;
	}

}

