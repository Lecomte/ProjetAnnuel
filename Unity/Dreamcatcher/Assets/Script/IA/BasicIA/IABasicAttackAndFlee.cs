using UnityEngine;
using System.Collections;

public class IABasicAttackAndFlee : IABasicAttack
{


	protected override IEnumerator AttackCoroutine()
	{
		weaponAnimator.SetBool ("isAttacking", true);
		stats.isAttacking = true;
		yield return new WaitForSeconds (0.8f);
		stats.isAttacking = false;
		weaponAnimator.SetBool ("isAttacking", false);


		float time = 0;
		while (time < 1.0) 
		{
			Vector3 direction = (target.transform.position - transform.position);
			direction.y = 0;
			direction.Normalize ();

			this.transform.position -= direction * Time.deltaTime * 10;
			this.transform.LookAt (target.transform.position);
			yield return new WaitForFixedUpdate();
			time += Time.deltaTime;
		}

		yield return null;
	}
	
	public override float Act()
	{
		Vector3 direction = (target.transform.position - transform.position);
		direction.y = 0;
		
		float dist = direction.sqrMagnitude;
		
		direction.Normalize ();
		
		if (dist > 6) 
		{
			this.transform.position += direction * Time.deltaTime * 10;
			this.transform.LookAt(target.transform.position);
			return -1;
		} 
		
		StopCoroutine ("AttackCoroutine");
		StartCoroutine ("AttackCoroutine");
		return 2f;	
	}

	// Use this for initialization
	void Start ()
	{
	
	}

	
	// Update is called once per frame
	void Update ()
	{
	
	}
}

