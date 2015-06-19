using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public enum TypeUnit{STANDARD = 0, BOSS = 10  };

public class IAScript : MonoBehaviour {

	[SerializeField]
	float DoNothingTime = 1.5f;

	[SerializeField] 
	public UnitManager unitManager;
	
	[SerializeField]
	MobStatisticScript stats;

	[SerializeField]
	IAMakeDecision brain;

	[SerializeField] 
	List <IABasicAttack> attacks;

	[SerializeField] 
	List <IABasicDefense> defends;

	[SerializeField] 
	List <IABasicFlee> flees;

	[SerializeField]
	List <IABehaviour> specials;

	
	[SerializeField]
	List <IABasicChase> chases;

	public IADecision lastDecisionMade;


	public int NbAttacks { get { return attacks.Count; } }
	public int NbDefends { get { return defends.Count; } }
	public int NbFlees 	 { get { return flees.Count; } }
	public int NbSpecials { get { return specials.Count; } }

	public bool isDead()
	{
		return stats.getCurrentHealth () <= 0;
	}

	// Use this for initialization
	void Start () {
		StartCoroutine ("IA");
		stats.isAttacking = stats.isDefending = false;
	}
	
	void Reset() {
		
	}


	IEnumerator IA() {

		while (true) 
		{
			IADecision decision = lastDecisionMade = brain.MakeDecision( this );

			float tps = -1;

			while (tps <= -1)
			{
				switch (decision.Type)
				{
					case IADecisionType.ATTACK : 

					attacks[decision.Numero].SetOptionsFromDecision (decision);
					tps = attacks[decision.Numero].Act();

					break;

					case IADecisionType.DEFEND : 

					defends[decision.Numero].SetOptionsFromDecision (decision);
					tps = defends[decision.Numero].Act();
						
					break;

					case IADecisionType.DO_NOTHING : 
						
					tps = DoNothingTime;
													
					break;

					case IADecisionType.FLEE : 
						
					flees[decision.Numero].SetOptionsFromDecision (decision);
					tps = flees[decision.Numero].Act();

					break;

					case IADecisionType.SPECIAL : 
						
					specials[decision.Numero].SetOptionsFromDecision (decision);
					tps = specials[decision.Numero].Act();
						
					break;

					case IADecisionType.CHASE:

					chases[decision.Numero].SetOptionsFromDecision (decision);

					tps = chases[decision.Numero].Act();

					break;

					default:
						
					tps = 0;	

					break;
				}
			
				if (tps <= -1)
					yield return new WaitForFixedUpdate();
				else if (tps > 0.0f)
					yield return new WaitForSeconds(tps);
			}

			yield return new WaitForFixedUpdate();

		}
		
	}
	void OnEnable () {
		Reset ();
		StopCoroutine ("IA");
		StartCoroutine ("IA");	
	}

	// Update is called once per frame
	void Update () {		

	}
	
}
