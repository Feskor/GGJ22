using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED

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
		private float standardRot;

		[Header("Movement Settings")]
		public bool analogMovement;

#if !UNITY_IOS || !UNITY_ANDROID
		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;
#endif

		private void Start()
		{
			standardRot = transform.rotation.eulerAngles.y;
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

		// old input sys if we do decide to have it (most likely wont)...
		private void Update()
		{
			MoveInput(new Vector2(moveHorizontal, 1));

			float MinY = standardRot - maxAngle;
			float MaxY = standardRot + maxAngle;
			Vector3 euler = transform.localEulerAngles;
			euler.y = Mathf.Clamp(euler.y, MinY, MaxY);
			transform.localEulerAngles = euler;
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