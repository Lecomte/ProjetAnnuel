using UnityEngine;
using System.Collections;

public class RedOnAttacking : MonoBehaviour {

	[SerializeField]
	private Renderer my_renderer;

	[SerializeField]
	private MobStatisticScript mob;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if (mob.isAttacking) {
			my_renderer.material.color = new Color(1.0f,0.5f,0.5f);
		} else {
			my_renderer.material.color = new Color(0.5f,0.5f,0.5f);
		}

	}
}
