using UnityEngine;
public class MouseManager : MonoBehaviour { 

	private bool isCursorLocked;

	void Update(){

		CheckForInput ();
		CheckIfCursorShouldBeLocked ();
	}

	void Start(){


	}
	public void LockCursor(){
		isCursorLocked = true;

	}

	public void UnlockCursor(){
		isCursorLocked = false;
	}
	public void ToggleCursorState()
	{
		isCursorLocked = !isCursorLocked;
	}

	void CheckForInput()
	{
		if (Input.GetKeyDown (KeyCode.Escape)) {
			ToggleCursorState ();
		}
	}

	void CheckIfCursorShouldBeLocked()
	{
		if (isCursorLocked) {
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		} else {

			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}

	}
}
