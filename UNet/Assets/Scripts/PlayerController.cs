using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

[RequireComponent(typeof(PlayerMotor))]
[RequireComponent(typeof(ConfigurableJoint))]
public class PlayerController : NetworkBehaviour {

	[SerializeField]
	private float speed = 5f;
	[SerializeField]
	private float looksensitivity = 3f;
	private PlayerMotor motor;
	private ConfigurableJoint joint;
	private Image EnergyBar;

	[SerializeField]
	private GameObject[] Jetpack;

	[SerializeField]
	private float thrusterForce = 1000f;

	[Header("Spring Settings")]
	[SerializeField]
	private JointDriveMode jointMode = JointDriveMode.Position;
	[SerializeField]
	private float jointSpring = 20f;
	[SerializeField]
	private float jointMaxForce = 40f;
	[SerializeField]
	private float ThrusterTime = 3f;

	void Start(){
		motor = GetComponent<PlayerMotor> ();
		joint = GetComponent<ConfigurableJoint> ();

		SetJointSettings (jointSpring);

		GameObject Energy = GameObject.Find ("GameManager").GetComponent<GameManagerReferences> ().EnergyBarHolder;
		Energy.SetActive (true);
		EnergyBar = Energy.GetComponentInChildren<Image> ();
	}

	void Update(){
		//Calculate movement velocity as 3d vector
		float xMove = Input.GetAxisRaw ("Horizontal");
		float zMove = Input.GetAxisRaw ("Vertical");

		Vector3 movHorizontal = transform.right * xMove;
		Vector3 movVertical = transform.forward * zMove;



		//Final movement vector
		Vector3 velocity = (movHorizontal + movVertical).normalized * speed;

		//Apply Movement
		motor.move (velocity);

		//Calculate roatation as a 3d vector: TURNING AROUND.
		float yRot = Input.GetAxisRaw ("Mouse X");

		Vector3 rotation = new Vector3 (0f, yRot, 0f) * looksensitivity;

		//Apply rotation
		motor.rotate (rotation);

		//Calculate camera rotation as a 3d vector: Aim up and down
		float xRot = Input.GetAxisRaw ("Mouse Y");
		
		float camerarotationX = xRot * looksensitivity;
		
		//Apply camera rotation
		motor.rotatecamera (camerarotationX);

		//Calc thruster force based on input
		Vector3 _thrusterforce = Vector3.zero;
		//Apply thruster force
		if (Input.GetButton ("Jump")) {
			if (ThrusterTime > 0f) {
				_thrusterforce = Vector3.up * thrusterForce; 
				SetJointSettings (0f);
				CmdJetpackCommand(true);
				motor.applythruster (_thrusterforce);
				ThrusterTime -= 1f * Time.fixedDeltaTime;
			}else{
				SetJointSettings(jointSpring);
				_thrusterforce = Vector3.zero;
				CmdJetpackCommand(false);
			}

		} else {
			SetJointSettings(jointSpring);
			CmdJetpackCommand(false);
		}

		//Apply Thruster Force
		EnergyBar.fillAmount = ThrusterTime / 3;

		if (!Input.GetButton ("Jump")) {
			if(ThrusterTime != 3f){
				if(ThrusterTime > 3f){
					ThrusterTime = 3f;
				}else{
				ThrusterTime += 0.3f * Time.fixedDeltaTime;
				}
			}
		}
		Debug.Log ("Thruster Time = " + ThrusterTime);
	}

	[Command]
	void CmdJetpackCommand(bool t){
		RpcJetpackCommand (t);
		foreach (GameObject i in Jetpack) {
			i.SetActive (t);
		}
	}

	[ClientRpc]
	void RpcJetpackCommand(bool t){
		foreach (GameObject i in Jetpack) {
			i.SetActive (t);
		}
	}
	
	private void SetJointSettings(float _jointSpring){
		joint.yDrive = new JointDrive{
			mode = jointMode, 
			positionSpring = _jointSpring, 
			maximumForce = jointMaxForce
		};
	}
}
