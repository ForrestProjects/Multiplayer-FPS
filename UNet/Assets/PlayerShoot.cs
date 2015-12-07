using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerShoot : NetworkBehaviour {

	private const string PLAYER_TAG = "Player";

	public PlayerWeaponScript weapon;
	private float shotTime;

	[SerializeField]
	private Animator PlayerAnimator;

	[SerializeField]
	private NetworkAnimator NPlayerAnimator;

	[SerializeField]
	private Camera cam;

	[SerializeField]
	private LayerMask mask;

	public override void OnStartLocalPlayer(){

		if (cam == null) {
			Debug.LogError ("No camera referenced!");
			this.enabled = false;
		}
		shotTime = weapon.shotTime;
		Debug.Log (shotTime);
	}

	void Update(){
		if (Input.GetButtonDown ("Fire1")) {
			Shoot();
		}
		if(PlayerAnimator.GetCurrentAnimatorStateInfo(0).IsName ("GunShot")){
			PlayerAnimator.SetBool("isShooting", false);
		}

		if(shotTime != weapon.shotTime){
			shotTime += Time.deltaTime;
			if(shotTime > weapon.shotTime){
				shotTime = weapon.shotTime;
			}
		}
	}


	[Client]
	void Shoot(){

		Debug.Log ("shotTime = " + shotTime + " weaponSTime = " + weapon.shotTime);
		if (shotTime >= weapon.shotTime) {

			if(isLocalPlayer){
				PlayerAnimator.SetBool("isShooting", true);
			}
			string x = this.GetComponent<NetworkIdentity> ().name;
			RaycastHit hit;
			if (Physics.Raycast (cam.transform.position, cam.transform.forward, out hit, weapon.range, mask)) {
				if (hit.collider.tag == PLAYER_TAG) {

					CmdPlayerShot (hit.collider.name, x, weapon.damage);
				}
			}
			shotTime = 0;
		} else {
			Debug.Log ("Can't shoot yet");
		}

	}




	

	[Command]
	void CmdPlayerShot(string PlayerID, string x, int _damage){

	
		Debug.Log (PlayerID + " has been shot by " + x);

		Player _player = GameManager.GetPlayer (PlayerID);
		_player.TakeDamage (_damage);

	}
}
