using UnityEngine;
using System.Collections;

public class IABasicFlee : IABehaviour 
{
	
	public Collider target;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator FleeCoroutine()
	{
		float time = 0;
		Vector3 dir =  (transform.position - target.transform.position);	// direction opposé du joueur
		dir.y = 0;
		dir.Normalize();
		float angle = Vector2.Angle( new Vector2(1, 0),new Vector2(dir.x, dir.z) ) + Mathf.PI/2; // on récupére l'angle xz la direction voulue et la droite et on y ajouter 90° pour éviter d'etre bloqué dans un coin si poursuivi
		dir.x = Mathf.Cos(angle);
		dir.z = -Mathf.Sin(angle); // sin(angle)² + cos(angle)² = 1 => |dir| = 1;  CQFD
		
		while (time < 1.8)
		{					
			this.transform.position += dir * Time.fixedDeltaTime * 10;
			this.transform.LookAt(this.transform.position - dir);
			time += Time.fixedDeltaTime;
			yield return new WaitForFixedUpdate();
		}

		yield return null;
	}
	
	public override float Act()
	{
		StopCoroutine  ("FleeCoroutine");
		StartCoroutine ("FleeCoroutine");
		return 2f;
	}

}
