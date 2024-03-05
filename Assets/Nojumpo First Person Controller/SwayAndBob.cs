using Nojumpo.FirstPersonController;
using UnityEngine;

namespace Nojumpo
{
    [DisallowMultipleComponent]
    public class SwayAndBob : MonoBehaviour
    {
        // -------------------------------- FIELDS ---------------------------------
        [Header("EXTERNAL REFERENCES")]
        [SerializeField] NJFPController playerController;
        [SerializeField] Rigidbody playerRigidbody;

        [Header("INPUTS")]
        Vector2 _walkInput;
        Vector2 _lookInput;

        [Header("SWAY")]
        [SerializeField] float step = 0.01f;
        [SerializeField] float maxStepDistance = 0.06f;
        Vector3 _swayPosition;

        [Header("SWAY ROTATION")]
        [SerializeField] float rotationStep = 4f;
        [SerializeField] float maxRotationStep = 5f;
        Vector3 _swayEulerRotation;

        [Header("BOB OFFSET")]
        [SerializeField] Vector3 travelLimit = Vector3.one * 0.025f;
        [SerializeField] Vector3 bobLimit = Vector3.one * 0.01f;
        float _speedCurve;
        Vector3 _bobPosition;
        float _curveSin { get { return Mathf.Sin(_speedCurve); } }
        float _curveCos { get { return Mathf.Cos(_speedCurve); } }

        [Header("BOB ROTATION")]
        [SerializeField] Vector3 multiplier;
        Vector3 _bobEulerRotation;

        [Header("BOB AND SWAY APPLY SETTINGS")]
        const float _smooth = 10.0f;
        const float _smoothRotation = 12f;


        // ------------------------- UNITY BUILT-IN METHODS ------------------------
        void Update() {
            GetInput();

            Sway();
            SwayRotation();
            BobOffset();
            BobRotation();

            CompositePositionRotation();
        }

        
        // ------------------------- CUSTOM PRIVATE METHODS ------------------------
        void GetInput() {
            _walkInput.x = Input.GetAxis("Horizontal");
            _walkInput.y = Input.GetAxis("Vertical");
            _walkInput = _walkInput.normalized;

            _lookInput.x = Input.GetAxis("Mouse X");
            _lookInput.y = Input.GetAxis("Mouse Y");
        }

        void Sway() {
            Vector3 invertLook = _lookInput * -step;
            invertLook.x = Mathf.Clamp(invertLook.x, -maxStepDistance, maxStepDistance);
            invertLook.y = Mathf.Clamp(invertLook.y, -maxStepDistance, maxStepDistance);

            _swayPosition = invertLook;
        }

        void SwayRotation() {
            Vector2 invertLook = _lookInput * -rotationStep;
            invertLook.x = Mathf.Clamp(invertLook.x, -maxRotationStep, maxRotationStep);
            invertLook.y = Mathf.Clamp(invertLook.y, -maxRotationStep, maxRotationStep);

            _swayEulerRotation = new Vector3(invertLook.y, invertLook.x, invertLook.x);
        }

        void BobOffset() {
            _speedCurve += Time.deltaTime * (playerController.IsGrounded ? playerRigidbody.velocity.magnitude : 1f) + 0.01f;

            _bobPosition.x = _curveCos * bobLimit.x * (playerController.IsGrounded ? 1 : 0) - _walkInput.x * travelLimit.x;
            _bobPosition.y = _curveSin * bobLimit.y - playerRigidbody.velocity.y * travelLimit.y;
            _bobPosition.z = -(_walkInput.y * travelLimit.z);
        }

        void BobRotation() {
            _bobEulerRotation.x = _walkInput != Vector2.zero ? multiplier.x * Mathf.Sin(2 * _speedCurve) : multiplier.x * Mathf.Sin(2 * _speedCurve / 2);
            _bobEulerRotation.y = _walkInput != Vector2.zero ? multiplier.y * multiplier.y * _curveCos : 0;
            _bobEulerRotation.z = _walkInput != Vector2.zero ? multiplier.z * multiplier.z * _curveCos * _walkInput.x : 0;
        }

        void CompositePositionRotation() {
            Transform objectTransform = transform;
            
            objectTransform .localPosition = Vector3.Lerp(objectTransform.localPosition, _swayPosition + _bobPosition, Time.deltaTime * _smooth);
            transform.localRotation = Quaternion.Slerp(objectTransform.localRotation, Quaternion.Euler(_swayEulerRotation) * Quaternion.Euler(_bobEulerRotation), Time.deltaTime * _smoothRotation);
        }
    }
}