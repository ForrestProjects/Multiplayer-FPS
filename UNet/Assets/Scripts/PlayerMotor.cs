using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour {

	[SerializeField]
	private Camera cam;

	private Vector3 velocity = Vector3.zero;
	private Vector3 rotation = Vector3.zero;
	private float camerarotationX = 0f;
	private Vector3 thrusterForce = Vector3.zero;
	private float currentcamerarotationX = 0f;



	[SerializeField]
	private float cameraRotationLimit = 85f;

	private Rigidbody rb;

	void Start()
	{

		rb = GetComponent<Rigidbody> ();
	}
//Gets movement vector
	public void move(Vector3 _velocity)
	{
		velocity = _velocity;

	}

	public void rotate(Vector3 _rotation){
		rotation = _rotation;
	}

	public void rotatecamera(float _camerarotationX){
		camerarotationX = _camerarotationX;
	}

	//get force vector for thruster
	public void applythruster(Vector3 _thrusterforce){
		thrusterForce = _thrusterforce;

	}
	void FixedUpdate()
	{
		PerformMovement ();
		PerformRotation ();
	}

	//Perform movement based on velocity variable
	void PerformMovement(){
		if (velocity != Vector3.zero) {
			rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
		}

		if (thrusterForce != Vector3.zero) {

			rb.AddForce (thrusterForce * Time.fixedDeltaTime, ForceMode.Acceleration );
		}

	}
	//Perform rotation
	void PerformRotation()
	{

		rb.MoveRotation (rb.rotation * Quaternion.Euler (rotation));
		if (cam != null) {
			//set rotation then clamp it
			currentcamerarotationX -= camerarotationX;
			currentcamerarotationX = Mathf.Clamp(currentcamerarotationX, -cameraRotationLimit, cameraRotationLimit);

			//apply rotation to the transform of the camera
			cam.transform.localEulerAngles = new Vector3 (currentcamerarotationX, 0f, 0f);

		}
	}

}
