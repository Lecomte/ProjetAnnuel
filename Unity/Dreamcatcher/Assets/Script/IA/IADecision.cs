using UnityEngine;
using System.Collections;

public enum IADecisionType {ATTACK = 0, DEFEND, FLEE, SPECIAL, DO_NOTHING };

public struct IADecision
{
	IADecisionType type;
	int numero;
	public IADecisionType Type { get{ return type;} } 
	public int Numero { get{ return numero;} } 

	public IADecision(IADecisionType type, int numero)
	{
		this.type = type;
		this.numero = numero;
	}
}


public abstract class IAMakeDecision : MonoBehaviour
{
	public abstract IADecision MakeDecision(IAScript ia);
}
