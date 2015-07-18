using UnityEngine;
using System.Collections;

public class IAPartyBrain : IABasicBrain
{
	[SerializeField]
	int id_party;

	IAMonsterParty party = null;

	public override IADecision MakeDecision(IAScript ia)
	{
		//return base.MakeDecision(ia);
		if (party == null) 
		{
			party = ia.unitManager.getMonsterParty(id_party);	
			if (party != null)
				party.join(ia);
		}

		IADecision decision = null;
		decision = party == null? null : party.getPartyDecision( ia );

		if (decision == null) // le groupe n'existe pas ou il envoie null pour indiquer de prendre soi meme une decision
		{
			//return new IADecision(IADecisionType.DEFEND,0);
			return base.MakeDecision(ia);
		}

		return decision;
	}

}

