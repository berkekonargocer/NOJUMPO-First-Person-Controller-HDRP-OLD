using Cinemachine;
using NOJUMPO.InputSystem;
using UnityEngine;

namespace NOJUMPO.Extensions
{
    public class CinemachinePOVExtension : CinemachineExtension
    {
        // -------------------------------- FIELDS ---------------------------------
        [SerializeField] NJInputReaderSO njInputReader;

        //[SerializeField] float maxSpeed;
        //[SerializeField] bool acceleratedRotation;

        //[SerializeField] float accelerationTime = 1.0f;
        //[SerializeField] float decelerationTime = 1.5f;

        [SerializeField] float maxRotation = 70.0f;

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
