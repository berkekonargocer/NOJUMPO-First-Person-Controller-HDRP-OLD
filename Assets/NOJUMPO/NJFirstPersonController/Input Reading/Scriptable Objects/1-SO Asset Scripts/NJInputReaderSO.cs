using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace NOJUMPO.InputSystem
{
    [CreateAssetMenu(fileName = "NewInputReader", menuName = "Nojumpo/Scriptable Objects/Game Input/New Input Reader")]
    public class NJInputReaderSO : ScriptableObject, NJInputActions.IPlayerActions, NJInputActions.IUIActions, NJInputActions.IInspectActions
    {

#if UNITY_EDITOR

        [TextArea]
        [SerializeField] string _developerDescription = "CREATE ONE FOR EACH PLAYER (e.g. IF THERE ARE TWO PLAYERS CREATE TWO AND BIND THEIR OWN)";

#endif
        // -------------------------------- FIELDS ---------------------------------

        [field: SerializeField] public float HorizontalLookSpeed { get; private set; }
        [field: SerializeField] public float VerticalLookSpeed { get; private set; }

        NJInputActions _njInputActions;

        public Vector2 MoveInput { get; private set; }
        public Vector2 MouseDelta { get; private set; }
        public Vector2 InspectMouseDelta { get; private set; }
        
        public event Action<Vector2> OnMovementInputPressed;
        
        public event Action OnInteractionInputPressed;       
        public event Action OnInteractionInputReleased;
        
        public event Action OnJumpInputPressed;  
        public event Action OnJumpInputReleased;


        // ------------------------- UNITY BUILT-IN METHODS ------------------------
        void OnEnable() {
            if (_njInputActions == null)
            {
                _njInputActions = new NJInputActions();

                _njInputActions.Player.SetCallbacks(this);
                _njInputActions.UI.SetCallbacks(this);
                _njInputActions.Inspect.SetCallbacks(this);

                SetPlayerInput();
            }
        }

        
        // ------------------------- CUSTOM PUBLIC METHODS -------------------------
        public void OnMove(InputAction.CallbackContext context) {
            MoveInput = context.ReadValue<Vector2>();
            OnMovementInputPressed?.Invoke(MoveInput);
        }

        public void OnLook(InputAction.CallbackContext context) {
            MouseDelta = context.ReadValue<Vector2>();
        }
        
        public void OnInspectLook(InputAction.CallbackContext context) {
            InspectMouseDelta = context.ReadValue<Vector2>();
        }

        public void OnInteractButton(InputAction.CallbackContext context) {
            if (context.phase == InputActionPhase.Performed)
            {
                OnInteractionInputPressed?.Invoke();
            }

            if (context.phase == InputActionPhase.Canceled)
            {
                OnInteractionInputReleased?.Invoke();
            }
        }

        public void OnResumeGame(InputAction.CallbackContext context) {
            // throw new System.NotImplementedException();
        }

        public void SetPlayerInput() {
            _njInputActions.Inspect.Disable();
            _njInputActions.UI.Disable();
            _njInputActions.Player.Enable();
        }

        public void SetUIInput() {
            _njInputActions.Player.Disable();
            _njInputActions.Inspect.Disable();
            _njInputActions.UI.Enable();
        }

        public void SetInspectionInput() {
            _njInputActions.Player.Disable();
            _njInputActions.UI.Disable();
            _njInputActions.Inspect.Enable();
        }
    }
}
