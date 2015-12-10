using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class CameraFacingBillboard : NetworkBehaviour
{
	public Camera m_Camera;
	
	void Update()
	{
		if (!isLocalPlayer) {
			transform.LookAt (transform.position + m_Camera.transform.rotation * Vector3.forward, m_Camera.transform.rotation * Vector3.up);
		}
	}
}