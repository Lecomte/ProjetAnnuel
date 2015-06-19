using UnityEngine;
using System.Collections;

public class BlueOnDefending : MonoBehaviour {
	
	[SerializeField]
	private Renderer my_renderer;
	
	[SerializeField]
	private MobStatisticScript mob;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		if (mob.isDefending) {
			my_renderer.material.color = new Color( 0.5f ,0.5f, 1f);
		} else {
			my_renderer.material.color = new Color(0.5f,0.5f,0.5f);
		}
		
	}
}
