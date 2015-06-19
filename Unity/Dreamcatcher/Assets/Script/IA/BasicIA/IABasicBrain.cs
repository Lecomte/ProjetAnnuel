using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IABasicBrain : IAMakeDecision // cerveau basique fonctionnant de façon séquentielle et non pas contextuelle
{

	int decision = -1;

	public override IADecision MakeDecision(IAScript ia)
	{
		IADecisionType decisionType = IADecisionType.DO_NOTHING;
		int nbPossibility = 0;
		int nb_iteration = 0;  // nombre de decisions tentées, si plus de 10 tentatives échouent, on décide de ne rien faire



		while (nbPossibility == 0 && nb_iteration < 10) 
		{
			nb_iteration ++;
			switch (decision) 
			{		
				case 0:
					decisionType = IADecisionType.ATTACK;
					nbPossibility = ia.NbAttacks;
				break;

				case 1:
					decisionType = IADecisionType.DEFEND;
					nbPossibility = ia.NbDefends;
				break;

				case 2:
					decisionType = IADecisionType.DO_NOTHING;
					nbPossibility = 1;
				break;
				/*
				case 3:
					decisionType = IADecisionType.SPECIAL;
					nbPossibility = ia.NbSpecials;
				break;

				case 4:
					decisionType = IADecisionType.FLEE;
					nbPossibility = ia.NbFlees;
				break;
				*/
				default:
					decisionType = IADecisionType.DO_NOTHING;
					decision = -1;		
				break;	
			}
			decision++;
		}

		if (nb_iteration >= 10)
			decisionType = IADecisionType.DO_NOTHING;

		return new IADecision (decisionType, Random.Range(0, nbPossibility) );
	}
}