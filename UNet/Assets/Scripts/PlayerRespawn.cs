using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerRespawn : NetworkBehaviour {
	//string PlayerLayerName = "Player";
	private Player PlayerScript;
	private PlayerSetup PSetup;
	private GameObject respawnButton;
	// Use this for initialization
	void Start () {
	
		PlayerScript = GetComponent<Player> ();
		PSetup = GetComponent<PlayerSetup> ();
		PlayerScript.EventRespawn += EnablePlayer;
		SetRespawnButton ();
	}

	void SetRespawnButton(){
		if (isLocalPlayer) {

			respawnButton = GameObject.Find ("GameManager").GetComponent<GameManagerReferences>().respawnButton;
			respawnButton.GetComponent<Button>().onClick.AddListener(CommenceRespawn);
			respawnButton.SetActive (false);
		}

	}
	// Update is called once per frame
	void Update () {
	
	}

	void OnDisable(){

		PlayerScript.EventRespawn -= EnablePlayer;
	}

	void EnablePlayer(){
		//PSetup.OnStartClient ();
		Debug.Log ("RESPAWNING");
		GetComponent<PlayerShoot> ().enabled = true;
		GetComponent<SphereCollider> ().enabled = true;
		GetComponent<Rigidbody> ().detectCollisions = true;
		if (isLocalPlayer) {
			GetComponent<PlayerController> ().enabled = true;
			GetComponent<PlayerMotor> ().enabled = true;
			GameObject Crosshair = GameObject.Find ("GameManager").GetComponent<GameManagerReferences>().crosshairHolder;
			Crosshair.SetActive(true);
			GameObject LocalHealthBar = GameObject.Find ("GameManager").GetComponent<GameManagerReferences>().LocalHealthBar;
			LocalHealthBar.SetActive (true);
			GameObject JetpackBar = GameObject.Find ("GameManager").GetComponent<GameManagerReferences>().EnergyBarHolder;
			JetpackBar.SetActive (true);
			GameObject AmmoObject = GameObject.Find ("GameManager").GetComponent<GameManagerReferences>().AmmoObject;
			AmmoObject.SetActive (true);
			GetComponent<PlayerShoot> ().enabled = true;
			GetComponent<SphereCollider> ().enabled = true;
			GetComponent<Rigidbody> ().detectCollisions = true;

		}
		//gameObject.layer = LayerMask.NameToLayer (PlayerLayerName);
		Renderer[]renderers = GetComponentsInChildren<Renderer> ();
		
		foreach (Renderer rens in renderers) {
			
			rens.enabled = true;
		}

		if (isLocalPlayer) {
			respawnButton.SetActive(false);
		}
		
		PlayerScript.isDead = false;
	}

	void CommenceRespawn(){

		CmdRespawnOnServer ();
	}

	[Command]
	void CmdRespawnOnServer(){

		PlayerScript.ResetHealth ();
	}
}
