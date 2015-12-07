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
