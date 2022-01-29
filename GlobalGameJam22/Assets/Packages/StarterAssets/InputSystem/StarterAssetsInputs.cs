using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;
		private float moveHorizontal;
		public float maxAngle = 45f;
		public float standardRot;

		[Header("Movement Settings")]
		public bool analogMovement;

#if !UNITY_IOS || !UNITY_ANDROID
		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;
#endif

		private void Start()
		{
			standardRot = transform.rotation.y;
		}

		private void FixedUpdate()
		{
			if (Input.GetKey(KeyCode.A))
			{
				moveHorizontal = -1;
			}
			else if (Input.GetKey(KeyCode.D))
			{
				moveHorizontal = 1;
			}
			else
			{
				moveHorizontal = 0;
			}
		}

		
		private void Update()
		{
			MoveInput(new Vector2(moveHorizontal, 1));

			transform.rotation = Quaternion.Euler(transform.rotation.x, standardRot, transform.rotation.x);//new Quaternion(transform.rotation.x, standardRot, transform.rotation.z, transform.rotation.w);
		}

		public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		}

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}

#if !UNITY_IOS || !UNITY_ANDROID

		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}

#endif

	}

}