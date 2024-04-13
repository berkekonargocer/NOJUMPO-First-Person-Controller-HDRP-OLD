using NOJUMPO.FirstPersonController;
using NOJUMPO.InputSystem;
using UnityEngine;

namespace NOJUMPO
{
    [DisallowMultipleComponent]
    public class SwayAndBob : MonoBehaviour
    {
        // -------------------------------- FIELDS ---------------------------------
        [Header("EXTERNAL REFERENCES")]
        [SerializeField] NJFPController playerController;
        [SerializeField] NJInputReaderSO njInputReader;
        [SerializeField] Rigidbody playerRigidbody;

        [Header("SWAY")]
        [SerializeField] float step = 0.02f;
        [SerializeField] float maxStepDistance = 0.08f;
        Vector3 _swayPosition;

        [Header("SWAY ROTATION")]
        [SerializeField] float rotationStep = 3f;
        [SerializeField] float maxRotationStep = 6f;
        Vector3 _swayEulerRotation;

        [Header("BOB OFFSET")]
        [SerializeField] Vector3 travelLimit = new Vector3(0.05f, 0.01f, 0.05f);
        [SerializeField] Vector3 bobLimit = Vector3.one * 0.0025f;
        float _speedCurve;
        Vector3 _bobPosition;
        float _curveSin { get { return Mathf.Sin(_speedCurve); } }
        float _curveCos { get { return Mathf.Cos(_speedCurve); } }

        [Header("BOB ROTATION")]
        [SerializeField] Vector3 multiplier = new Vector3(1.0f, 0.05f, 1.0f);
        Vector3 _bobEulerRotation;

        [Header("BOB AND SWAY APPLY SETTINGS")]
        const float _smooth = 5.0f;
        const float _smoothRotation = 6f;


        // ------------------------- UNITY BUILT-IN METHODS ------------------------
        void FixedUpdate() {
            Sway();
            SwayRotation();
            BobOffset();
            BobRotation();

            CompositePositionRotation();
        }


        // ------------------------- CUSTOM PRIVATE METHODS ------------------------
        void Sway() {
            Vector3 invertLook = njInputReader.MouseDelta * -step;
            invertLook.x = Mathf.Clamp(invertLook.x, -maxStepDistance, maxStepDistance);
            invertLook.y = Mathf.Clamp(invertLook.y, -maxStepDistance, maxStepDistance);

            _swayPosition = invertLook;
        }

        void SwayRotation() {
            Vector2 invertLook = njInputReader.MouseDelta * -rotationStep;
            invertLook.x = Mathf.Clamp(invertLook.x, -maxRotationStep, maxRotationStep);
            invertLook.y = Mathf.Clamp(invertLook.y, -maxRotationStep, maxRotationStep);

            _swayEulerRotation = new Vector3(invertLook.y, invertLook.x, invertLook.x);
        }

        void BobOffset() {
            _speedCurve += Time.fixedDeltaTime * (playerController.IsGrounded ? playerRigidbody.velocity.magnitude : 1f) + 0.01f;

            _bobPosition.x = _curveCos * bobLimit.x * (playerController.IsGrounded ? 1 : 0) - njInputReader.MoveInput.x * travelLimit.x;
            _bobPosition.y = _curveSin * bobLimit.y - playerRigidbody.velocity.y * travelLimit.y;
            _bobPosition.z = -(njInputReader.MoveInput.y * travelLimit.z);
        }

        void BobRotation() {
            _bobEulerRotation.x = njInputReader.MoveInput != Vector2.zero ? multiplier.x * Mathf.Sin(2 * _speedCurve) : multiplier.x * Mathf.Sin(2 * _speedCurve / 2);
            _bobEulerRotation.y = njInputReader.MoveInput != Vector2.zero ? multiplier.y * multiplier.y * _curveCos : 0;
            _bobEulerRotation.z = njInputReader.MoveInput != Vector2.zero ? multiplier.z * multiplier.z * _curveCos * njInputReader.MoveInput.x : 0;
        }

        void CompositePositionRotation() {
            Transform objectTransform = transform;

            objectTransform.localPosition = Vector3.Lerp(objectTransform.localPosition, _swayPosition + _bobPosition, Time.fixedDeltaTime * _smooth);
            transform.localRotation = Quaternion.Slerp(objectTransform.localRotation, Quaternion.Euler(_swayEulerRotation) * Quaternion.Euler(_bobEulerRotation), Time.fixedDeltaTime * _smoothRotation);
        }
    }
}