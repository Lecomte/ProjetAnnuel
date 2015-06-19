using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IAMonsterParty : MonoBehaviour 
{
	protected List<IAScript> members;

	[SerializeField]
	public Collider target;

	public virtual void join(IAScript script)
	{
		members.Add (script);
	}

	public virtual IADecision	getPartyDecision(IAScript script)
	{
		return null;
	}

	// Use this for initialization
	void Start ()
	{
		members = new List<IAScript> ();	
	}

}
