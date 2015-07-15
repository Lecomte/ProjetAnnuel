using UnityEngine;
using System.Collections;

public class IABasicAttack : IABehaviour 
{
	[SerializeField]
	public Animator weaponAnimator;

	[SerializeField]
	protected MobStatisticScript stats;

	public Collider target;

	// Use this for initialization
	void Start () {
	
	}


	protected virtual IEnumerator AttackCoroutine()
	{
		weaponAnimator.SetBool ("isAttacking", true);
		stats.isAttacking = true;
		yield return new WaitForSeconds (0.8f);
		stats.isAttacking = false;
		weaponAnimator.SetBool ("isAttacking", false);
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
		return 1f;	
	}

	// Update is called once per frame
	void Update () {
	
	}
}
