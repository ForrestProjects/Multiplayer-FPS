using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour {

	[SerializeField]
	private int maxHealth = 100;

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

	void Awake(){
		SetDefaults ();
		MM.LockCursor ();
		HP = GameObject.Find ("GameManager").GetComponent<GameManagerReferences> ().HP;
	}

	
	void Update(){
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

	public void TakeDamage(int _amount){

		currentHealth -= _amount;
		Debug.Log (transform.name + " now has " + currentHealth + " health.");
	}

	public void SetDefaults(){

		currentHealth = maxHealth;
	}

	public void ResetHealth(){

		currentHealth = maxHealth;
	}
}
