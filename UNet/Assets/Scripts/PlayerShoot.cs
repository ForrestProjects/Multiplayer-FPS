using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;
[RequireComponent(typeof(AudioSource))]
public class PlayerShoot : NetworkBehaviour {

	private const string PLAYER_TAG = "Player";

	public PlayerWeaponScript weapon;
	private float shotTime;
	

	[SerializeField]
	private Text AimName;

	[SerializeField]
	private AudioClip AReload;

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
	private Text KillText;
	private Image Hitmarker;
	private GameObject AmmoObject;
	private Text AmmoCount;
	private bool isReloading;

	private int currentAmmo;

	public override void OnStartLocalPlayer(){

		if (cam == null) {
			Debug.LogError ("No camera referenced!");
			this.enabled = false;
		}
		shotTime = weapon.shotTime;
		Debug.Log (shotTime);
	}

	private void AimingName(){
		if (isLocalPlayer) {
			RaycastHit PlayerScan;
			if (Physics.Raycast (cam.transform.position, cam.transform.forward, out PlayerScan, 10000)) {
				if (PlayerScan.collider.tag == PLAYER_TAG) {
					AimName.text = PlayerScan.transform.name;
				
				
				} else {
					AimName.text = " ";
				}
			} 

		}
	}
	void Start(){
		if (isLocalPlayer) {
			AimName = GameObject.Find ("/GUI/AimPlayer").GetComponent<Text> ();
			Hitmarker = GameObject.Find ("/GUI/HitMarker").GetComponent<Image> ();
			KillText = GameObject.Find ("/GUI/KillText").GetComponent<Text> ();
			AmmoCount = GameObject.Find ("/GUI/AmmoObject/AmmoPicture/AmmoCount").GetComponent<Text> ();
			currentAmmo = weapon.Ammo;
		}
	}

	void Update(){
		if (isLocalPlayer) {
			AimingName ();

			AmmoCount.text = currentAmmo.ToString ();
			if (Input.GetButtonDown ("Reload")) {
				if (currentAmmo != weapon.Ammo && isReloading == false) {
					isReloading = true;
					//CmdReload ();
					PlayerAnimator.SetBool ("Reload", true);
					Vector3 _p = this.transform.position;
					CmdReloadSound (_p);
					ASource.PlayOneShot (AReload);
					currentAmmo = weapon.Ammo;
					isReloading = false;
				}
			}
			if (Input.GetButtonDown ("Fire1")) {
				Shoot ();
			}
			if (PlayerAnimator.GetCurrentAnimatorStateInfo (0).IsName ("GunShot")) {
				PlayerAnimator.SetBool ("isShooting", false);
		
			}
			if (PlayerAnimator.GetCurrentAnimatorStateInfo (0).IsName ("Reload")) {
				PlayerAnimator.SetBool ("Reload", false);
			
			}

			if (shotTime != weapon.shotTime) {
				shotTime += Time.deltaTime;
				if (shotTime > weapon.shotTime) {
					shotTime = weapon.shotTime;
				}
			}
		}
	}

	//[Command]
	//void CmdReload(){
		//PlayerAnimator.SetBool("Reload", true);
		//RpcReload ();
	//}

	//[ClientRpc]
	//void RpcReload(){
		//if (isLocalPlayer) {
		//	return;
		//}
		//PlayerAnimator.SetBool ("Reload", true);
	//}

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

	[Command]
	void CmdReloadSound(Vector3 position){

		RpcReloadSound (position);

	}
	[ClientRpc]
	void RpcReloadSound(Vector3 position){
		if (isLocalPlayer) {
			return;
		}
		AudioSource.PlayClipAtPoint (AReload, position);

	}

	IEnumerator CallADelay(float secs){
		yield return new WaitForSeconds (secs);
		Hitmarker.enabled = false;


	}
	IEnumerator CallTextDelay(float secs){
		yield return new WaitForSeconds (secs);
		Text ChangeText = GameObject.Find ("/GUI/KillText").GetComponent<Text> ();
		ChangeText.text = " ";

		
	}


	void UpdateKillText(string _KillText){
		Text ChangeText = GameObject.Find ("/GUI/KillText").GetComponent<Text> ();
		ChangeText.text = _KillText;
		StartCoroutine(CallTextDelay(3f));

	}

	[Client]
	void Shoot(){


		if (currentAmmo > 0) {
			if (shotTime >= weapon.shotTime) {
				Debug.Log (currentAmmo);
				currentAmmo = currentAmmo - 1;
				if (isLocalPlayer) {

					PlayerAnimator.SetBool ("isShooting", true);
					MuzzleFlash.GetComponent<ParticleSystem> ().Play ();
					Vector3 MyPosition = this.transform.position;
					CmdMuzzle (MyPosition);
					ASource.Play ();
					Debug.Log (ASource.isPlaying);


				}
				string x = this.GetComponent<NetworkIdentity> ().name;
				RaycastHit hit;
				if (Physics.Raycast (cam.transform.position, cam.transform.forward, out hit, weapon.range)) {
					if (hit.collider.tag == PLAYER_TAG) {
						float y = 10;
						Hitmarker.enabled = true;
						CmdPlayerShot (hit.collider.name, weapon.damage, this.gameObject);
						StartCoroutine (CallADelay (0.2f));


					}
				}
				shotTime = 0;
			} else {
// no ammo
			}
	
		} else {
			//

		}
	}



	[ClientRpc]
	void RpcPlayFatality(){
		this.ASource.PlayOneShot (Fatality);

	}
	
	[ClientRpc]
	void RpcChangeText(string SendKillText){

		UpdateKillText(SendKillText);
	}
	[Command]
	void CmdPlayerShot(string PlayerID, int _damage, GameObject x){

		string Shooter = x.name;

		Player _player = GameManager.GetPlayer (PlayerID);

			bool DidDie = _player.TakeDamage (_damage, Shooter);
			if (DidDie) {
				string SendKillText = Shooter + " Killed " + _player.name;
				UpdateKillText (SendKillText);
				RpcChangeText (SendKillText);
				RpcPlayFatality ();
				x.GetComponent<Player> ().RegisterKill ();

			}

	}
}
