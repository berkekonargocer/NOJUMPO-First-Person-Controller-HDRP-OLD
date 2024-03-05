using UnityEngine;

namespace NOJUMPO
{
    [DisallowMultipleComponent]
    public class NJFPCameraController : MonoBehaviour
    {
        // -------------------------------- FIELDS ---------------------------------

        [SerializeField] Transform _cameraTransform;
        [SerializeField] Transform _handTransform;

        [SerializeField] float cameraSensitivity = 200.0f;
        [SerializeField] float cameraAcceleration = 5.0f;

        [SerializeField] int lookLimitY = 80;

        float _lookInputX, _lookInputY;


        // ------------------------- UNITY BUILT-IN METHODS ------------------------
        void Awake() {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        void OnEnable() {
        }

        void OnDisable() {
        }

        void Start() {
        }

        void Update() {
            GetInputs();

            Look();
        }


        // ------------------------- CUSTOM PUBLIC METHODS -------------------------


        // ------------------------ CUSTOM PROTECTED METHODS -----------------------


        // ------------------------- CUSTOM PRIVATE METHODS ------------------------
        void GetInputs() {
            _lookInputY += Input.GetAxis("Mouse X") * cameraSensitivity * Time.deltaTime;
            _lookInputX -= Input.GetAxis("Mouse Y") * cameraSensitivity * Time.deltaTime;
            _lookInputX = Mathf.Clamp(_lookInputX, -lookLimitY, lookLimitY);
        }

        void Look() {
            _handTransform.rotation = Quaternion.Euler(_lookInputX, _lookInputY, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, _lookInputY, 0), cameraAcceleration * Time.deltaTime);
            _cameraTransform.localRotation = Quaternion.Lerp(_cameraTransform.localRotation, Quaternion.Euler(_lookInputX, 0, 0), cameraAcceleration * Time.deltaTime);
        }
    }
}