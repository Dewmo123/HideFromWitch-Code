using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets._00.Work.YHB.Scripts.Core
{
	[CreateAssetMenu(fileName = "InputSO", menuName = "SO/Input", order = 0)]
    public class InputSO : ScriptableObject, InputControlls.IPlayerActions,InputControlls.IUIActions
	{
		private InputControlls _controlls;

		private void OnEnable()
		{
			if (_controlls == null)
			{
				_controlls = new InputControlls();
				_controlls.Player.SetCallbacks(this);
				_controlls.UI.SetCallbacks(this);
			}
			_controlls.Player.Enable();
			_controlls.UI.Enable();
		}
		public void SetEnable(bool val)
		{
			if (val)
				_controlls.Player.Enable();
			else
				_controlls.Player.Disable();
		}

		private void OnDisable()
		{
			_controlls?.Player.Disable();
			_controlls.UI.Disable();
		}

		#region Player Input

		[SerializeField] private LayerMask whatIsGround;
		private Vector2 _screenPosition;

		public bool GetWorldPosition(out Vector3 worldPosition)
		{
			Camera mainCam = Camera.main;
			Debug.Assert(mainCam != null, "No main camera in this scene.");
			Ray cameraRay = mainCam.ScreenPointToRay(_screenPosition);
			if (Physics.Raycast(cameraRay, out RaycastHit hit, mainCam.farClipPlane, whatIsGround))
			{
				worldPosition = hit.point;
				return true;
			}

			worldPosition = Vector3.zero;
			return false;
		}

		public event Action<bool> OnAttackKStatusChangeEvent;
		public event Action<bool> OnJumpStatusChangeEvent;
		public event Action<bool> OnSprintStatusChangeEvent;
		public event Action<bool> OnTabPressedEvent;
		public event Action<bool> OnMouseLockEvent;

		public event Action<Vector2> OnMoveKeyPressedEvent;
		public event Action<Vector2> OnMoveValueChangedEvent;
		public event Action<Vector2> OnLookChangedEvent;

		public event Action<float> OnZoomOutDeltaEvent;
		public event Action OnMoveLockEvent;
		public event Action OnScanEvent;
		public event Action OnEscapeEvent;
		public event Action OnEnterEvent;

        public Vector2 MovementDirection { get; private set; }

		public void OnAttack(InputAction.CallbackContext context)
		{
			if (context.performed)
				OnAttackKStatusChangeEvent?.Invoke(true);

			if (context.canceled)
				OnAttackKStatusChangeEvent?.Invoke(false);
		}

		public void OnJump(InputAction.CallbackContext context)
		{
			if (context.performed)
				OnJumpStatusChangeEvent?.Invoke(true);

			if (context.canceled)
				OnJumpStatusChangeEvent?.Invoke(false);
		}

		public void OnLook(InputAction.CallbackContext context)
		{
			Vector2 lookInputVector = context.ReadValue<Vector2>();
			if (context.performed)
				OnLookChangedEvent?.Invoke(lookInputVector);
		}

		public void OnMove(InputAction.CallbackContext context)
		{
			Vector2 movementInputVector = context.ReadValue<Vector2>();

			if (context.performed)
				OnMoveKeyPressedEvent?.Invoke(movementInputVector);

			MovementDirection = movementInputVector;
			OnMoveValueChangedEvent?.Invoke(MovementDirection);
		}

		public void OnSprint(InputAction.CallbackContext context)
		{
			if (context.performed)
				OnSprintStatusChangeEvent?.Invoke(true);

			if (context.canceled)
				OnSprintStatusChangeEvent?.Invoke(false);
		}

		public void OnZoomOut(InputAction.CallbackContext context)
		{
			OnZoomOutDeltaEvent?.Invoke(-context.ReadValue<Vector2>().y);
		}

		public void OnMouseLock(InputAction.CallbackContext context)
		{
			if (context.performed)
				OnMouseLockEvent?.Invoke(true);
			if(context.canceled)
				OnMouseLockEvent?.Invoke(false);
		}
        public void OnScan(InputAction.CallbackContext context)
        {
            if(context.performed)
				OnScanEvent?.Invoke();
        }

        public void OnLock(InputAction.CallbackContext context)
		{
			if (context.performed)
				OnMoveLockEvent?.Invoke();
		}

		public void OnPointer(InputAction.CallbackContext context)
		{
			_screenPosition = context.ReadValue<Vector2>();
		}
        public void OnTab(InputAction.CallbackContext context)
        {
			if (context.performed)
				OnTabPressedEvent?.Invoke(true);
			if (context.canceled)
				OnTabPressedEvent?.Invoke(false);
        }

        #endregion
        #region UIInput
        public void OnEscape(InputAction.CallbackContext context)
        {
            if (context.performed)
                OnEscapeEvent?.Invoke();
        }

        public void OnEnter(InputAction.CallbackContext context)
        {
            if (context.performed)
                OnEnterEvent?.Invoke();
        }
        public void OnNavigate(InputAction.CallbackContext context)
        {
        }

        public void OnSubmit(InputAction.CallbackContext context)
        {
        }

        public void OnCancel(InputAction.CallbackContext context)
        {
        }

        public void OnPoint(InputAction.CallbackContext context)
        {
        }

        public void OnClick(InputAction.CallbackContext context)
        {
        }

        public void OnRightClick(InputAction.CallbackContext context)
        {
        }

        public void OnMiddleClick(InputAction.CallbackContext context)
        {
        }

        public void OnScrollWheel(InputAction.CallbackContext context)
        {
        }

        public void OnTrackedDevicePosition(InputAction.CallbackContext context)
        {
        }

        public void OnTrackedDeviceOrientation(InputAction.CallbackContext context)
        {
        }
        #endregion

    }
}
