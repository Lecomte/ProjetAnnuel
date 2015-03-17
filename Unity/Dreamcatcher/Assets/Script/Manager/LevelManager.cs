using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour 
{


	public bool spawnUnits { get; set;}
	float elapsedTime = 0f;

	Vector2 dancefloor_size = new Vector2(80,80);
	Vector2 dancefloor_origin = new Vector2(-40,-40);

	[SerializeField]
	public UnitStatManager unitStatManager;

	[SerializeField]
	public BattleManager battleManager;

	[SerializeField] 
	public SpawnManager spawnManager;

	[SerializeField]
	public UnitManager unitManager;


	public void Start()
	{
		spawnUnits = true;
		battleManager.EnemyDeathEvent.AddListener (spawnManager.OnUnitDeath);
	}

	public void Update()
	{
		if (spawnUnits) 
		{

			if (elapsedTime >= 60f / GetBPM ()) 
			{
				Collider collider = spawnManager.PullUnit ("Type1");
				if (collider == null)
					return;			

				unitStatManager.InitStats (unitManager.EntityDictionary [collider], GetIntensity ());
				collider.gameObject.SetActive (true);
				collider.gameObject.transform.position = new Vector3 (Random.Range (dancefloor_origin.x, dancefloor_origin.x + dancefloor_size.x), 0, Random.Range (dancefloor_origin.y, dancefloor_origin.y + dancefloor_size.y)); 

				elapsedTime = 0;
				battleManager.fireEnemySpawnEvent ("Type1");	
			} 
			else
			{
				elapsedTime += Time.deltaTime;
			}
		} 
		else
			elapsedTime = 0f;
	}

	public float GetBPM()
	{
		return 15f; // à remplacer par la récuperation du bpm
	}
	
	public float GetIntensity()
	{
		return 4f;	// à remplacer par la récuperation de l'energie diffusée
	}
}
