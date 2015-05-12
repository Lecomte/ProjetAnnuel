using UnityEngine;
using System.Collections;

public abstract class IABehaviour : MonoBehaviour
{
	public abstract float Act(); // agit et renvoie le temps de l'action, renvoie -1 si l'action n'a pas un temps fini et 0 si l'action est finie
	public virtual bool Acting
	{
		get{ return false;} 
		set{}
	}
}