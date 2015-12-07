using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
[RequireComponent(typeof(ConfigurableJoint))]
public class PlayerController : MonoBehaviour {

	[SerializeField]
	private float speed = 5f;
	[SerializeField]
	private float looksensitivity = 3f;
	private PlayerMotor motor;
	private ConfigurableJoint joint;


	[SerializeField]
	private float thrusterForce = 1000f;

	[Header("Spring Settings")]
	[SerializeField]
	private JointDriveMode jointMode = JointDriveMode.Position;
	[SerializeField]
	private float jointSpring = 20f;
	[SerializeField]
	private float jointMaxForce = 40f;

	void Start(){
		motor = GetComponent<PlayerMotor> ();
		joint = GetComponent<ConfigurableJoint> ();

		SetJointSettings (jointSpring);
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
			_thrusterforce = Vector3.up * thrusterForce; 
			SetJointSettings (0f);
		} else {
			SetJointSettings(jointSpring);
		}

		//Apply Thruster Force
		motor.applythruster (_thrusterforce);
	}

	private void SetJointSettings(float _jointSpring){
		joint.yDrive = new JointDrive{
			mode = jointMode, 
			positionSpring = _jointSpring, 
			maximumForce = jointMaxForce
		};
	}
}
