using UnityEngine;
using UnityEngine.Networking;

public class PlayerDeath : NetworkBehaviour {

	private Player PlayerScript;
	private PlayerSetup PSetup;




	void Start () {

		PSetup = GetComponent<PlayerSetup> ();
		PlayerScript = GetComponent<Player> ();
		PlayerScript.EventDie += DisablePlayer;

	}

	void OnDisable(){

		PlayerScript.EventDie -= DisablePlayer;
	}
	


	void DisablePlayer(){

		GetComponent<PlayerShoot> ().enabled = false;
		GetComponent<SphereCollider> ().enabled = false;
		GetComponent<Rigidbody> ().detectCollisions = false;
		//gameObject.layer = LayerMask.NameToLayer (DeadLayerName);
		if (isLocalPlayer) {
			GetComponent<PlayerController> ().enabled = false;
			GetComponent<PlayerMotor> ().enabled = false;
			GameObject Crosshair = GameObject.Find ("GameManager").GetComponent<GameManagerReferences>().crosshairHolder;
			Crosshair.SetActive(false);
			GameObject LocalHealthBar = GameObject.Find ("GameManager").GetComponent<GameManagerReferences>().LocalHealthBar;
			LocalHealthBar.SetActive (false);
			GameObject JetpackBar = GameObject.Find ("GameManager").GetComponent<GameManagerReferences>().EnergyBarHolder;
			JetpackBar.SetActive (false);
			GameObject AmmoObject = GameObject.Find ("GameManager").GetComponent<GameManagerReferences>().AmmoObject;
			AmmoObject.SetActive (false);


		}
		Renderer[]renderers = GetComponentsInChildren<Renderer> ();
		
		foreach (Renderer rens in renderers) {
			
			rens.enabled = false;
		}
		
		PlayerScript.isDead = true;
	

		if (isLocalPlayer) {

			GameObject.Find ("GameManager").GetComponent<GameManagerReferences>().respawnButton.SetActive(true); 
		}
	}
}
