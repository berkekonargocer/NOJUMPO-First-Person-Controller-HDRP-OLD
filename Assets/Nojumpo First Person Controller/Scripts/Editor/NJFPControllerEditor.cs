using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

namespace Nojumpo.FirstPersonController
{
    [CustomEditor(typeof(NJFPController))]
    public class NJFPControllerEditor : Editor
    {
        NJFPController _nojumpoFirstPersonController;
        SerializedObject _serializedNojumpoFirstPersonController;
        static bool showCrouchMods;
        static bool showFOVKickSet;
        static bool showAdvanced;
        static bool showStaticFS;
        SerializedProperty staticFS;

        static bool showWoodFS;
        SerializedProperty woodFS;
        SerializedProperty woodMat;
        SerializedProperty woodPhysMat;

        static bool showMetalFS;
        SerializedProperty metalFS;
        SerializedProperty metalAndGlassMat;
        SerializedProperty metalAndGlassPhysMat;

        static bool showGrassFS;
        SerializedProperty grassFS;
        SerializedProperty grassMat;
        SerializedProperty grassPhysMat;

        static bool showDirtFS;
        SerializedProperty dirtFS;
        SerializedProperty dirtAndGravelMat;
        SerializedProperty dirtAndGravelPhysMat;

        static bool showConcreteFS;
        SerializedProperty concreteFS;
        SerializedProperty rockAndConcreteMat;
        SerializedProperty rockAndConcretePhysMat;

        static bool showMudFS;
        SerializedProperty mudFS;
        SerializedProperty mudMat;
        SerializedProperty mudPhysMat;

        static bool showCustomFS;
        SerializedProperty customFS;
        SerializedProperty customMat;
        SerializedProperty customPhysMat;

        readonly string _versionNum = "0.1";

        void OnEnable() {
            _nojumpoFirstPersonController = (NJFPController)target;
            _serializedNojumpoFirstPersonController = new SerializedObject(_nojumpoFirstPersonController);
            staticFS = _serializedNojumpoFirstPersonController.FindProperty("footStepSounds");

            woodFS = _serializedNojumpoFirstPersonController.FindProperty("dynamicFootstep.woodClipSet");
            woodMat = _serializedNojumpoFirstPersonController.FindProperty("dynamicFootstep.woodMat");
            woodPhysMat = _serializedNojumpoFirstPersonController.FindProperty("dynamicFootstep.woodPhysMat");

            metalFS = _serializedNojumpoFirstPersonController.FindProperty("dynamicFootstep.metalAndGlassClipSet");
            metalAndGlassMat = _serializedNojumpoFirstPersonController.FindProperty("dynamicFootstep.metalAndGlassMat");
            metalAndGlassPhysMat = _serializedNojumpoFirstPersonController.FindProperty("dynamicFootstep.metalAndGlassPhysMat");

            grassFS = _serializedNojumpoFirstPersonController.FindProperty("dynamicFootstep.grassClipSet");
            grassMat = _serializedNojumpoFirstPersonController.FindProperty("dynamicFootstep.grassMat");
            grassPhysMat = _serializedNojumpoFirstPersonController.FindProperty("dynamicFootstep.grassPhysMat");

            dirtFS = _serializedNojumpoFirstPersonController.FindProperty("dynamicFootstep.dirtAndGravelClipSet");
            dirtAndGravelMat = _serializedNojumpoFirstPersonController.FindProperty("dynamicFootstep.dirtAndGravelMat");
            dirtAndGravelPhysMat = _serializedNojumpoFirstPersonController.FindProperty("dynamicFootstep.dirtAndGravelPhysMat");

            concreteFS = _serializedNojumpoFirstPersonController.FindProperty("dynamicFootstep.rockAndConcreteClipSet");
            rockAndConcreteMat = _serializedNojumpoFirstPersonController.FindProperty("dynamicFootstep.rockAndConcreteMat");
            rockAndConcretePhysMat = _serializedNojumpoFirstPersonController.FindProperty("dynamicFootstep.rockAndConcretePhysMat");

            mudFS = _serializedNojumpoFirstPersonController.FindProperty("dynamicFootstep.mudClipSet");
            mudMat = _serializedNojumpoFirstPersonController.FindProperty("dynamicFootstep.mudMat");
            mudPhysMat = _serializedNojumpoFirstPersonController.FindProperty("dynamicFootstep.mudPhysMat");

            customFS = _serializedNojumpoFirstPersonController.FindProperty("dynamicFootstep.customClipSet");
            customMat = _serializedNojumpoFirstPersonController.FindProperty("dynamicFootstep.customMat");
            customPhysMat = _serializedNojumpoFirstPersonController.FindProperty("dynamicFootstep.customPhysMat");
        }
        public override void OnInspectorGUI() {
            _serializedNojumpoFirstPersonController.Update();
            EditorGUILayout.Space();

            GUILayout.Label("Nojumpo First Person Controller", new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Bold,
                fontSize = 16
            });
            GUILayout.Label("version: " + _versionNum, new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter
            });
            EditorGUILayout.Space();

        #region Camera Setup

            //EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            //GUILayout.Label("Camera Setup", new GUIStyle(GUI.skin.label)
            //{
            //    alignment = TextAnchor.MiddleCenter,
            //    fontStyle = FontStyle.Bold,
            //    fontSize = 13
            //}, GUILayout.ExpandWidth(true));
            //EditorGUILayout.Space();
            //EditorGUILayout.Space();
            //_nojumpoFirstPersonController.enableCameraMovement = EditorGUILayout.ToggleLeft(new GUIContent("Enable Camera Movement", "Determines whether the player can move camera or not."), _nojumpoFirstPersonController.enableCameraMovement);
            //EditorGUILayout.Space();
            //GUI.enabled = _nojumpoFirstPersonController.enableCameraMovement;
            //_nojumpoFirstPersonController.VerticalRotationRange = EditorGUILayout.Slider(new GUIContent("Vertical Rotation Range", "Determines how much range does the camera have to move vertically."), _nojumpoFirstPersonController.VerticalRotationRange, 90, 180);
            //_nojumpoFirstPersonController.MouseInputInversion = (NJFPController.InvertMouseInput)EditorGUILayout.EnumPopup(new GUIContent("Mouse Input Inversion", "Determines if mouse input should be inverted, and along which axes"), _nojumpoFirstPersonController.MouseInputInversion);
            //_nojumpoFirstPersonController.MouseSensitivityInternal = _nojumpoFirstPersonController.MouseSensitivity = EditorGUILayout.Slider(new GUIContent("Mouse Sensitivity", "Determines how sensitive the mouse is."), _nojumpoFirstPersonController.MouseSensitivity, 1, 15);

            ////_nojumpoFirstPersonController.mouseSensitivity = EditorGUILayout.Slider(new GUIContent("Mouse Sensitivity","Determines how sensitive the mouse is."),t.mouseSensitivity, 1,15);
            //_nojumpoFirstPersonController.FOVToMouseSensitivity = EditorGUILayout.Slider(new GUIContent("FOV to Mouse Sensitivity", "Determines how much the camera's Field Of View will effect the mouse sensitivity. \n\n0 = no effect, 1 = full effect on sensitivity."), _nojumpoFirstPersonController.FOVToMouseSensitivity, 0, 1);
            //_nojumpoFirstPersonController.CameraSmoothing = EditorGUILayout.Slider(new GUIContent("Camera Smoothing", "Determines how smooth the camera movement is."), _nojumpoFirstPersonController.CameraSmoothing, 1, 25);
            //_nojumpoFirstPersonController.playerCamera = (Camera)EditorGUILayout.ObjectField(new GUIContent("Player Camera", "Camera attached to this controller"), _nojumpoFirstPersonController.playerCamera, typeof(Camera), true);

            //if (!_nojumpoFirstPersonController.playerCamera) { EditorGUILayout.HelpBox("A Camera is required for operation.", MessageType.Error); }

            //_nojumpoFirstPersonController.enableCameraShake = EditorGUILayout.ToggleLeft(new GUIContent("Enable Camera Shake?", "Call this Coroutine externally with duration ranging from 0.01 to 1, and a magnitude of 0.01 to 0.5."), _nojumpoFirstPersonController.enableCameraShake);
            //_nojumpoFirstPersonController.lockAndHideCursor = EditorGUILayout.ToggleLeft(new GUIContent("Lock and Hide Cursor", "For debuging or if You don't plan on having a pause menu or quit button."), _nojumpoFirstPersonController.lockAndHideCursor);

            //GUI.enabled = true;
            //EditorGUILayout.Space();

        #endregion

        #region Movement Setup

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Label("Movement Setup", new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Bold,
                fontSize = 13
            }, GUILayout.ExpandWidth(true));
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            _nojumpoFirstPersonController.PlayerCanMove = EditorGUILayout.ToggleLeft(new GUIContent("Enable Player Movement", "Determines if the player is allowed to move."), _nojumpoFirstPersonController.PlayerCanMove);
            GUI.enabled = _nojumpoFirstPersonController.PlayerCanMove;
            _nojumpoFirstPersonController.WalkByDefault = EditorGUILayout.ToggleLeft(new GUIContent("Walk By Default", "Determines if the default mode of movement is 'Walk' or 'Srpint'."), _nojumpoFirstPersonController.WalkByDefault);
            _nojumpoFirstPersonController.WalkSpeed = EditorGUILayout.Slider(new GUIContent("Walk Speed", "Determines how fast the player walks."), _nojumpoFirstPersonController.WalkSpeed, 0.1f, 10);
            _nojumpoFirstPersonController.SprintKey = (KeyCode)EditorGUILayout.EnumPopup(new GUIContent("Sprint Key", "Determines what key needs to be pressed to enter a sprint"), _nojumpoFirstPersonController.SprintKey);
            _nojumpoFirstPersonController.SprintSpeed = EditorGUILayout.Slider(new GUIContent("Sprint Speed", "Determines how fast the player sprints."), _nojumpoFirstPersonController.SprintSpeed, 0.1f, 20);
            _nojumpoFirstPersonController.CanJump = EditorGUILayout.ToggleLeft(new GUIContent("Can Player Jump?", "Determines if the player is allowed to jump."), _nojumpoFirstPersonController.CanJump);
            GUI.enabled = _nojumpoFirstPersonController.PlayerCanMove && _nojumpoFirstPersonController.CanJump;
            EditorGUI.indentLevel++;
            _nojumpoFirstPersonController.JumpPower = EditorGUILayout.Slider(new GUIContent("Jump Power", "Determines how high the player can jump."), _nojumpoFirstPersonController.JumpPower, 0.1f, 15);
            _nojumpoFirstPersonController.CanHoldJump = EditorGUILayout.ToggleLeft(new GUIContent("Hold Jump", "Determines if the jump button needs to be pressed down to jump, or if the player can hold the jump button to automaticly jump every time the it hits the ground."), _nojumpoFirstPersonController.CanHoldJump);
            EditorGUI.indentLevel--;
            GUI.enabled = _nojumpoFirstPersonController.PlayerCanMove;
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            showCrouchMods = EditorGUILayout.BeginFoldoutHeaderGroup(showCrouchMods, new GUIContent("Crouch Modifiers", "Stat modifiers that will apply when player is crouching."));

            if (showCrouchMods)
            {
                _nojumpoFirstPersonController.CharacterCrouchModifiers.useCrouch = EditorGUILayout.ToggleLeft(new GUIContent("Enable Coruch", "Determines if the player is allowed to crouch."), _nojumpoFirstPersonController.CharacterCrouchModifiers.useCrouch);
                GUI.enabled = _nojumpoFirstPersonController.PlayerCanMove && _nojumpoFirstPersonController.CharacterCrouchModifiers.useCrouch;
                _nojumpoFirstPersonController.CharacterCrouchModifiers.crouchKey = (KeyCode)EditorGUILayout.EnumPopup(new GUIContent("Crouch Key", "Determines what key needs to be pressed to crouch"), _nojumpoFirstPersonController.CharacterCrouchModifiers.crouchKey);
                _nojumpoFirstPersonController.CharacterCrouchModifiers.toggleCrouch = EditorGUILayout.ToggleLeft(new GUIContent("Toggle Crouch?", "Determines if the crouching behaviour is on a toggle or momentary basis."), _nojumpoFirstPersonController.CharacterCrouchModifiers.toggleCrouch);
                _nojumpoFirstPersonController.CharacterCrouchModifiers.crouchWalkSpeedMultiplier = EditorGUILayout.Slider(new GUIContent("Crouch Movement Speed Multiplier", "Determines how fast the player can move while crouching."), _nojumpoFirstPersonController.CharacterCrouchModifiers.crouchWalkSpeedMultiplier, 0.01f, 1.5f);
                _nojumpoFirstPersonController.CharacterCrouchModifiers.crouchJumpPowerMultiplier = EditorGUILayout.Slider(new GUIContent("Crouching Jump Power Mult.", "Determines how much the player's jumping power is increased or reduced while crouching."), _nojumpoFirstPersonController.CharacterCrouchModifiers.crouchJumpPowerMultiplier, 0, 1.5f);
                _nojumpoFirstPersonController.CharacterCrouchModifiers.crouchOverride = EditorGUILayout.ToggleLeft(new GUIContent("Force Crouch Override", "A Toggle that will override the crouch key to force player to crouch."), _nojumpoFirstPersonController.CharacterCrouchModifiers.crouchOverride);
            }

            GUI.enabled = _nojumpoFirstPersonController.PlayerCanMove;
            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space();
            showFOVKickSet = EditorGUILayout.BeginFoldoutHeaderGroup(showFOVKickSet, new GUIContent("FOV Kick Settings", "Settings for FOV Kick"));

            if (showFOVKickSet)
            {
                GUILayout.Label("Under Development", new GUIStyle(GUI.skin.label)
                {
                    alignment = TextAnchor.MiddleCenter,
                    fontStyle = FontStyle.Bold,
                    fontSize = 13
                }, GUILayout.ExpandWidth(true));
                GUI.enabled = true;
                _nojumpoFirstPersonController.FOVKick.useFOVKick = EditorGUILayout.ToggleLeft(new GUIContent("Enable FOV Kick", "Determines if the camera's Field of View will kick when entering a sprint."), _nojumpoFirstPersonController.FOVKick.useFOVKick);

                //GUI.enabled = _nojumpoFirstPersonController.playerCanMove&&t.fOVKick.useFOVKick;
                _nojumpoFirstPersonController.FOVKick.FOVKickAmount = EditorGUILayout.Slider(new GUIContent("Kick Amount", "Determines how much the camera's FOV will kick upon entering a sprint."), _nojumpoFirstPersonController.FOVKick.FOVKickAmount, 0, 10);
                _nojumpoFirstPersonController.FOVKick.changeTime = EditorGUILayout.Slider(new GUIContent("Change Time", "Determines the duration of the FOV kick"), _nojumpoFirstPersonController.FOVKick.changeTime, 0.01f, 5);
                _nojumpoFirstPersonController.FOVKick.KickCurve = EditorGUILayout.CurveField(new GUIContent("Kick Curve", ""), _nojumpoFirstPersonController.FOVKick.KickCurve);
            }

            GUI.enabled = _nojumpoFirstPersonController.PlayerCanMove;
            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.Space();
            showAdvanced = EditorGUILayout.BeginFoldoutHeaderGroup(showAdvanced, new GUIContent("Advanced Movement", "Advanced movenet settings"));

            if (showAdvanced)
            {
                _nojumpoFirstPersonController.UseStamina = EditorGUILayout.ToggleLeft(new GUIContent("Enable Stamina", "Determines if spriting will be limited by stamina."), _nojumpoFirstPersonController.UseStamina);
                GUI.enabled = _nojumpoFirstPersonController.PlayerCanMove && _nojumpoFirstPersonController.UseStamina;
                EditorGUI.indentLevel++;
                _nojumpoFirstPersonController.MaxStamina = EditorGUILayout.Slider(new GUIContent("Stamina Level", "Determines how much stamina the player has. if left 0, stamina will not be used."), _nojumpoFirstPersonController.MaxStamina, 0, 100);
                _nojumpoFirstPersonController.StaminaDepletionSpeed = EditorGUILayout.Slider(new GUIContent("Stamina Depletion Speed", "Determines how quickly the player's stamina depletes."), _nojumpoFirstPersonController.StaminaDepletionSpeed, 0.1f, 9);

                GUI.enabled = _nojumpoFirstPersonController.PlayerCanMove;
                EditorGUI.indentLevel--;
                EditorGUILayout.Space();
                _nojumpoFirstPersonController.NJFPAdvancedSettings.gravityMultiplier = EditorGUILayout.Slider(new GUIContent("Gravity Multiplier", "Determines how much the physics engine's gravitational force is multiplied."), _nojumpoFirstPersonController.NJFPAdvancedSettings.gravityMultiplier, 0.1f, 5);
                _nojumpoFirstPersonController.NJFPAdvancedSettings._maxSlopeAngle = EditorGUILayout.Slider(new GUIContent("Max Slope Angle", "Determines the maximum angle the player can walk up. If left 0, the slope detection/limiting system will not be used."), _nojumpoFirstPersonController.NJFPAdvancedSettings._maxSlopeAngle, 0, 70);
                _nojumpoFirstPersonController.NJFPAdvancedSettings.maxStepHeight = EditorGUILayout.Slider(new GUIContent("Max Step Height", "EXPERIMENTAL! Determines if a small ledge is a stair by comparing it to this value. Values over 0.5 produces     odd results."), _nojumpoFirstPersonController.NJFPAdvancedSettings.maxStepHeight, 0, 1);
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
            GUI.enabled = true;
            EditorGUILayout.Space();

        #endregion

        #region Headbobbing Setup

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Label("Headbobbing Setup", new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Bold,
                fontSize = 13
            }, GUILayout.ExpandWidth(true));
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            _nojumpoFirstPersonController.UseHeadbob = EditorGUILayout.ToggleLeft(new GUIContent("Enable Headbobbing", "Determines if headbobbing will be used."), _nojumpoFirstPersonController.UseHeadbob);
            GUI.enabled = _nojumpoFirstPersonController.UseHeadbob;
            _nojumpoFirstPersonController.Head = (Transform)EditorGUILayout.ObjectField(new GUIContent("Head Transform", "A transform representing the head. The camera should be a child to this transform."), _nojumpoFirstPersonController.Head, typeof(Transform), true);

            if (!_nojumpoFirstPersonController.Head) { EditorGUILayout.HelpBox("A Head Transform is required for headbobbing.", MessageType.Error); }

            GUI.enabled = _nojumpoFirstPersonController.UseHeadbob && _nojumpoFirstPersonController.Head;
            _nojumpoFirstPersonController.SnapHeadjointToCapsul = EditorGUILayout.ToggleLeft(new GUIContent("Snap Head to collider", "Recommended. Determines if the head joint will snap to the top on the capsul Collider, It provides better crouch results."), _nojumpoFirstPersonController.SnapHeadjointToCapsul);
            _nojumpoFirstPersonController.HeadbobFrequency = EditorGUILayout.Slider(new GUIContent("Headbob Frequency (Hz)", "Determines how fast the headbobbing cycle is."), _nojumpoFirstPersonController.HeadbobFrequency, 0.1f, 10);
            _nojumpoFirstPersonController.HeadbobSwayAngle = EditorGUILayout.Slider(new GUIContent("Tilt Angle", "Determines the angle the head will tilt."), _nojumpoFirstPersonController.HeadbobSwayAngle, 0, 10);
            _nojumpoFirstPersonController.HeadbobHeight = EditorGUILayout.Slider(new GUIContent("Headbob Hight", "Determines the highest point the head will reach in the headbob cycle."), _nojumpoFirstPersonController.HeadbobHeight, 0, 10);
            _nojumpoFirstPersonController.HeadbobSideMovement = EditorGUILayout.Slider(new GUIContent("Headbob Horizontal Movement", "Determines how much vertical movement will occur in the headbob cycle."), _nojumpoFirstPersonController.HeadbobSideMovement, 0, 10);
            _nojumpoFirstPersonController.JumpLandIntensity = EditorGUILayout.Slider(new GUIContent("Jump/Land Jerk Intensity", "Determines the Jerk intensity when jumping and landing if any."), _nojumpoFirstPersonController.JumpLandIntensity, 0, 15);
            GUI.enabled = true;
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        #endregion

        #region Audio/SFX Setup

            GUILayout.Label("Audio/SFX Setup", new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Bold,
                fontSize = 13
            }, GUILayout.ExpandWidth(true));
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            _nojumpoFirstPersonController.Volume = EditorGUILayout.Slider(new GUIContent("Volume", "Volume to play audio at."), _nojumpoFirstPersonController.Volume, 0, 10);
            EditorGUILayout.Space();
            _nojumpoFirstPersonController.FootstepSMode = (NJFPController.FootstepSoundMode)EditorGUILayout.EnumPopup(new GUIContent("Footstep Mode", "Determines the method used to trigger footsetps."), _nojumpoFirstPersonController.FootstepSMode);
            EditorGUILayout.Space();

            #region FS Static

            if (_nojumpoFirstPersonController.FootstepSMode == NJFPController.FootstepSoundMode.Static)
            {
                showStaticFS = EditorGUILayout.BeginFoldoutHeaderGroup(showStaticFS, new GUIContent("Footstep Clips", "Audio clips available as footstep sounds."));

                if (showStaticFS)
                {
                    GUILayout.BeginVertical("box");

                    for (int i = 0; i < staticFS.arraySize; i++)
                    {
                        SerializedProperty LS_ref = staticFS.GetArrayElementAtIndex(i);
                        EditorGUILayout.BeginHorizontal("box");
                        LS_ref.objectReferenceValue = EditorGUILayout.ObjectField("Clip " + (i + 1) + ":", LS_ref.objectReferenceValue, typeof(AudioClip), false);

                        if (GUILayout.Button(new GUIContent("X", "Remove this clip"), GUILayout.MaxWidth(20))) { this._nojumpoFirstPersonController.footStepSounds.RemoveAt(i); }

                        EditorGUILayout.EndHorizontal();
                    }

                    EditorGUILayout.Space();
                    EditorGUILayout.BeginHorizontal();

                    if (GUILayout.Button(new GUIContent("Add Clip", "Add new clip entry"))) { this._nojumpoFirstPersonController.footStepSounds.Add(null); }

                    if (GUILayout.Button(new GUIContent("Remove All Clips", "Remove all clip entries"))) { this._nojumpoFirstPersonController.footStepSounds.Clear(); }

                    EditorGUILayout.EndHorizontal();
                    GUILayout.EndVertical();
                    DropAreaGUI(_nojumpoFirstPersonController.footStepSounds, GUILayoutUtility.GetLastRect());
                }

                EditorGUILayout.EndFoldoutHeaderGroup();
                _nojumpoFirstPersonController.jumpSound = (AudioClip)EditorGUILayout.ObjectField(new GUIContent("Jump Clip", "An audio clip that will play when jumping."), _nojumpoFirstPersonController.jumpSound, typeof(AudioClip), false);
                _nojumpoFirstPersonController.landSound = (AudioClip)EditorGUILayout.ObjectField(new GUIContent("Land Clip", "An audio clip that will play when landing."), _nojumpoFirstPersonController.landSound, typeof(AudioClip), false);
            }

            #endregion

            else
            {
                _nojumpoFirstPersonController.dynamicFootstep.FootstepMaterialMode = (NJFPController.DynamicFootStep.DynamicFootstepMaterialMode)EditorGUILayout.EnumPopup(new GUIContent("Material Type", "Determines the type of material will trigger footstep audio."), _nojumpoFirstPersonController.dynamicFootstep.FootstepMaterialMode);
                EditorGUILayout.Space();

                #region Wood Section

                showWoodFS = EditorGUILayout.BeginFoldoutHeaderGroup(showWoodFS, new GUIContent("Wood Clips", "Audio clips available as footsteps when walking on a collider with the Physic Material assigned to 'Wood Physic Material'"));

                if (showWoodFS)
                {
                    GUILayout.BeginVertical("box");

                    if (_nojumpoFirstPersonController.dynamicFootstep.FootstepMaterialMode == NJFPController.DynamicFootStep.DynamicFootstepMaterialMode.PHYSICS_MATERIAL)
                    {
                        if (!_nojumpoFirstPersonController.dynamicFootstep.woodPhysMat.Any()) { EditorGUILayout.HelpBox("At least one Physic Material must be assigned first.", MessageType.Warning); }

                        EditorGUILayout.LabelField("Wood Physic Materials", new GUIStyle(GUI.skin.label)
                        {
                            alignment = TextAnchor.MiddleCenter,
                            fontStyle = FontStyle.Bold
                        });

                        for (int i = 0; i < woodPhysMat.arraySize; i++)
                        {
                            SerializedProperty LS_ref = woodPhysMat.GetArrayElementAtIndex(i);
                            EditorGUILayout.BeginHorizontal("box");
                            LS_ref.objectReferenceValue = EditorGUILayout.ObjectField("", LS_ref.objectReferenceValue, typeof(PhysicMaterial), false);

                            if (GUILayout.Button(new GUIContent("X", "Remove this Physic Material"), GUILayout.MaxWidth(20))) { _nojumpoFirstPersonController.dynamicFootstep.woodPhysMat.RemoveAt(i); }

                            EditorGUILayout.EndHorizontal();
                        }


                        if (GUILayout.Button(new GUIContent("Add new Physic Material entry", "Add new Physic Material entry"))) { _nojumpoFirstPersonController.dynamicFootstep.woodPhysMat.Add(null); }

                        GUI.enabled = _nojumpoFirstPersonController.dynamicFootstep.woodPhysMat.Any();
                    }

                    else
                    {
                        if (!_nojumpoFirstPersonController.dynamicFootstep.woodMat.Any()) { EditorGUILayout.HelpBox("At least one Material must be assigned first.", MessageType.Warning); }

                        EditorGUILayout.LabelField("Wood Materials", new GUIStyle(GUI.skin.label)
                        {
                            alignment = TextAnchor.MiddleCenter,
                            fontStyle = FontStyle.Bold
                        });

                        for (int i = 0; i < woodMat.arraySize; i++)
                        {
                            SerializedProperty LS_ref = woodMat.GetArrayElementAtIndex(i);
                            EditorGUILayout.BeginHorizontal("box");
                            LS_ref.objectReferenceValue = EditorGUILayout.ObjectField("", LS_ref.objectReferenceValue, typeof(Material), false);

                            if (GUILayout.Button(new GUIContent("X", "Remove this Material"), GUILayout.MaxWidth(20))) { _nojumpoFirstPersonController.dynamicFootstep.woodMat.RemoveAt(i); }

                            EditorGUILayout.EndHorizontal();
                        }

                        if (GUILayout.Button(new GUIContent("Add new Material entry", "Add new Material entry"))) { _nojumpoFirstPersonController.dynamicFootstep.woodMat.Add(null); }

                        GUI.enabled = _nojumpoFirstPersonController.dynamicFootstep.woodMat.Any();
                    }

                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Wood Audio Clips", new GUIStyle(GUI.skin.label)
                    {
                        alignment = TextAnchor.MiddleCenter,
                        fontStyle = FontStyle.Bold
                    });

                    for (int i = 0; i < woodFS.arraySize; i++)
                    {
                        SerializedProperty LS_ref = woodFS.GetArrayElementAtIndex(i);
                        EditorGUILayout.BeginHorizontal("box");
                        LS_ref.objectReferenceValue = EditorGUILayout.ObjectField("Clip " + (i + 1) + ":", LS_ref.objectReferenceValue, typeof(AudioClip), false);

                        if (GUILayout.Button(new GUIContent("X", "Remove this clip"), GUILayout.MaxWidth(20))) { _nojumpoFirstPersonController.dynamicFootstep.woodClipSet.RemoveAt(i); }

                        EditorGUILayout.EndHorizontal();
                    }

                    EditorGUILayout.Space();
                    EditorGUILayout.BeginHorizontal();

                    if (GUILayout.Button(new GUIContent("Add Clip", "Add new clip entry"))) { _nojumpoFirstPersonController.dynamicFootstep.woodClipSet.Add(null); }

                    if (GUILayout.Button(new GUIContent("Remove All Clips", "Remove all clip entries"))) { _nojumpoFirstPersonController.dynamicFootstep.woodClipSet.Clear(); }

                    EditorGUILayout.EndHorizontal();
                    GUILayout.EndVertical();
                    DropAreaGUI(_nojumpoFirstPersonController.dynamicFootstep.woodClipSet, GUILayoutUtility.GetLastRect());
                }

                GUI.enabled = true;
                EditorGUILayout.EndFoldoutHeaderGroup();
                EditorGUILayout.Space();

                #endregion

                #region Metal Section

                showMetalFS = EditorGUILayout.BeginFoldoutHeaderGroup(showMetalFS, new GUIContent("Metal & Glass Clips", "Audio clips available as footsteps when walking on a collider with the Physic Material assigned to 'Metal & Glass Physic Material'"));

                if (showMetalFS)
                {
                    GUILayout.BeginVertical("box");

                    if (_nojumpoFirstPersonController.dynamicFootstep.FootstepMaterialMode == NJFPController.DynamicFootStep.DynamicFootstepMaterialMode.PHYSICS_MATERIAL)
                    {
                        if (!_nojumpoFirstPersonController.dynamicFootstep.metalAndGlassPhysMat.Any()) { EditorGUILayout.HelpBox("At least one Physic Material must be assigned first.", MessageType.Warning); }

                        EditorGUILayout.LabelField("Metal & Glass Physic Materials", new GUIStyle(GUI.skin.label)
                        {
                            alignment = TextAnchor.MiddleCenter,
                            fontStyle = FontStyle.Bold
                        });

                        for (int i = 0; i < metalAndGlassPhysMat.arraySize; i++)
                        {
                            SerializedProperty LS_ref = metalAndGlassPhysMat.GetArrayElementAtIndex(i);
                            EditorGUILayout.BeginHorizontal("box");
                            LS_ref.objectReferenceValue = EditorGUILayout.ObjectField("", LS_ref.objectReferenceValue, typeof(PhysicMaterial), false);

                            if (GUILayout.Button(new GUIContent("X", "Remove this Physic Material"), GUILayout.MaxWidth(20))) { _nojumpoFirstPersonController.dynamicFootstep.metalAndGlassPhysMat.RemoveAt(i); }

                            EditorGUILayout.EndHorizontal();
                        }


                        if (GUILayout.Button(new GUIContent("Add new Physic Material entry", "Add new Physic Material entry"))) { _nojumpoFirstPersonController.dynamicFootstep.metalAndGlassPhysMat.Add(null); }

                        GUI.enabled = _nojumpoFirstPersonController.dynamicFootstep.metalAndGlassPhysMat.Any();
                    }

                    else
                    {
                        if (!_nojumpoFirstPersonController.dynamicFootstep.metalAndGlassMat.Any()) { EditorGUILayout.HelpBox("At least one Material must be assigned first.", MessageType.Warning); }

                        EditorGUILayout.LabelField("Metal & Glass Materials", new GUIStyle(GUI.skin.label)
                        {
                            alignment = TextAnchor.MiddleCenter,
                            fontStyle = FontStyle.Bold
                        });

                        for (int i = 0; i < metalAndGlassMat.arraySize; i++)
                        {
                            SerializedProperty LS_ref = metalAndGlassMat.GetArrayElementAtIndex(i);
                            EditorGUILayout.BeginHorizontal("box");
                            LS_ref.objectReferenceValue = EditorGUILayout.ObjectField("", LS_ref.objectReferenceValue, typeof(Material), false);

                            if (GUILayout.Button(new GUIContent("X", "Remove this Material"), GUILayout.MaxWidth(20))) { _nojumpoFirstPersonController.dynamicFootstep.metalAndGlassMat.RemoveAt(i); }

                            EditorGUILayout.EndHorizontal();
                        }

                        if (GUILayout.Button(new GUIContent("Add new Material entry", "Add new Material entry"))) { _nojumpoFirstPersonController.dynamicFootstep.metalAndGlassMat.Add(null); }

                        GUI.enabled = _nojumpoFirstPersonController.dynamicFootstep.metalAndGlassMat.Any();
                    }

                    EditorGUILayout.LabelField("Metal & Glass Audio Clips", new GUIStyle(GUI.skin.label)
                    {
                        alignment = TextAnchor.MiddleCenter,
                        fontStyle = FontStyle.Bold
                    });

                    for (int i = 0; i < metalFS.arraySize; i++)
                    {
                        SerializedProperty LS_ref = metalFS.GetArrayElementAtIndex(i);
                        EditorGUILayout.BeginHorizontal("box");
                        LS_ref.objectReferenceValue = EditorGUILayout.ObjectField("Clip " + (i + 1) + ":", LS_ref.objectReferenceValue, typeof(AudioClip), false);

                        if (GUILayout.Button(new GUIContent("X", "Remove this clip"), GUILayout.MaxWidth(20))) { _nojumpoFirstPersonController.dynamicFootstep.metalAndGlassClipSet.RemoveAt(i); }

                        EditorGUILayout.EndHorizontal();
                    }

                    EditorGUILayout.Space();
                    EditorGUILayout.BeginHorizontal();

                    if (GUILayout.Button(new GUIContent("Add Clip", "Add new clip entry"))) { _nojumpoFirstPersonController.dynamicFootstep.metalAndGlassClipSet.Add(null); }

                    if (GUILayout.Button(new GUIContent("Remove All Clips", "Remove all clip entries"))) { _nojumpoFirstPersonController.dynamicFootstep.metalAndGlassClipSet.Clear(); }

                    EditorGUILayout.EndHorizontal();
                    GUILayout.EndVertical();
                    DropAreaGUI(_nojumpoFirstPersonController.dynamicFootstep.metalAndGlassClipSet, GUILayoutUtility.GetLastRect());
                }

                GUI.enabled = true;
                EditorGUILayout.EndFoldoutHeaderGroup();
                EditorGUILayout.Space();

                #endregion

                #region Grass Section

                showGrassFS = EditorGUILayout.BeginFoldoutHeaderGroup(showGrassFS, new GUIContent("Grass Clips", "Audio clips available as footsteps when walking on a collider with the Physic Material assigned to 'Grass Physic Material'"));

                if (showGrassFS)
                {
                    GUILayout.BeginVertical("box");

                    if (_nojumpoFirstPersonController.dynamicFootstep.FootstepMaterialMode == NJFPController.DynamicFootStep.DynamicFootstepMaterialMode.PHYSICS_MATERIAL)
                    {
                        if (!_nojumpoFirstPersonController.dynamicFootstep.grassPhysMat.Any()) { EditorGUILayout.HelpBox("At least one Physic Material must be assigned first.", MessageType.Warning); }

                        EditorGUILayout.LabelField("Grass Physic Materials", new GUIStyle(GUI.skin.label)
                        {
                            alignment = TextAnchor.MiddleCenter,
                            fontStyle = FontStyle.Bold
                        });

                        for (int i = 0; i < grassPhysMat.arraySize; i++)
                        {
                            SerializedProperty LS_ref = grassPhysMat.GetArrayElementAtIndex(i);
                            EditorGUILayout.BeginHorizontal("box");
                            LS_ref.objectReferenceValue = EditorGUILayout.ObjectField("", LS_ref.objectReferenceValue, typeof(PhysicMaterial), false);

                            if (GUILayout.Button(new GUIContent("X", "Remove this Physic Material"), GUILayout.MaxWidth(20))) { _nojumpoFirstPersonController.dynamicFootstep.grassPhysMat.RemoveAt(i); }

                            EditorGUILayout.EndHorizontal();
                        }


                        if (GUILayout.Button(new GUIContent("Add new Physic Material entry", "Add new Physic Material entry"))) { _nojumpoFirstPersonController.dynamicFootstep.grassPhysMat.Add(null); }

                        GUI.enabled = _nojumpoFirstPersonController.dynamicFootstep.grassPhysMat.Any();
                    }

                    else
                    {
                        if (!_nojumpoFirstPersonController.dynamicFootstep.grassMat.Any()) { EditorGUILayout.HelpBox("At least one Material must be assigned first.", MessageType.Warning); }

                        EditorGUILayout.LabelField("Grass Materials", new GUIStyle(GUI.skin.label)
                        {
                            alignment = TextAnchor.MiddleCenter,
                            fontStyle = FontStyle.Bold
                        });

                        for (int i = 0; i < grassMat.arraySize; i++)
                        {
                            SerializedProperty LS_ref = grassMat.GetArrayElementAtIndex(i);
                            EditorGUILayout.BeginHorizontal("box");
                            LS_ref.objectReferenceValue = EditorGUILayout.ObjectField("", LS_ref.objectReferenceValue, typeof(Material), false);

                            if (GUILayout.Button(new GUIContent("X", "Remove this Material"), GUILayout.MaxWidth(20))) { _nojumpoFirstPersonController.dynamicFootstep.grassMat.RemoveAt(i); }

                            EditorGUILayout.EndHorizontal();
                        }

                        if (GUILayout.Button(new GUIContent("Add new Material entry", "Add new Material entry"))) { _nojumpoFirstPersonController.dynamicFootstep.grassMat.Add(null); }

                        GUI.enabled = _nojumpoFirstPersonController.dynamicFootstep.grassMat.Any();
                    }

                    EditorGUILayout.LabelField("Grass Audio Clips", new GUIStyle(GUI.skin.label)
                    {
                        alignment = TextAnchor.MiddleCenter,
                        fontStyle = FontStyle.Bold
                    });

                    for (int i = 0; i < grassFS.arraySize; i++)
                    {
                        SerializedProperty LS_ref = grassFS.GetArrayElementAtIndex(i);
                        EditorGUILayout.BeginHorizontal("box");
                        LS_ref.objectReferenceValue = EditorGUILayout.ObjectField("Clip " + (i + 1) + ":", LS_ref.objectReferenceValue, typeof(AudioClip), false);

                        if (GUILayout.Button(new GUIContent("X", "Remove this clip"), GUILayout.MaxWidth(20))) { _nojumpoFirstPersonController.dynamicFootstep.grassClipSet.RemoveAt(i); }

                        EditorGUILayout.EndHorizontal();
                    }

                    EditorGUILayout.Space();
                    EditorGUILayout.BeginHorizontal();

                    if (GUILayout.Button(new GUIContent("Add Clip", "Add new clip entry"))) { _nojumpoFirstPersonController.dynamicFootstep.grassClipSet.Add(null); }

                    if (GUILayout.Button(new GUIContent("Remove All Clips", "Remove all clip entries"))) { _nojumpoFirstPersonController.dynamicFootstep.grassClipSet.Clear(); }

                    EditorGUILayout.EndHorizontal();
                    GUILayout.EndVertical();
                    DropAreaGUI(_nojumpoFirstPersonController.dynamicFootstep.grassClipSet, GUILayoutUtility.GetLastRect());
                }

                GUI.enabled = true;
                EditorGUILayout.EndFoldoutHeaderGroup();
                EditorGUILayout.Space();

                #endregion

                #region Dirt Section

                showDirtFS = EditorGUILayout.BeginFoldoutHeaderGroup(showDirtFS, new GUIContent("Dirt & Gravel Clips", "Audio clips available as footsteps when walking on a collider with the Physic Material assigned to 'Dirt & Gravel Physic Material'"));

                if (showDirtFS)
                {
                    GUILayout.BeginVertical("box");

                    if (_nojumpoFirstPersonController.dynamicFootstep.FootstepMaterialMode == NJFPController.DynamicFootStep.DynamicFootstepMaterialMode.PHYSICS_MATERIAL)
                    {
                        if (!_nojumpoFirstPersonController.dynamicFootstep.dirtAndGravelPhysMat.Any()) { EditorGUILayout.HelpBox("At least one Physic Material must be assigned first.", MessageType.Warning); }

                        EditorGUILayout.LabelField("Dirt & Gravel Physic Materials", new GUIStyle(GUI.skin.label)
                        {
                            alignment = TextAnchor.MiddleCenter,
                            fontStyle = FontStyle.Bold
                        });

                        for (int i = 0; i < dirtAndGravelPhysMat.arraySize; i++)
                        {
                            SerializedProperty LS_ref = dirtAndGravelPhysMat.GetArrayElementAtIndex(i);
                            EditorGUILayout.BeginHorizontal("box");
                            LS_ref.objectReferenceValue = EditorGUILayout.ObjectField("", LS_ref.objectReferenceValue, typeof(PhysicMaterial), false);

                            if (GUILayout.Button(new GUIContent("X", "Remove this Physic Material"), GUILayout.MaxWidth(20))) { _nojumpoFirstPersonController.dynamicFootstep.dirtAndGravelPhysMat.RemoveAt(i); }

                            EditorGUILayout.EndHorizontal();
                        }


                        if (GUILayout.Button(new GUIContent("Add new Physic Material entry", "Add new Physic Material entry"))) { _nojumpoFirstPersonController.dynamicFootstep.dirtAndGravelPhysMat.Add(null); }

                        GUI.enabled = _nojumpoFirstPersonController.dynamicFootstep.dirtAndGravelPhysMat.Any();
                    }

                    else
                    {
                        if (!_nojumpoFirstPersonController.dynamicFootstep.dirtAndGravelMat.Any()) { EditorGUILayout.HelpBox("At least one Material must be assigned first.", MessageType.Warning); }

                        EditorGUILayout.LabelField("Dirt & Gravel Materials", new GUIStyle(GUI.skin.label)
                        {
                            alignment = TextAnchor.MiddleCenter,
                            fontStyle = FontStyle.Bold
                        });

                        for (int i = 0; i < dirtAndGravelMat.arraySize; i++)
                        {
                            SerializedProperty LS_ref = dirtAndGravelMat.GetArrayElementAtIndex(i);
                            EditorGUILayout.BeginHorizontal("box");
                            LS_ref.objectReferenceValue = EditorGUILayout.ObjectField("", LS_ref.objectReferenceValue, typeof(Material), false);

                            if (GUILayout.Button(new GUIContent("X", "Remove this Material"), GUILayout.MaxWidth(20))) { _nojumpoFirstPersonController.dynamicFootstep.dirtAndGravelMat.RemoveAt(i); }

                            EditorGUILayout.EndHorizontal();
                        }

                        if (GUILayout.Button(new GUIContent("Add new Material entry", "Add new Material entry"))) { _nojumpoFirstPersonController.dynamicFootstep.dirtAndGravelMat.Add(null); }

                        GUI.enabled = _nojumpoFirstPersonController.dynamicFootstep.dirtAndGravelMat.Any();
                    }

                    EditorGUILayout.LabelField("Dirt & Gravel Audio Clips", new GUIStyle(GUI.skin.label)
                    {
                        alignment = TextAnchor.MiddleCenter,
                        fontStyle = FontStyle.Bold
                    });

                    for (int i = 0; i < dirtFS.arraySize; i++)
                    {
                        SerializedProperty LS_ref = dirtFS.GetArrayElementAtIndex(i);
                        EditorGUILayout.BeginHorizontal("box");
                        LS_ref.objectReferenceValue = EditorGUILayout.ObjectField("Clip " + (i + 1) + ":", LS_ref.objectReferenceValue, typeof(AudioClip), false);

                        if (GUILayout.Button(new GUIContent("X", "Remove this clip"), GUILayout.MaxWidth(20))) { _nojumpoFirstPersonController.dynamicFootstep.dirtAndGravelClipSet.RemoveAt(i); }

                        EditorGUILayout.EndHorizontal();
                    }

                    EditorGUILayout.Space();
                    EditorGUILayout.BeginHorizontal();

                    if (GUILayout.Button(new GUIContent("Add Clip", "Add new clip entry"))) { _nojumpoFirstPersonController.dynamicFootstep.dirtAndGravelClipSet.Add(null); }

                    if (GUILayout.Button(new GUIContent("Remove All Clips", "Remove all clip entries"))) { _nojumpoFirstPersonController.dynamicFootstep.dirtAndGravelClipSet.Clear(); }

                    EditorGUILayout.EndHorizontal();
                    GUILayout.EndVertical();
                    DropAreaGUI(_nojumpoFirstPersonController.dynamicFootstep.dirtAndGravelClipSet, GUILayoutUtility.GetLastRect());
                }

                GUI.enabled = true;
                EditorGUILayout.EndFoldoutHeaderGroup();
                EditorGUILayout.Space();

                #endregion

                #region Rock Section

                showConcreteFS = EditorGUILayout.BeginFoldoutHeaderGroup(showConcreteFS, new GUIContent("Rock & Concrete Clips", "Audio clips available as footsteps when walking on a collider with the Physic Material assigned to 'Rock & Concrete Physic Material'"));

                if (showConcreteFS)
                {
                    GUILayout.BeginVertical("box");

                    if (_nojumpoFirstPersonController.dynamicFootstep.FootstepMaterialMode == NJFPController.DynamicFootStep.DynamicFootstepMaterialMode.PHYSICS_MATERIAL)
                    {
                        if (!_nojumpoFirstPersonController.dynamicFootstep.rockAndConcretePhysMat.Any()) { EditorGUILayout.HelpBox("At least one Physic Material must be assigned first.", MessageType.Warning); }

                        EditorGUILayout.LabelField("Rock & Concrete Physic Materials", new GUIStyle(GUI.skin.label)
                        {
                            alignment = TextAnchor.MiddleCenter,
                            fontStyle = FontStyle.Bold
                        });

                        for (int i = 0; i < rockAndConcretePhysMat.arraySize; i++)
                        {
                            SerializedProperty LS_ref = rockAndConcretePhysMat.GetArrayElementAtIndex(i);
                            EditorGUILayout.BeginHorizontal("box");
                            LS_ref.objectReferenceValue = EditorGUILayout.ObjectField("", LS_ref.objectReferenceValue, typeof(PhysicMaterial), false);

                            if (GUILayout.Button(new GUIContent("X", "Remove this Physic Material"), GUILayout.MaxWidth(20))) { _nojumpoFirstPersonController.dynamicFootstep.rockAndConcretePhysMat.RemoveAt(i); }

                            EditorGUILayout.EndHorizontal();
                        }


                        if (GUILayout.Button(new GUIContent("Add new Physic Material entry", "Add new Physic Material entry"))) { _nojumpoFirstPersonController.dynamicFootstep.rockAndConcretePhysMat.Add(null); }

                        GUI.enabled = _nojumpoFirstPersonController.dynamicFootstep.rockAndConcretePhysMat.Any();
                    }

                    else
                    {
                        if (!_nojumpoFirstPersonController.dynamicFootstep.rockAndConcreteMat.Any()) { EditorGUILayout.HelpBox("At least one Material must be assigned first.", MessageType.Warning); }

                        EditorGUILayout.LabelField("Rock & Concrete Materials", new GUIStyle(GUI.skin.label)
                        {
                            alignment = TextAnchor.MiddleCenter,
                            fontStyle = FontStyle.Bold
                        });

                        for (int i = 0; i < rockAndConcreteMat.arraySize; i++)
                        {
                            SerializedProperty LS_ref = rockAndConcreteMat.GetArrayElementAtIndex(i);
                            EditorGUILayout.BeginHorizontal("box");
                            LS_ref.objectReferenceValue = EditorGUILayout.ObjectField("", LS_ref.objectReferenceValue, typeof(Material), false);

                            if (GUILayout.Button(new GUIContent("X", "Remove this Material"), GUILayout.MaxWidth(20))) { _nojumpoFirstPersonController.dynamicFootstep.rockAndConcreteMat.RemoveAt(i); }

                            EditorGUILayout.EndHorizontal();
                        }

                        if (GUILayout.Button(new GUIContent("Add new Material entry", "Add new Material entry"))) { _nojumpoFirstPersonController.dynamicFootstep.rockAndConcreteMat.Add(null); }

                        GUI.enabled = _nojumpoFirstPersonController.dynamicFootstep.rockAndConcreteMat.Any();
                    }

                    EditorGUILayout.LabelField("Rock & Concrete Audio Clips", new GUIStyle(GUI.skin.label)
                    {
                        alignment = TextAnchor.MiddleCenter,
                        fontStyle = FontStyle.Bold
                    });

                    for (int i = 0; i < concreteFS.arraySize; i++)
                    {
                        SerializedProperty LS_ref = concreteFS.GetArrayElementAtIndex(i);
                        EditorGUILayout.BeginHorizontal("box");
                        LS_ref.objectReferenceValue = EditorGUILayout.ObjectField("Clip " + (i + 1) + ":", LS_ref.objectReferenceValue, typeof(AudioClip), false);

                        if (GUILayout.Button(new GUIContent("X", "Remove this clip"), GUILayout.MaxWidth(20))) { _nojumpoFirstPersonController.dynamicFootstep.rockAndConcreteClipSet.RemoveAt(i); }

                        EditorGUILayout.EndHorizontal();
                    }

                    EditorGUILayout.Space();
                    EditorGUILayout.BeginHorizontal();

                    if (GUILayout.Button(new GUIContent("Add Clip", "Add new clip entry"))) { _nojumpoFirstPersonController.dynamicFootstep.rockAndConcreteClipSet.Add(null); }

                    if (GUILayout.Button(new GUIContent("Remove All Clips", "Remove all clip entries"))) { _nojumpoFirstPersonController.dynamicFootstep.rockAndConcreteClipSet.Clear(); }

                    EditorGUILayout.EndHorizontal();
                    GUILayout.EndVertical();
                    DropAreaGUI(_nojumpoFirstPersonController.dynamicFootstep.rockAndConcreteClipSet, GUILayoutUtility.GetLastRect());
                }

                GUI.enabled = true;
                EditorGUILayout.EndFoldoutHeaderGroup();
                EditorGUILayout.Space();

                #endregion

                #region Mud Section

                showMudFS = EditorGUILayout.BeginFoldoutHeaderGroup(showMudFS, new GUIContent("Mud Clips", "Audio clips available as footsteps when walking on a collider with the Physic Material assigned to 'Mud Physic Material'"));

                if (showMudFS)
                {
                    GUILayout.BeginVertical("box");

                    if (_nojumpoFirstPersonController.dynamicFootstep.FootstepMaterialMode == NJFPController.DynamicFootStep.DynamicFootstepMaterialMode.PHYSICS_MATERIAL)
                    {
                        if (!_nojumpoFirstPersonController.dynamicFootstep.mudPhysMat.Any()) { EditorGUILayout.HelpBox("At least one Physic Material must be assigned first.", MessageType.Warning); }

                        EditorGUILayout.LabelField("Mud Physic Materials", new GUIStyle(GUI.skin.label)
                        {
                            alignment = TextAnchor.MiddleCenter,
                            fontStyle = FontStyle.Bold
                        });

                        for (int i = 0; i < mudPhysMat.arraySize; i++)
                        {
                            SerializedProperty LS_ref = mudPhysMat.GetArrayElementAtIndex(i);
                            EditorGUILayout.BeginHorizontal("box");
                            LS_ref.objectReferenceValue = EditorGUILayout.ObjectField("", LS_ref.objectReferenceValue, typeof(PhysicMaterial), false);

                            if (GUILayout.Button(new GUIContent("X", "Remove this Physic Material"), GUILayout.MaxWidth(20))) { _nojumpoFirstPersonController.dynamicFootstep.mudPhysMat.RemoveAt(i); }

                            EditorGUILayout.EndHorizontal();
                        }


                        if (GUILayout.Button(new GUIContent("Add new Physic Material entry", "Add new Physic Material entry"))) { _nojumpoFirstPersonController.dynamicFootstep.mudPhysMat.Add(null); }

                        GUI.enabled = _nojumpoFirstPersonController.dynamicFootstep.mudPhysMat.Any();
                    }

                    else
                    {
                        if (!_nojumpoFirstPersonController.dynamicFootstep.mudMat.Any()) { EditorGUILayout.HelpBox("At least one Material must be assigned first.", MessageType.Warning); }

                        EditorGUILayout.LabelField("Mud Materials", new GUIStyle(GUI.skin.label)
                        {
                            alignment = TextAnchor.MiddleCenter,
                            fontStyle = FontStyle.Bold
                        });

                        for (int i = 0; i < mudMat.arraySize; i++)
                        {
                            SerializedProperty LS_ref = mudMat.GetArrayElementAtIndex(i);
                            EditorGUILayout.BeginHorizontal("box");
                            LS_ref.objectReferenceValue = EditorGUILayout.ObjectField("", LS_ref.objectReferenceValue, typeof(Material), false);

                            if (GUILayout.Button(new GUIContent("X", "Remove this Material"), GUILayout.MaxWidth(20))) { _nojumpoFirstPersonController.dynamicFootstep.mudMat.RemoveAt(i); }

                            EditorGUILayout.EndHorizontal();
                        }

                        if (GUILayout.Button(new GUIContent("Add new Material entry", "Add new Material entry"))) { _nojumpoFirstPersonController.dynamicFootstep.mudMat.Add(null); }

                        GUI.enabled = _nojumpoFirstPersonController.dynamicFootstep.mudMat.Any();
                    }

                    EditorGUILayout.LabelField("Mud Audio Clips", new GUIStyle(GUI.skin.label)
                    {
                        alignment = TextAnchor.MiddleCenter,
                        fontStyle = FontStyle.Bold
                    });

                    for (int i = 0; i < mudFS.arraySize; i++)
                    {
                        SerializedProperty LS_ref = mudFS.GetArrayElementAtIndex(i);
                        EditorGUILayout.BeginHorizontal("box");
                        LS_ref.objectReferenceValue = EditorGUILayout.ObjectField("Clip " + (i + 1) + ":", LS_ref.objectReferenceValue, typeof(AudioClip), false);

                        if (GUILayout.Button(new GUIContent("X", "Remove this clip"), GUILayout.MaxWidth(20))) { _nojumpoFirstPersonController.dynamicFootstep.mudClipSet.RemoveAt(i); }

                        EditorGUILayout.EndHorizontal();
                    }

                    EditorGUILayout.Space();
                    EditorGUILayout.BeginHorizontal();

                    if (GUILayout.Button(new GUIContent("Add Clip", "Add new clip entry"))) { _nojumpoFirstPersonController.dynamicFootstep.mudClipSet.Add(null); }

                    if (GUILayout.Button(new GUIContent("Remove All Clips", "Remove all clip entries"))) { _nojumpoFirstPersonController.dynamicFootstep.mudClipSet.Clear(); }

                    EditorGUILayout.EndHorizontal();
                    GUILayout.EndVertical();
                    DropAreaGUI(_nojumpoFirstPersonController.dynamicFootstep.mudClipSet, GUILayoutUtility.GetLastRect());
                }

                GUI.enabled = true;
                EditorGUILayout.EndFoldoutHeaderGroup();
                EditorGUILayout.Space();

                #endregion

                #region Custom Section

                showCustomFS = EditorGUILayout.BeginFoldoutHeaderGroup(showCustomFS, new GUIContent("Custom Material Clips", "Audio clips available as footsteps when walking on a collider with the Physic Material assigned to 'Custom Physic Material'"));

                if (showCustomFS)
                {
                    GUILayout.BeginVertical("box");

                    if (_nojumpoFirstPersonController.dynamicFootstep.FootstepMaterialMode == NJFPController.DynamicFootStep.DynamicFootstepMaterialMode.PHYSICS_MATERIAL)
                    {
                        if (!_nojumpoFirstPersonController.dynamicFootstep.customPhysMat.Any()) { EditorGUILayout.HelpBox("At least one Physic Material must be assigned first.", MessageType.Warning); }

                        EditorGUILayout.LabelField("Custom Physic Materials", new GUIStyle(GUI.skin.label)
                        {
                            alignment = TextAnchor.MiddleCenter,
                            fontStyle = FontStyle.Bold
                        });

                        for (int i = 0; i < customPhysMat.arraySize; i++)
                        {
                            SerializedProperty LS_ref = customPhysMat.GetArrayElementAtIndex(i);
                            EditorGUILayout.BeginHorizontal("box");
                            LS_ref.objectReferenceValue = EditorGUILayout.ObjectField("", LS_ref.objectReferenceValue, typeof(PhysicMaterial), false);

                            if (GUILayout.Button(new GUIContent("X", "Remove this Physic Material"), GUILayout.MaxWidth(20))) { _nojumpoFirstPersonController.dynamicFootstep.customPhysMat.RemoveAt(i); }

                            EditorGUILayout.EndHorizontal();
                        }


                        if (GUILayout.Button(new GUIContent("Add new Physic Material entry", "Add new Physic Material entry"))) { _nojumpoFirstPersonController.dynamicFootstep.customPhysMat.Add(null); }

                        GUI.enabled = _nojumpoFirstPersonController.dynamicFootstep.customPhysMat.Any();
                    }

                    else
                    {
                        if (!_nojumpoFirstPersonController.dynamicFootstep.customMat.Any()) { EditorGUILayout.HelpBox("At least one Material must be assigned first.", MessageType.Warning); }

                        EditorGUILayout.LabelField("Custom Materials", new GUIStyle(GUI.skin.label)
                        {
                            alignment = TextAnchor.MiddleCenter,
                            fontStyle = FontStyle.Bold
                        });

                        for (int i = 0; i < customMat.arraySize; i++)
                        {
                            SerializedProperty LS_ref = customMat.GetArrayElementAtIndex(i);
                            EditorGUILayout.BeginHorizontal("box");
                            LS_ref.objectReferenceValue = EditorGUILayout.ObjectField("", LS_ref.objectReferenceValue, typeof(Material), false);

                            if (GUILayout.Button(new GUIContent("X", "Remove this Material"), GUILayout.MaxWidth(20))) { _nojumpoFirstPersonController.dynamicFootstep.customMat.RemoveAt(i); }

                            EditorGUILayout.EndHorizontal();
                        }

                        if (GUILayout.Button(new GUIContent("Add new Material entry", "Add new Material entry"))) { _nojumpoFirstPersonController.dynamicFootstep.customMat.Add(null); }

                        GUI.enabled = _nojumpoFirstPersonController.dynamicFootstep.customMat.Any();
                    }

                    EditorGUILayout.LabelField("Custom Audio Clips", new GUIStyle(GUI.skin.label)
                    {
                        alignment = TextAnchor.MiddleCenter,
                        fontStyle = FontStyle.Bold
                    });

                    for (int i = 0; i < customFS.arraySize; i++)
                    {
                        SerializedProperty LS_ref = customFS.GetArrayElementAtIndex(i);
                        EditorGUILayout.BeginHorizontal("box");
                        LS_ref.objectReferenceValue = EditorGUILayout.ObjectField("Clip " + (i + 1) + ":", LS_ref.objectReferenceValue, typeof(AudioClip), false);

                        if (GUILayout.Button(new GUIContent("X", "Remove this clip"), GUILayout.MaxWidth(20))) { _nojumpoFirstPersonController.dynamicFootstep.customClipSet.RemoveAt(i); }

                        EditorGUILayout.EndHorizontal();
                    }

                    EditorGUILayout.Space();
                    EditorGUILayout.BeginHorizontal();

                    if (GUILayout.Button(new GUIContent("Add Clip", "Add new clip entry"))) { _nojumpoFirstPersonController.dynamicFootstep.customClipSet.Add(null); }

                    if (GUILayout.Button(new GUIContent("Remove All Clips", "Remove all clip entries"))) { _nojumpoFirstPersonController.dynamicFootstep.customClipSet.Clear(); }

                    EditorGUILayout.EndHorizontal();
                    GUILayout.EndVertical();
                    DropAreaGUI(_nojumpoFirstPersonController.dynamicFootstep.customClipSet, GUILayoutUtility.GetLastRect());
                }

                GUI.enabled = true;
                EditorGUILayout.EndFoldoutHeaderGroup();
                EditorGUILayout.Space();

                #endregion

                #region Fallback Section

                showStaticFS = EditorGUILayout.BeginFoldoutHeaderGroup(showStaticFS, new GUIContent("Fallback Footstep Clips", "Audio clips available as footsteps in case a collider with an unrecognized/null Physic Material is walked on."));

                if (showStaticFS)
                {
                    GUILayout.BeginVertical("box");

                    for (int i = 0; i < staticFS.arraySize; i++)
                    {
                        SerializedProperty LS_ref = staticFS.GetArrayElementAtIndex(i);
                        EditorGUILayout.BeginHorizontal("box");
                        LS_ref.objectReferenceValue = EditorGUILayout.ObjectField("Clip " + (i + 1) + ":", LS_ref.objectReferenceValue, typeof(AudioClip), false);

                        if (GUILayout.Button(new GUIContent("X", "Remove this clip"), GUILayout.MaxWidth(20))) { this._nojumpoFirstPersonController.footStepSounds.RemoveAt(i); }

                        EditorGUILayout.EndHorizontal();
                    }

                    EditorGUILayout.Space();
                    EditorGUILayout.BeginHorizontal();

                    if (GUILayout.Button(new GUIContent("Add Clip", "Add new clip entry"))) { this._nojumpoFirstPersonController.footStepSounds.Add(null); }

                    if (GUILayout.Button(new GUIContent("Remove All Clips", "Remove all clip entries"))) { this._nojumpoFirstPersonController.footStepSounds.Clear(); }

                    EditorGUILayout.EndHorizontal();
                    GUILayout.EndVertical();
                    DropAreaGUI(_nojumpoFirstPersonController.footStepSounds, GUILayoutUtility.GetLastRect());
                }

                EditorGUILayout.EndFoldoutHeaderGroup();

                #endregion
            }

        #endregion

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            if (GUI.changed)
            {
                EditorUtility.SetDirty(_nojumpoFirstPersonController);
                Undo.RecordObject(_nojumpoFirstPersonController, "Nojumpo First Person Controller Change");
                _serializedNojumpoFirstPersonController.ApplyModifiedProperties();
            }
        }
        void DropAreaGUI(List<AudioClip> clipList, Rect dropArea) {
            var evt = Event.current;

            switch (evt.type)
            {
                case EventType.DragUpdated:
                case EventType.DragPerform:
                    if (!dropArea.Contains(evt.mousePosition)) { break; }

                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                    if (evt.type == EventType.DragPerform)
                    {
                        DragAndDrop.AcceptDrag();

                        foreach (var draggedObject in DragAndDrop.objectReferences)
                        {
                            var drago = draggedObject as AudioClip;

                            if (!drago) { continue; }

                            clipList.Add(drago);
                        }
                    }

                    Event.current.Use();
                    EditorUtility.SetDirty(_nojumpoFirstPersonController);
                    _serializedNojumpoFirstPersonController.ApplyModifiedProperties();
                    break;
            }
        }
    }
}