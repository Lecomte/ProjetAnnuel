using UnityEngine;
using System.Collections;

public enum IADecisionType {ATTACK = 0, CHASE, DEFEND, FLEE, SPECIAL, DO_NOTHING };



public class IADecision
{
	protected IADecisionType type;
	protected int numero = 0;
	protected float time = -1.0f;
	public float Time { get { return time; } }

	public IADecisionType Type { get{ return type;} } 
	public int Numero { get{ return numero;} } 

	public IADecision(IADecisionType type, int numero, float time = 1.0f )
	{
		this.type = type;
		this.numero = numero;
		this.time = time;
	}
}

public class IADecisionChase : IADecision
{

	float distance = 6;

	public float Distance { get { return distance; } }

	public IADecisionChase(float time = -1.0f, float distance = 6) : base(IADecisionType.CHASE,0)
	{
		this.time = time;
		this.distance = distance;
	}
}


public abstract class IAMakeDecision : MonoBehaviour
{
	public abstract IADecision MakeDecision(IAScript ia);
}
