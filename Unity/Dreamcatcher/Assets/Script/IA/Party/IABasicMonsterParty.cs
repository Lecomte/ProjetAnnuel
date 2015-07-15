using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IABasicMonsterParty : IAMonsterParty
{
	[SerializeField]
	public int distNear = 20; // distance d'initialisation de l'attaque de groupe
	bool orderToAttack = false;
	bool canSetOrderToAttack = true;

	public override IADecision	getPartyDecision(IAScript script)
	{
		if (members.Count < 3) // décision de se défendre si le groupe n'a pas assez de membres par exemple
		{
			return new IADecision(IADecisionType.DEFEND, Random.Range(0,script.NbDefends) );
		}

		if (!orderToAttack && CountReadyToAttack () >= 2) 
		{
			SetOrderToAttack();
		}

		if ( (script.transform.position - target.transform.position).sqrMagnitude > distNear)
		{
			return new IADecisionChase(0.03f, distNear);
		}
		else 
		{
			if (orderToAttack)
				return new IADecision(IADecisionType.ATTACK, Random.Range(0, script.NbAttacks) );
			else 
				return new IADecision(IADecisionType.DEFEND, Random.Range(0, script.NbDefends), 0.1f);
		}
	}

	int CountReadyToAttack()
	{
		int count = 0;

		foreach (var script in members)
		if ((script.transform.position - target.transform.position).sqrMagnitude < distNear + 2) 
		{
			count++;
		}

		return count;
	}


	IEnumerator SetOffOrderLater()
	{
		yield return new WaitForSeconds (0.3f); // on donne l'ordre pendant un temps pour etre sur que tout le monde qui peut l'appliquer le recoive
		orderToAttack = false;
		yield return new WaitForSeconds (2.0f); // on arrete d'attaquer pendant 2s avant de pouvoir reattaquer
		canSetOrderToAttack = true;
		yield return null;
	}

	void SetOrderToAttack()
	{
		if (orderToAttack || !canSetOrderToAttack)
			return;

		StopCoroutine ("SetOffOrderLater");

		orderToAttack = true;		
		canSetOrderToAttack = false;
		StartCoroutine ("SetOffOrderLater");	
	}

	// Use this for initialization
	void Start ()
	{
		members = new List<IAScript> ();	
	}


	void Update ()
	{
		// vidange des membres morts ( remplaçable par la prise en compte de l'event quand une unit meurt)
		List <IAScript> corbeille = new List<IAScript> ();
		
		foreach (var script in members) 
			if (script.isDead ())
				corbeille.Add (script);
		
		foreach (var script in corbeille) 
			members.Remove (script);


		
	}
	

}

