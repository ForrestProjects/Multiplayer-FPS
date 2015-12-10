using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[RequireComponent(typeof(Player))]
public class PlayerSetup : NetworkBehaviour {

	[SerializeField]
	Behaviour[] componentsToDisable;

	[SerializeField]
	string remoteLayerName = "RemotePlayer";

	Camera sceneCamera;


	void Start(){
		if (!isLocalPlayer) {
			DisableComponents ();
			AssignRemoteLayer ();

		} else {
			GameObject Crosshair = GameObject.Find ("GameManager").GetComponent<GameManagerReferences>().crosshairHolder;
			Crosshair.SetActive(true);
			GameObject LocalHealthBar = GameObject.Find ("GameManager").GetComponent<GameManagerReferences>().LocalHealthBar;
			LocalHealthBar.SetActive (true);
			GameObject AmmoObject = GameObject.Find ("GameManager").GetComponent<GameManagerReferences>().AmmoObject;
			AmmoObject.SetActive (true);
			//GameObject ChangeNameHolder = GameObject.Find ("GameManager").GetComponent<GameManagerReferences>().ChangeNameHolder;
			//ChangeNameHolder.SetActive (true);
			sceneCamera = Camera.main;
			if(sceneCamera != null){
				sceneCamera.gameObject.SetActive (false);
			}
		}
	}

	public override void OnStartClient(){

		base.OnStartClient ();

		string _netID = GetComponent<NetworkIdentity> ().netId.ToString ();
		Player _player = GetComponent<Player> ();

		GameManager.RegisterPlayer (_netID, _player);
	}
	void AssignRemoteLayer(){

		gameObject.layer = LayerMask.NameToLayer (remoteLayerName);
	}

	void DisableComponents(){
		for (int i = 0; i < componentsToDisable.Length; i++) {
			componentsToDisable [i].enabled = false;
		}
	}

	public void OnDisable(){
		if (sceneCamera != null) {
			sceneCamera.gameObject.SetActive(true);
			//GetComponentInChildren<AudioListener>().enabled = false;
		}
		if (isLocalPlayer) {
			//GameObject Crosshair = GameObject.Find ("GameManager").GetComponent<GameManagerReferences> ().crosshairHolder;
			//Crosshair.SetActive (false);
			GameObject LocalHealthBar = GameObject.Find ("GameManager").GetComponent<GameManagerReferences> ().LocalHealthBar;
			LocalHealthBar.SetActive (false);
			GameObject JetpackBar = GameObject.Find ("GameManager").GetComponent<GameManagerReferences> ().EnergyBarHolder;
			JetpackBar.SetActive (false);
			GameObject AmmoObject = GameObject.Find ("GameManager").GetComponent<GameManagerReferences> ().AmmoObject;
			AmmoObject.SetActive (false);
			GameManager.DeRegisterPlayer (transform.name);
		}
	}
}
