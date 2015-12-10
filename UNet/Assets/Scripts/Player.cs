using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour {

	[SerializeField]
	private int maxHealth = 100;

	[SyncVar]
	private int Kills;

	[SyncVar]
	private int currentHealth;

	private bool shouldDie = false;
	public bool isDead = false;
	public MouseManager MM;
	public delegate void DieDelegate();
	public event DieDelegate EventDie;
	public HealthBar HP;

	public delegate void RespawnDelegate();
	public event RespawnDelegate EventRespawn;

	
	public int ReturnHealth(){
		int y = currentHealth;
		return y;
	}
	void Awake(){
		SetDefaults ();
		MM.LockCursor ();
		HP = GameObject.Find ("GameManager").GetComponent<GameManagerReferences> ().HP;
	}

	
	void Update(){
		Debug.Log ("Is dead value : " + isDead);
		CheckCondition ();
		if (isLocalPlayer) {
			HP.SyncHP (currentHealth);
		}
	}

	void CheckCondition(){
		if (currentHealth <= 0 && !shouldDie && !isDead) {
			shouldDie = true;

		}
		if (currentHealth <= 0 && shouldDie) {
			if(EventDie != null){

				EventDie();
				MM.UnlockCursor ();
			}

			shouldDie = false;
		}
		if (currentHealth> 0 && isDead) {
			if(EventRespawn != null)
			{
				EventRespawn();
				MM.LockCursor ();
			}
		}
	}

	public bool TakeDamage(int _amount, string Shooter){

		currentHealth -= _amount;
		if(currentHealth <= 0){
			currentHealth = 0;
			Debug.Log (this.name + " was murdererd by " + Shooter);
			return true;
		}
		Debug.Log (transform.name + " now has " + currentHealth + " health.");
		return false;
	}

	public void SetDefaults(){

		currentHealth = maxHealth;
	}

	public void ResetHealth(){

		currentHealth = maxHealth;
	}

	public void RegisterKill(){
		Kills++;
	}

	public int ReturnKills(){
		return Kills;
	}
}
