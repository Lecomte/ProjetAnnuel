using UnityEngine;
using System.Collections;

public class IABoss1Attack : IABasicAttack 
{

	public MeshRenderer m_renderer;
	// Use this for initialization
	void Start () {
	
	}


	protected override IEnumerator AttackCoroutine()
	{
		m_renderer.material.color = new Color (1.0f, 0, 0);
		yield return new WaitForSeconds (0.2f);
		m_renderer.material.color = new Color (0.5f, 0.5f, 0.5f);
		yield return new WaitForSeconds (0.2f);
		m_renderer.material.color = new Color (1.0f, 0, 0);
		yield return new WaitForSeconds (0.2f);
		m_renderer.material.color = new Color (0.5f, 0.5f, 0.5f);
		yield return new WaitForSeconds (0.2f);
		float time = 0f;
		stats.isAttacking = true;
		while (time < 3f) {
			this.transform.Rotate(0, 3.14f * 2 * time, 0);
			yield return new WaitForFixedUpdate();
			time += Time.fixedDeltaTime;
		}
		// weaponAnimator.SetBool ("isAttacking", true);
;
		yield return new WaitForSeconds (0.2f);
		stats.isAttacking = false;
		// weaponAnimator.SetBool ("isAttacking", false);
		yield return null;
	}

	public override float Act()
	{
		Vector3 direction = (target.transform.position - transform.position);
		direction.y = 0;


		float dist = direction.sqrMagnitude;

		direction.Normalize ();

		if (dist > 9) 
		{
			this.transform.position += direction * Time.fixedDeltaTime * 10;
			this.transform.LookAt(target.transform.position);
			return -1;
		} 

		StopCoroutine ("AttackCoroutine");
		StartCoroutine ("AttackCoroutine");
		return 8f;	
	}

	// Update is called once per frame
	void Update () {
	
	}
}
