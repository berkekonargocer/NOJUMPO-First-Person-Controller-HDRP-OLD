using Cinemachine;
using NOJUMPO.InputSystem;
using NOJUMPO.Utils;
using UnityEngine;

namespace NOJUMPO
{
    public class CinemachinePOVExtension : CinemachineExtension
    {
        // -------------------------------- FIELDS ---------------------------------
        [SerializeField] NJInputReaderSO njInputReader;

        [SerializeField] Transform playerTransform;
        [SerializeField] float maxRotation = 70.0f;

        [SerializeField] float mouseDeltaX;
        float _verticalLookSpeed;
        float _horizontalLookSpeed;
        
        Vector3 _startingRotation;

        // ------------------------- UNITY BUILT-IN METHODS ------------------------
        protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime) {
            if (vcam.Follow)
            {
                if (stage == CinemachineCore.Stage.Aim)
                {
                    Vector2 deltaInput = njInputReader.MouseDelta;
                    mouseDeltaX = deltaInput.x;
                    _startingRotation.x += deltaInput.x * _horizontalLookSpeed * Time.deltaTime;
                    _startingRotation.y += deltaInput.y * _verticalLookSpeed * Time.deltaTime;
                    _startingRotation.y = Mathf.Clamp(_startingRotation.y, -maxRotation, maxRotation);
                    state.RawOrientation = Quaternion.Euler(-_startingRotation.y, _startingRotation.x, 0f);
                }
            }
        }


        // ------------------------- CUSTOM PRIVATE METHODS ------------------------
        protected override void Awake() {
            _startingRotation = transform.localRotation.eulerAngles;
            _verticalLookSpeed = njInputReader.VerticalLookSpeed;
            _horizontalLookSpeed = njInputReader.HorizontalLookSpeed;
            base.Awake();
        }

        // ------------------------- CUSTOM PUBLIC METHODS -------------------------

    }
}
