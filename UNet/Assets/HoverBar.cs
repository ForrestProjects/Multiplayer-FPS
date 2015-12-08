using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class HoverBar : NetworkBehaviour {

	public Player Player;
	public Image Img;
	// Use this for initialization
	void Start () {
	
		Img = this.GetComponent<Image> ();
	}
	
	// Update is called once per frame
	void Update () {
	
		int currentHealth = Player.ReturnHealth ();

		
		float _HP = currentHealth / 100f;
		Debug.Log ("Floating Bar Converted and divided HP = " + _HP);
		Img.fillAmount = (_HP);

	}
}
