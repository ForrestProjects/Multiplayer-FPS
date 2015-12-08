using UnityEngine;
using System.Collections;

public class FTIE01_DeadTime : MonoBehaviour {

	public float deadtime;

	void Awake (){
		if (deadtime != 0) {
			Destroy (gameObject, deadtime);
		}
	}
	
	
	void Update () {
	
	}
}
