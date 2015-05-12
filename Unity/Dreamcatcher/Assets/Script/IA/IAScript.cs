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

	public int NbAttacks { get { return attacks.Count; } }
	public int NbDefends { get { return defends.Count; } }
	public int NbFlees 	 { get { return flees.Count; } }
	public int NbSpecials { get { return specials.Count; } }


	// Use this for initialization
	void Start () {
		StartCoroutine ("IA");
	}
	
	void Reset() {
		
	}

	IEnumerator IA() {

		while (true) 
		{
			IADecision decision = brain.MakeDecision( this );

			float tps = -1;

			while (tps <= -1)
			{
				switch (decision.Type)
				{
					case IADecisionType.ATTACK : 

					tps = attacks[decision.Numero].Act();

					break;

					case IADecisionType.DEFEND : 

					tps = defends[decision.Numero].Act();
						
					break;

					case IADecisionType.DO_NOTHING : 
						
					tps = DoNothingTime;
													
					break;

					case IADecisionType.FLEE : 
						
					tps = flees[decision.Numero].Act();

					break;

					case IADecisionType.SPECIAL : 
						
					tps = specials[decision.Numero].Act();
						
					break;
				}
			
				if (tps <= -1)
					yield return new WaitForFixedUpdate();
				else if (tps > 0)
					yield return new WaitForSeconds(tps);
			}

			yield return new WaitForFixedUpdate();

		}
		
	}

	// Update is called once per frame
	void Update () {		

	}
	
}
