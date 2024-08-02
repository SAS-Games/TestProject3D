using System;
using UnityEngine;
using Cinemachine;
using System.Collections;

public class CameraManager : MonoBehaviour, IObjectSpawnedListener
{
	[SerializeField] private CameraLookControls m_CameraLookControls;
	[SerializeField] private Camera m_MainCamera;
	[SerializeField] private CinemachineFreeLook m_FreeLookVCam;
	private bool _isRMBPressed;

	[SerializeField, Range(.5f, 3f)]
	private float _speedMultiplier = 1f; //TODO: make this modifiable in the game settings											

	//[Header("Listening on channels")]
	//[Tooltip("The CameraManager listens to this event, fired by objects in any scene, to adapt camera position")]
	//[SerializeField] private TransformEventChannelSO _frameObjectChannel = default;


	private bool _cameraMovementLock = false;

	private void Awake()
	{
		m_CameraLookControls = new CameraLookControls();
	}

	private void SetupProtagonistVirtualCamera()
	{
		
	}

	private void OnEnable()
	{
		m_CameraLookControls.Mouse.Enable();

		m_CameraLookControls.Mouse.RotateCamera.started += context => OnCameraMove(context.ReadValue<Vector2>(), context.control.device.name == "Mouse");
		m_CameraLookControls.Mouse.RotateCamera.performed += context => OnCameraMove(context.ReadValue<Vector2>(), context.control.device.name == "Mouse");
		m_CameraLookControls.Mouse.RotateCamera.canceled += context => OnCameraMove(context.ReadValue<Vector2>(), context.control.device.name == "Mouse");

		m_CameraLookControls.Mouse.MouseControlCamera.performed += _ => OnEnableMouseControlCamera();
		m_CameraLookControls.Mouse.MouseControlCamera.canceled += _ => OnDisableMouseControlCamera();

		//if (_frameObjectChannel != null)
		//	_frameObjectChannel.OnEventRaised += OnFrameObjectEvent;
	}

	private void OnDisable()
	{
		m_CameraLookControls.Mouse.RotateCamera.started -= context => OnCameraMove(context.ReadValue<Vector2>(), context.control.device.name == "Mouse");
		m_CameraLookControls.Mouse.RotateCamera.performed -= context => OnCameraMove(context.ReadValue<Vector2>(), context.control.device.name == "Mouse");
		m_CameraLookControls.Mouse.RotateCamera.canceled -= context => OnCameraMove(context.ReadValue<Vector2>(), context.control.device.name == "Mouse");

		m_CameraLookControls.Mouse.MouseControlCamera.performed -= _ => OnEnableMouseControlCamera();
		m_CameraLookControls.Mouse.MouseControlCamera.canceled -= _ => OnDisableMouseControlCamera();
	}

	private void OnEnableMouseControlCamera()
	{
		_isRMBPressed = true;

		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

		StartCoroutine(DisableMouseControlForFrame());
	}

	IEnumerator DisableMouseControlForFrame()
	{
		_cameraMovementLock = true;
		yield return new WaitForEndOfFrame();
		_cameraMovementLock = false;
	}

	private void OnDisableMouseControlCamera()
	{
		_isRMBPressed = false;

		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;

		// when mouse control is disabled, the input needs to be cleared
		// or the last frame's input will 'stick' until the action is invoked again
		m_FreeLookVCam.m_XAxis.m_InputAxisValue = 0;
		m_FreeLookVCam.m_YAxis.m_InputAxisValue = 0;
	}

	private void OnCameraMove(Vector2 cameraMovement, bool isDeviceMouse)
	{
		if (_cameraMovementLock)
			return;

		if (isDeviceMouse && !_isRMBPressed)
			return;

		m_FreeLookVCam.m_XAxis.m_InputAxisValue = cameraMovement.x * Time.deltaTime * _speedMultiplier;
		m_FreeLookVCam.m_YAxis.m_InputAxisValue = cameraMovement.y * Time.deltaTime * _speedMultiplier;
	}


    void IObjectSpawnedListener.OnSpawn(GameObject gameObject)
    {
		var target = gameObject.transform;
		m_FreeLookVCam.Follow = target;
		m_FreeLookVCam.LookAt = target;
		m_FreeLookVCam.OnTargetObjectWarped(target, target.position - m_FreeLookVCam.transform.position - Vector3.forward);

	}

    void IObjectSpawnedListener.OnDespawn(GameObject gameObject)
    {
		m_FreeLookVCam.Follow = null;
		m_FreeLookVCam.LookAt = null;
	}
}
