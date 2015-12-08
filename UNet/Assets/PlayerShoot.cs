using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
[RequireComponent(typeof(AudioSource))]
public class PlayerShoot : NetworkBehaviour {

	private const string PLAYER_TAG = "Player";

	public PlayerWeaponScript weapon;
	private float shotTime;

	[SerializeField]
	private GameObject Bullet_Emitter;

	[SerializeField]
	private AudioClip AFire;

	[SerializeField]
	private AudioClip Fatality;

	[SerializeField]
	private AudioSource ASource;

	[SerializeField]
	private GameObject MuzzleFlash;

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


	[Command]
	void CmdMuzzle(Vector3 _position){

		RpcMuzzle (_position);
	}


	[ClientRpc]
	void RpcMuzzle(Vector3 position){
		if (isLocalPlayer) {
			return;
		}
		MuzzleFlash.GetComponent<ParticleSystem> ().Play ();
		AudioSource.PlayClipAtPoint (AFire, position );

	}

	[Client]
	void Shoot(){

		Debug.Log ("shotTime = " + shotTime + " weaponSTime = " + weapon.shotTime);
		if (shotTime >= weapon.shotTime) {

			if(isLocalPlayer){

				PlayerAnimator.SetBool("isShooting", true);
				MuzzleFlash.GetComponent<ParticleSystem> ().Play ();
				Vector3 MyPosition = this.transform.position;
				CmdMuzzle (MyPosition);
				ASource.Play ();
				Debug.Log (ASource.isPlaying);


			}
			string x = this.GetComponent<NetworkIdentity> ().name;
			RaycastHit hit;
			if (Physics.Raycast (cam.transform.position, cam.transform.forward, out hit, weapon.range, mask)) {
				if (hit.collider.tag == PLAYER_TAG) {

					CmdPlayerShot (hit.collider.name, weapon.damage, this.gameObject);
				}
			}
			shotTime = 0;
		} else {
			Debug.Log ("Can't shoot yet");
		}

	}



	[ClientRpc]
	void RpcPlayFatality(){
		this.ASource.PlayOneShot (Fatality);

	}
	

	[Command]
	void CmdPlayerShot(string PlayerID, int _damage, GameObject x){

		string Shooter = x.name;

		Player _player = GameManager.GetPlayer (PlayerID);
		bool DidDie = _player.TakeDamage (_damage, Shooter);
		if (DidDie){
			RpcPlayFatality();
			x.GetComponent<Player>().RegisterKill();
			Debug.Log ("This many kills! : " + x.GetComponent<Player>().ReturnKills());
		}

	}
}
