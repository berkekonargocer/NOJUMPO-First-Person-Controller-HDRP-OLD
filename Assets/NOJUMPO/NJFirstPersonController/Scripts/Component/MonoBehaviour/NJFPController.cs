using System;
using NOJUMPO.Utils;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;
using NOJUMPO.InputSystem;
using Cinemachine;

namespace NOJUMPO.FirstPersonController
{
    [RequireComponent(typeof(CapsuleCollider)), RequireComponent(typeof(Rigidbody)), AddComponentMenu("Nojumpo First Person Controller")]
    public class NJFPController : MonoBehaviour
    {

        public NJInputReaderSO nJInputReader;
        public Transform playerVirtualCameraTransform;

        float cameraSensitivity = 10.0f;
        float cameraAcceleration = 5.0f;

        public int verticalLookLimit = 80;

        #region Variables

        #region Movement Settings

        public bool PlayerCanMove = true;
        public bool WalkByDefault = true;
        public float WalkSpeed = 4f;
        public KeyCode SprintKey = KeyCode.LeftShift;
        public float SprintSpeed = 8f;
        public float JumpPower = 5f;
        public bool CanJump = true;
        public bool CanHoldJump;
        public bool UseStamina = true;
        public float StaminaDepletionSpeed = 5f;
        public float MaxStamina = 50;
        public float MovementSpeed;
        public float Stamina;
        float _walkSpeed;
        float _sprintSpeed;
        float _jumpPower;
        bool _didJump;

        [Serializable]
        public class CrouchModifiers
        {
            public bool useCrouch = true;
            public bool toggleCrouch;
            public KeyCode crouchKey = KeyCode.LeftControl;
            public float crouchWalkSpeedMultiplier = 0.5f;
            public float crouchJumpPowerMultiplier;
            public bool crouchOverride;
            internal float colliderHeight;
        }

        public CrouchModifiers CharacterCrouchModifiers { get; } = new CrouchModifiers();

        [Serializable]
        public class FOV_Kick
        {
            public bool useFOVKick;
            public float FOVKickAmount = 4;
            public float changeTime = 0.1f;
            public AnimationCurve KickCurve = new AnimationCurve();
            public float fovStart;
        }

        public FOV_Kick FOVKick { get; } = new FOV_Kick();

        [Serializable]
        public class AdvancedSettings
        {
            public float gravityMultiplier = 1.0f;
            public PhysicMaterial zeroFrictionMaterial;
            public PhysicMaterial highFrictionMaterial;
            public float _maxSlopeAngle = 70;
            public float maxStepHeight = 0.2f;
            internal bool stairMiniHop;
            public RaycastHit surfaceAngleCheck;
            public float lastKnownSlopeAngle;
        }

        public AdvancedSettings NJFPAdvancedSettings { get; } = new AdvancedSettings();
        CapsuleCollider _capsuleCollider;

        public bool IsGrounded { get; private set; }
        Vector2 inputXY;
        public bool IsCrouching { get; private set; }
        bool _isSprinting;

        Rigidbody _characterRigidbody;

        #endregion

        #region Headbobbing Settings

        public bool UseHeadbob = true;
        public Transform Head;
        public bool SnapHeadjointToCapsul = true;
        public float HeadbobFrequency = 1.5f;
        public float HeadbobSwayAngle = 5f;
        public float HeadbobHeight = 3f;
        public float HeadbobSideMovement = 5f;
        public float JumpLandIntensity = 3f;
        Vector3 _originalLocalPosition;
        float _nextStepTime = 0.5f;
        float _headbobCycle;
        float _headbobFade;
        float _springPosition;
        float _springVelocity;
        float _springElastic = 1.1f;
        float _springDampen = 0.8f;
        float _springVelocityThreshold = 0.05f;
        float _springPositionThreshold = 0.05f;
        Vector3 _previousPosition;
        Vector3 _previousVelocity = Vector3.zero;
        Vector3 _miscRefVel;
        bool _previousGrounded;
        AudioSource _audioSource;

        #endregion

        #region Audio Settings

        public float Volume = 5f;
        public AudioClip jumpSound;
        public AudioClip landSound;
        public List<AudioClip> footStepSounds;
        public enum FootstepSoundMode { Static, Dynamic }
        [FormerlySerializedAs("fsmode")]
        public FootstepSoundMode FootstepSMode;

        [Serializable]
        public class DynamicFootStep
        {
            public enum DynamicFootstepMaterialMode
            {
                PHYSICS_MATERIAL,
                MATERIAL
            }

            public DynamicFootstepMaterialMode FootstepMaterialMode;
            public List<PhysicMaterial> woodPhysMat;
            public List<PhysicMaterial> metalAndGlassPhysMat;
            public List<PhysicMaterial> grassPhysMat;
            public List<PhysicMaterial> dirtAndGravelPhysMat;
            public List<PhysicMaterial> rockAndConcretePhysMat;
            public List<PhysicMaterial> mudPhysMat;
            public List<PhysicMaterial> customPhysMat;

            public List<Material> woodMat;
            public List<Material> metalAndGlassMat;
            public List<Material> grassMat;
            public List<Material> dirtAndGravelMat;
            public List<Material> rockAndConcreteMat;
            public List<Material> mudMat;
            public List<Material> customMat;
            public List<AudioClip> currentClipSet;

            public List<AudioClip> woodClipSet;
            public List<AudioClip> metalAndGlassClipSet;
            public List<AudioClip> grassClipSet;
            public List<AudioClip> dirtAndGravelClipSet;
            public List<AudioClip> rockAndConcreteClipSet;
            public List<AudioClip> mudClipSet;
            public List<AudioClip> customClipSet;
        }

        public DynamicFootStep dynamicFootstep = new DynamicFootStep();

        #endregion

        #endregion

        void Awake() {
            #region Look Settings - Awake

            NJUtils.SetCursorState(false);

            #endregion

            #region Movement Settings - Awake

            _walkSpeed = WalkSpeed;
            _sprintSpeed = SprintSpeed;
            _jumpPower = JumpPower;
            _capsuleCollider = GetComponent<CapsuleCollider>();
            IsGrounded = true;
            IsCrouching = false;
            _characterRigidbody = GetComponent<Rigidbody>();
            CharacterCrouchModifiers.colliderHeight = _capsuleCollider.height;

            #endregion
        }

        void Start() {
            #region Movement Settings - Start

            Stamina = MaxStamina;
            NJFPAdvancedSettings.zeroFrictionMaterial = new PhysicMaterial("Zero_Friction");
            NJFPAdvancedSettings.zeroFrictionMaterial.dynamicFriction = 0;
            NJFPAdvancedSettings.zeroFrictionMaterial.staticFriction = 0;
            NJFPAdvancedSettings.zeroFrictionMaterial.frictionCombine = PhysicMaterialCombine.Minimum;
            NJFPAdvancedSettings.zeroFrictionMaterial.bounceCombine = PhysicMaterialCombine.Minimum;
            NJFPAdvancedSettings.highFrictionMaterial = new PhysicMaterial("Max_Friction");
            NJFPAdvancedSettings.highFrictionMaterial.dynamicFriction = 1;
            NJFPAdvancedSettings.highFrictionMaterial.staticFriction = 1;
            NJFPAdvancedSettings.highFrictionMaterial.frictionCombine = PhysicMaterialCombine.Maximum;
            NJFPAdvancedSettings.highFrictionMaterial.bounceCombine = PhysicMaterialCombine.Average;

            #endregion

            #region Headbobbing Settings - Start

            _originalLocalPosition = SnapHeadjointToCapsul ? new Vector3(Head.localPosition.x, (_capsuleCollider.height / 2) * Head.localScale.y, Head.localPosition.z) : Head.localPosition;

            if (GetComponent<AudioSource>() == null) { gameObject.AddComponent<AudioSource>(); }

            _previousPosition = _characterRigidbody.position;
            _audioSource = GetComponent<AudioSource>();

            #endregion
        }

        void Update() {
            #region Input Settings - Update

            _didJump = CanHoldJump ? Input.GetButton("Jump") : Input.GetButtonDown("Jump");

            if (CharacterCrouchModifiers.useCrouch)
            {
                if (!CharacterCrouchModifiers.toggleCrouch) { IsCrouching = CharacterCrouchModifiers.crouchOverride || Input.GetKey(CharacterCrouchModifiers.crouchKey); }
                else
                {
                    if (Input.GetKeyDown(CharacterCrouchModifiers.crouchKey)) { IsCrouching = !IsCrouching || CharacterCrouchModifiers.crouchOverride; }
                }
            }

            #endregion
        }

        float rotationAmount;
        float verticalRotationAmount;
        void Look() {
            Vector2 mouseDelta = nJInputReader.MouseDelta;
            transform.rotation *= Quaternion.Euler(0, mouseDelta.x * nJInputReader.HorizontalLookSpeed * Time.fixedDeltaTime, 0);
            //mouseDelta *= cameraSensitivity * Time.fixedDeltaTime;
            //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, mouseDelta.x, 0), cameraAcceleration * Time.fixedDeltaTime);

            //mouseDelta.y = Mathf.Clamp(mouseDelta.y, -verticalLookLimit, verticalLookLimit);
            //playerVirtualCameraTransform.localRotation = Quaternion.Lerp(playerVirtualCameraTransform.localRotation, Quaternion.Euler(-mouseDelta.y, 0, 0), cameraAcceleration * Time.fixedDeltaTime);



            // Accumulate rotation over time
            //rotationAmount += mouseDelta.x * cameraSensitivity * Time.fixedDeltaTime;
            // Apply rotation with acceleration
            //Quaternion targetRotation = Quaternion.Euler(0, rotationAmount, 0);
            //transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, cameraAcceleration * Time.fixedDeltaTime);


            // Clamp vertical rotation
            // Accumulate vertical rotation over time
            //verticalRotationAmount += mouseDelta.y * cameraSensitivity * Time.fixedDeltaTime;
            //verticalRotationAmount = Mathf.Clamp(verticalRotationAmount, -verticalLookLimit, verticalLookLimit);
            // Apply vertical rotation with acceleration
            //Quaternion targetVerticalRotation = Quaternion.Euler(-verticalRotationAmount, 0, 0);
            //playerVirtualCameraTransform.localRotation = Quaternion.Lerp(playerVirtualCameraTransform.localRotation, targetVerticalRotation, cameraAcceleration * Time.fixedDeltaTime);

        }

        void FixedUpdate() {
            #region Movement Settings - FixedUpdate
            Look();
            //transform.rotation *= Quaternion.Euler(new Vector3(0, nJInputReader.MouseDelta.x * nJInputReader.HorizontalLookSpeed * Time.fixedDeltaTime, 0));

            if (UseStamina)
            {
                _isSprinting = Input.GetKey(SprintKey) && !IsCrouching && Stamina > 0 && (Mathf.Abs(_characterRigidbody.velocity.x) > 0.01f);

                if (_isSprinting)
                {
                    Stamina -= (StaminaDepletionSpeed * 2) * Time.deltaTime;
                }
                else if ((!Input.GetKey(SprintKey) || Mathf.Abs(_characterRigidbody.velocity.x) < 0.01f || IsCrouching) && Stamina < MaxStamina)
                {
                    Stamina += StaminaDepletionSpeed * Time.deltaTime;
                }

                Stamina = Mathf.Clamp(Stamina, 0, MaxStamina);
            }
            else { _isSprinting = Input.GetKey(SprintKey); }

            Vector3 movementVector = Vector3.zero;
            MovementSpeed = WalkByDefault ? IsCrouching ? _walkSpeed : (_isSprinting ? _sprintSpeed : _walkSpeed) : (_isSprinting ? _walkSpeed : _sprintSpeed);

            if (IsGrounded || _characterRigidbody.velocity.y < 0.1)
            {
                RaycastHit[] hits = Physics.SphereCastAll(transform.position - new Vector3(0, ((_capsuleCollider.height / 2) * transform.localScale.y) - 0.01f, 0), _capsuleCollider.radius, Vector3.down, 0, Physics.AllLayers);
                float nearest = float.PositiveInfinity;
                IsGrounded = false;

                for (int i = 0; i < hits.Length; i++)
                {
                    if (hits[i].distance < nearest && hits[i].collider != _capsuleCollider)
                    {
                        IsGrounded = true;
                        NJFPAdvancedSettings.stairMiniHop = false;
                        nearest = hits[i].distance;
                    }
                }
            }

            if (NJFPAdvancedSettings._maxSlopeAngle > 0 && Physics.Raycast(transform.position - new Vector3(0, ((_capsuleCollider.height / 2) * transform.localScale.y) - _capsuleCollider.radius, 0), new Vector3(movementVector.x, -1.5f, movementVector.z), out NJFPAdvancedSettings.surfaceAngleCheck, 1.5f))
            {
                movementVector = (transform.forward * inputXY.y * MovementSpeed + transform.right * inputXY.x * _walkSpeed) * SlopeCheck();
                if (SlopeCheck() <= 0) { _didJump = false; }
            }
            else
            {
                movementVector = transform.forward * inputXY.y * MovementSpeed + transform.right * inputXY.x * _walkSpeed;
            }

            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            inputXY = new Vector2(horizontalInput, verticalInput);

            if (inputXY.magnitude > 1) { inputXY.Normalize(); }

            float velocityY = _characterRigidbody.velocity.y;

            if (!CanJump) _didJump = false;

            if (IsGrounded && _didJump && _jumpPower > 0)
            {
                velocityY += _jumpPower;
                IsGrounded = false;
                _didJump = false;
            }

            if (PlayerCanMove)
            {
                _characterRigidbody.velocity = movementVector + (Vector3.up * velocityY);
            }
            else { _characterRigidbody.velocity = Vector3.zero; }

            if (movementVector.magnitude > 0 || !IsGrounded)
            {
                _capsuleCollider.sharedMaterial = NJFPAdvancedSettings.zeroFrictionMaterial;
            }
            else { _capsuleCollider.sharedMaterial = NJFPAdvancedSettings.highFrictionMaterial; }

            _characterRigidbody.AddForce(Physics.gravity * (NJFPAdvancedSettings.gravityMultiplier - 1));

            if (CharacterCrouchModifiers.useCrouch)
            {
                if (IsCrouching)
                {
                    _capsuleCollider.height = Mathf.MoveTowards(_capsuleCollider.height, CharacterCrouchModifiers.colliderHeight / 1.5f, 5 * Time.deltaTime);
                    _walkSpeed = WalkSpeed * CharacterCrouchModifiers.crouchWalkSpeedMultiplier;
                    _jumpPower = JumpPower * CharacterCrouchModifiers.crouchJumpPowerMultiplier;
                }
                else
                {
                    _capsuleCollider.height = Mathf.MoveTowards(_capsuleCollider.height, CharacterCrouchModifiers.colliderHeight, 5 * Time.deltaTime);
                    _walkSpeed = WalkSpeed;
                    _sprintSpeed = SprintSpeed;
                    _jumpPower = JumpPower;
                }
            }

            #endregion

            #region Headbobbing Settings - FixedUpdate

            float yPos = 0;
            float xPos = 0;
            float zTilt = 0;
            float xTilt = 0;
            float bobSwayFactor = 0;
            float bobFactor = 0;
            float strideLangthen = 0;
            float flatVel = 0;

            //calculate Headbob freq
            if (UseHeadbob || FootstepSMode == FootstepSoundMode.Dynamic)
            {
                Vector3 vel = (_characterRigidbody.position - _previousPosition) / Time.deltaTime;
                Vector3 velChange = vel - _previousVelocity;
                _previousPosition = _characterRigidbody.position;
                _previousVelocity = vel;
                _springVelocity -= velChange.y;
                _springVelocity -= _springPosition * _springElastic;
                _springVelocity *= _springDampen;
                _springPosition += _springVelocity * Time.deltaTime;
                _springPosition = Mathf.Clamp(_springPosition, -0.3f, 0.3f);

                if (Mathf.Abs(_springVelocity) < _springVelocityThreshold && Mathf.Abs(_springPosition) < _springPositionThreshold)
                {
                    _springPosition = 0;
                    _springVelocity = 0;
                }

                flatVel = new Vector3(vel.x, 0.0f, vel.z).magnitude;
                strideLangthen = 1 + (flatVel * ((HeadbobFrequency * 2) / 10));
                _headbobCycle += (flatVel / strideLangthen) * (Time.deltaTime / HeadbobFrequency);
                bobFactor = Mathf.Sin(_headbobCycle * Mathf.PI * 2);
                bobSwayFactor = Mathf.Sin(Mathf.PI * (2 * _headbobCycle + 0.5f));
                bobFactor = 1 - (bobFactor * 0.5f + 1);
                bobFactor *= bobFactor;

                yPos = 0;
                xPos = 0;
                zTilt = 0;

                if (JumpLandIntensity > 0 && !NJFPAdvancedSettings.stairMiniHop) { xTilt = -_springPosition * (JumpLandIntensity * 5.5f); }
                else if (!NJFPAdvancedSettings.stairMiniHop) { xTilt = -_springPosition; }

                if (IsGrounded)
                {
                    if (new Vector3(vel.x, 0.0f, vel.z).magnitude < 0.1f) { _headbobFade = Mathf.MoveTowards(_headbobFade, 0.0f, 0.5f); }
                    else { _headbobFade = Mathf.MoveTowards(_headbobFade, 1.0f, Time.deltaTime); }

                    float speedHeightFactor = 1 + (flatVel * 0.3f);
                    xPos = -(HeadbobSideMovement / 10) * _headbobFade * bobSwayFactor;
                    yPos = _springPosition * (JumpLandIntensity / 10) + bobFactor * (HeadbobHeight / 10) * _headbobFade * speedHeightFactor;
                    zTilt = bobSwayFactor * (HeadbobSwayAngle / 10) * _headbobFade;
                }
            }

            //apply Headbob position
            if (UseHeadbob)
            {
                if (_characterRigidbody.velocity.magnitude > 0.1f)
                {
                    Head.localPosition = Vector3.MoveTowards(Head.localPosition, SnapHeadjointToCapsul ? new Vector3(_originalLocalPosition.x, _capsuleCollider.height / 2 * Head.localScale.y, _originalLocalPosition.z) + new Vector3(xPos, yPos, 0) : _originalLocalPosition + new Vector3(xPos, yPos, 0), 0.5f);
                }
                else
                {
                    Head.localPosition = Vector3.SmoothDamp(Head.localPosition, SnapHeadjointToCapsul ? new Vector3(_originalLocalPosition.x, _capsuleCollider.height / 2 * Head.localScale.y, _originalLocalPosition.z) + new Vector3(xPos, yPos, 0) : _originalLocalPosition + new Vector3(xPos, yPos, 0), ref _miscRefVel, 0.15f);
                }

                Head.localRotation = Quaternion.Euler(xTilt, 0, zTilt);
            }

            #endregion

            #region Dynamic Footsteps

            if (FootstepSMode == FootstepSoundMode.Dynamic)
            {
                if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit))
                {
                    if (dynamicFootstep.FootstepMaterialMode == DynamicFootStep.DynamicFootstepMaterialMode.PHYSICS_MATERIAL)
                    {
                        dynamicFootstep.currentClipSet = dynamicFootstep.woodPhysMat.Any() && dynamicFootstep.woodPhysMat.Contains(hit.collider.sharedMaterial) && dynamicFootstep.woodClipSet.Any() ? dynamicFootstep.woodClipSet
                            : dynamicFootstep.grassPhysMat.Any() && dynamicFootstep.grassPhysMat.Contains(hit.collider.sharedMaterial) && dynamicFootstep.grassClipSet.Any() ? dynamicFootstep.grassClipSet
                            : dynamicFootstep.metalAndGlassPhysMat.Any() && dynamicFootstep.metalAndGlassPhysMat.Contains(hit.collider.sharedMaterial) && dynamicFootstep.metalAndGlassClipSet.Any() ? dynamicFootstep.metalAndGlassClipSet
                            : dynamicFootstep.rockAndConcretePhysMat.Any() && dynamicFootstep.rockAndConcretePhysMat.Contains(hit.collider.sharedMaterial) && dynamicFootstep.rockAndConcreteClipSet.Any() ? dynamicFootstep.rockAndConcreteClipSet
                            : dynamicFootstep.dirtAndGravelPhysMat.Any() && dynamicFootstep.dirtAndGravelPhysMat.Contains(hit.collider.sharedMaterial) && dynamicFootstep.dirtAndGravelClipSet.Any() ? dynamicFootstep.dirtAndGravelClipSet
                            : dynamicFootstep.mudPhysMat.Any() && dynamicFootstep.mudPhysMat.Contains(hit.collider.sharedMaterial) && dynamicFootstep.mudClipSet.Any() ? dynamicFootstep.mudClipSet
                            : dynamicFootstep.customPhysMat.Any() && dynamicFootstep.customPhysMat.Contains(hit.collider.sharedMaterial) && dynamicFootstep.customClipSet.Any() ? dynamicFootstep.customClipSet
                            : footStepSounds;
                    }
                    else if (hit.collider.GetComponent<MeshRenderer>())
                    {
                        dynamicFootstep.currentClipSet = dynamicFootstep.woodMat.Any() && dynamicFootstep.woodMat.Contains(hit.collider.GetComponent<MeshRenderer>().sharedMaterial) && dynamicFootstep.woodClipSet.Any() ? dynamicFootstep.woodClipSet
                            : dynamicFootstep.grassMat.Any() && dynamicFootstep.grassMat.Contains(hit.collider.GetComponent<MeshRenderer>().sharedMaterial) && dynamicFootstep.grassClipSet.Any() ? dynamicFootstep.grassClipSet
                            : dynamicFootstep.metalAndGlassMat.Any() && dynamicFootstep.metalAndGlassMat.Contains(hit.collider.GetComponent<MeshRenderer>().sharedMaterial) && dynamicFootstep.metalAndGlassClipSet.Any() ? dynamicFootstep.metalAndGlassClipSet
                            : dynamicFootstep.rockAndConcreteMat.Any() && dynamicFootstep.rockAndConcreteMat.Contains(hit.collider.GetComponent<MeshRenderer>().sharedMaterial) && dynamicFootstep.rockAndConcreteClipSet.Any() ? dynamicFootstep.rockAndConcreteClipSet
                            : dynamicFootstep.dirtAndGravelMat.Any() && dynamicFootstep.dirtAndGravelMat.Contains(hit.collider.GetComponent<MeshRenderer>().sharedMaterial) && dynamicFootstep.dirtAndGravelClipSet.Any() ? dynamicFootstep.dirtAndGravelClipSet
                            : dynamicFootstep.mudMat.Any() && dynamicFootstep.mudMat.Contains(hit.collider.GetComponent<MeshRenderer>().sharedMaterial) && dynamicFootstep.mudClipSet.Any() ? dynamicFootstep.mudClipSet
                            : dynamicFootstep.customMat.Any() && dynamicFootstep.customMat.Contains(hit.collider.GetComponent<MeshRenderer>().sharedMaterial) && dynamicFootstep.customClipSet.Any() ? dynamicFootstep.customClipSet
                            : footStepSounds.Any() ? footStepSounds : null; // If material is unknown, fall back
                    }

                    if (IsGrounded)
                    {
                        if (!_previousGrounded)
                        {
                            if (dynamicFootstep.currentClipSet.Any()) { _audioSource.PlayOneShot(dynamicFootstep.currentClipSet[Random.Range(0, dynamicFootstep.currentClipSet.Count)], Volume / 10); }

                            _nextStepTime = _headbobCycle + 0.5f;
                        }
                        else
                        {
                            if (_headbobCycle > _nextStepTime)
                            {
                                _nextStepTime = _headbobCycle + 0.5f;

                                if (dynamicFootstep.currentClipSet.Any()) { _audioSource.PlayOneShot(dynamicFootstep.currentClipSet[Random.Range(0, dynamicFootstep.currentClipSet.Count)], Volume / 10); }
                            }
                        }

                        _previousGrounded = true;
                    }
                    else
                    {
                        if (_previousGrounded)
                        {
                            if (dynamicFootstep.currentClipSet.Any()) { _audioSource.PlayOneShot(dynamicFootstep.currentClipSet[Random.Range(0, dynamicFootstep.currentClipSet.Count)], Volume / 10); }
                        }

                        _previousGrounded = false;
                    }
                }
                else
                {
                    dynamicFootstep.currentClipSet = footStepSounds;

                    if (IsGrounded)
                    {
                        if (!_previousGrounded)
                        {
                            if (landSound) { _audioSource.PlayOneShot(landSound, Volume / 10); }

                            _nextStepTime = _headbobCycle + 0.5f;
                        }
                        else
                        {
                            if (_headbobCycle > _nextStepTime)
                            {
                                _nextStepTime = _headbobCycle + 0.5f;
                                int n = Random.Range(0, footStepSounds.Count);

                                if (footStepSounds.Any()) { _audioSource.PlayOneShot(footStepSounds[n], Volume / 10); }

                                footStepSounds[n] = footStepSounds[0];
                            }
                        }

                        _previousGrounded = true;
                    }
                    else
                    {
                        if (_previousGrounded)
                        {
                            if (jumpSound) { _audioSource.PlayOneShot(jumpSound, Volume / 10); }
                        }

                        _previousGrounded = false;
                    }
                }
            }
            else
            {
                if (IsGrounded)
                {
                    if (!_previousGrounded)
                    {
                        if (landSound) { _audioSource.PlayOneShot(landSound, Volume / 10); }

                        _nextStepTime = _headbobCycle + 0.5f;
                    }
                    else
                    {
                        if (_headbobCycle > _nextStepTime)
                        {
                            _nextStepTime = _headbobCycle + 0.5f;
                            int n = Random.Range(0, footStepSounds.Count);

                            if (footStepSounds.Any() && footStepSounds[n] != null) { _audioSource.PlayOneShot(footStepSounds[n], Volume / 10); }
                        }
                    }

                    _previousGrounded = true;
                }
                else
                {
                    if (_previousGrounded)
                    {
                        if (jumpSound) { _audioSource.PlayOneShot(jumpSound, Volume / 10); }
                    }

                    _previousGrounded = false;
                }
            }

            #endregion
        }

        float SlopeCheck() {
            NJFPAdvancedSettings.lastKnownSlopeAngle = Vector3.Angle(NJFPAdvancedSettings.surfaceAngleCheck.normal, Vector3.up);
            return new AnimationCurve(new Keyframe(-90.0f, 1.0f), new Keyframe(0.0f, 1.0f), new Keyframe(NJFPAdvancedSettings._maxSlopeAngle, 0.0f), new Keyframe(90, 0.0f))
            {
                preWrapMode = WrapMode.Clamp,
                postWrapMode = WrapMode.ClampForever
            }.Evaluate(NJFPAdvancedSettings.lastKnownSlopeAngle);
        }
    }
}