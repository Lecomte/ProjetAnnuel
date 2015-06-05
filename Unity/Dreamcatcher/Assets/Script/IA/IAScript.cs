using UnityEngine;
using System.Collections;

public class IAScript : MonoBehaviour {
	
	[SerializeField] 
	public UnitManager unitManager;

	[SerializeField]
	public Animator weaponAnimator;

	[SerializeField]
	MobStatisticScript stats;

	// Use this for initialization
	void Start () {
		StartCoroutine ("IA");
	}
	
	void Reset() {
		
	}

	IEnumerator IA() {

		while (true) {
			Collider player = unitManager.colliderPlayerList[0].collider; // obtiens le premier joueur // TODO : chercher le joueur le plus proche quand y'en aura plusieurs
			while (stats.getCurrentHealth() > 0) {
				Vector3 direction = (player.transform.position - transform.position);
				direction.y = 0;
				float dist = direction.sqrMagnitude;
				direction.Normalize ();
		
				if (dist > 6) {
					this.transform.position += direction * Time.fixedDeltaTime * 10;
					this.transform.LookAt(player.transform.position);
					yield return new WaitForFixedUpdate ();
				} 
				else 
				{
					weaponAnimator.SetBool ("isAttacking", true);
					stats.isAttacking = true;
					yield return new WaitForSeconds (0.8f);
					stats.isAttacking = false;
					weaponAnimator.SetBool ("isAttacking", false);
					yield return new WaitForSeconds (1f);

					float time = 0;
					Vector3 dir =  (transform.position - player.transform.position);	// direction opposé du joueur
					dir.y = 0;
					dir.Normalize();
					float angle = Vector2.Angle( new Vector2(1, 0),new Vector2(dir.x, dir.z) ) + Mathf.PI/2; // on récupére l'angle xz la direction voulue et la droite et on y ajouter 90° pour éviter d'etre bloqué dans un coin si poursuivi
					dir.x = Mathf.Cos(angle);
					dir.z = -Mathf.Sin(angle); // sin(angle)² + cos(angle)² = 1 => |dir| = 1;  CQFD

					while (time < 2.0)
					{					
						this.transform.position += dir * Time.fixedDeltaTime * 10;
						this.transform.LookAt(this.transform.position - dir);
						time += Time.fixedDeltaTime;
						yield return new WaitForFixedUpdate();
					}

				}
			}
			yield return new WaitForSeconds (1.5f);
		}
		
	}

	// Update is called once per frame
	void Update () {		

	}
	
}
