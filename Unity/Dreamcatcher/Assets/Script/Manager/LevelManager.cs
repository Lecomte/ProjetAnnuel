using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour 
{


	public bool spawnUnits { get; set;}

	Vector3 dancefloor_size;
	Vector3 dancefloor_origin;

	[SerializeField]
	private GameObject danceFloor;

	[SerializeField]
	public BattleManager battleManager;

	[SerializeField]
	public UnitManager unitManager;

	[SerializeField]
	private float bpm = 20f;

	[SerializeField]
	private float intensity = 15f;


	public void Start()
	{
		Init (); // je sais pas quand tu veux appeler les fonctions d'init alors je fous là en attendant pour que ça se lance quand je lance le projet
	}

	public void Init()
	{
		spawnUnits = true;
		// battleManager.EnemyDeathEvent.AddListener (spawnManager.OnUnitDeath);

		dancefloor_origin = danceFloor.transform.position;
		dancefloor_size = danceFloor.GetComponent<Renderer> ().bounds.size; // évite le getcomponent dans la coroutine, pas moyen d'accèder directement  "renderer"

		StartCoroutine ("SpawnCoroutine");
	}

	public IEnumerator SpawnCoroutine()
	{
		while (true) { // game loop en cours

			yield return new WaitForSeconds(GetBPM()/60f );

			while (!spawnUnits){
				yield return new WaitForSeconds(0.2f);
			}

			Collider collider = null; 
			collider = unitManager.PullUnit (1);
			while (collider == null)
			{
				yield return new WaitForSeconds(0.2f);			
				collider =  unitManager.PullUnit (1);
			}

			InitStats ( (MobStatisticScript) unitManager.EntityDictionary [collider]);
			collider.gameObject.SetActive (true);
			collider.gameObject.transform.position = new Vector3 (Random.Range (dancefloor_origin.x, dancefloor_origin.x + dancefloor_size.x), 0, Random.Range (dancefloor_origin.y, dancefloor_origin.z + dancefloor_size.z)); 
			battleManager.fireEnemySpawnEvent (1);	
		
		}
	}

    public void InitStats(MobStatisticScript script)
	{
        // TODO Resistance SetDamage SetCurrentHealth SetMaxHealth SetResistance
		float levelIntensity = GetIntensity ();
        script.SetDamage((int)(levelIntensity * 4f * StatVariation()));
        script.SetCurrentHealth((int)(levelIntensity * 20f * StatVariation()));
        script.SetMaxHealth( script.getCurrentHealth() );
		script.SetResistance((int) (levelIntensity * 0.1f  * StatVariation()));
	}
	
	private float StatVariation()
	{
		return Random.Range (0.8f, 1.2f); 
	}

	public void Update()
	{

	}

	public float GetBPM()
	{
		return bpm; // à remplacer par la récuperation du bpm
	}
	
	public float GetIntensity()
	{
		return intensity;	// à remplacer par la récuperation de l'energie diffusée
	}
}
