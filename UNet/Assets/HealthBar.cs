using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class HealthBar : MonoBehaviour {

	public Player Player;
	public Image Bar;

	void Start(){

	}

	public void SyncHP(int HP){

			float _HP = HP / 100f;
			Debug.Log ("Converted and divided HP = " + _HP);
			Bar.fillAmount = (_HP);

	}
	void Update () {

	}
}
