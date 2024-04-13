using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace NOJUMPO.Utils
{
    public static class NJUtils
    {
        // ---------------------------------- CAMERA ---------------------------------
        public static Camera MainCam { get; } = Camera.main;

        // ----------------------------------- GAME ----------------------------------
        public static void QuitGame() {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }


        // ---------------------------- WAIT FOR SECONDS -----------------------------
        static readonly Dictionary<float, WaitForSeconds> WaitDictionary = new Dictionary<float, WaitForSeconds>();
        /// <summary>
        /// Better WaitForSeconds. 
        /// Looks if there are same valued WaitForSeconds if you have uses it to not create garbage
        /// if not, creates it
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static WaitForSeconds GetWait(float time) {
            if (time < 1f / Application.targetFrameRate) 
                return null;

            if (WaitDictionary.TryGetValue(time, out var wait))
                return wait;

            WaitDictionary[time] = new WaitForSeconds(time);
            return WaitDictionary[time];
        }


        // ----------------------------------- UI ------------------------------------
        /// <summary>
        /// Set the visibility and lock state of cursor
        /// </summary>
        /// <param name="isVisible"></param>
        public static void SetCursorState(bool isVisible) {
            Cursor.visible = isVisible;
            Cursor.lockState = isVisible ? CursorLockMode.None : CursorLockMode.Locked;
        }

        /// <summary>
        /// Put any object to the canvas, spawn particles on canvas etc.
        /// </summary>
        /// <param name="rectTransform"></param>
        /// <returns></returns>
        public static Vector2 GetWorldPositionOfCanvasElement(RectTransform rectTransform) {
            RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, rectTransform.position, MainCam, out var result);
            return result;
        }

        public static IEnumerator FadeCoroutine(CanvasGroup target, float targetAlphaOne, float targetAlphaTwo, float fadeDuration, bool isTargetAlphaOne) {
            float initialAlpha = target.alpha;
            float targetAlpha = isTargetAlphaOne ? targetAlphaOne : targetAlphaTwo;
            float timeElapsed = 0.0f;

            while (timeElapsed < fadeDuration)
            {
                float t = timeElapsed / fadeDuration;
                target.alpha = Mathf.Lerp(initialAlpha, targetAlpha, t);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            target.alpha = targetAlpha;
        }


        // ------------------------------- TRANSFORM ---------------------------------
        public static IEnumerator ScaleCoroutine(Transform target, Vector3 targetScaleOne, Vector3 targetScaleTwo, float scaleDuration, bool isTargetScaleOne) {
            Vector3 initialScale = target.localScale;
            Vector3 targetScale = isTargetScaleOne ? targetScaleOne : targetScaleTwo;
            float timeElapsed = 0.0f;

            while (timeElapsed < scaleDuration)
            {
                float t = timeElapsed / scaleDuration;
                target.localScale = Vector3.Lerp(initialScale, targetScale, t);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            target.localScale = targetScale;
        }


        // -------------------------------- ACTION -----------------------------------
        /// <summary>
        /// Use Cautiously! It can stack up and run in the background non stop therefore mess up the fps
        /// </summary>
        /// <param name="action"></param>
        /// <param name="repeatInterval"></param>
        /// <returns></returns>
        public static IEnumerator InfiniteRepeatedAction(Action action, int repeatInterval) {
            while (true)
            {
                action?.Invoke();
                yield return GetWait(repeatInterval);
            }
        }

        public static IEnumerator DelayedAction(Action action, float delayAmount) {
            yield return GetWait(delayAmount);
            action.Invoke();
        }

        public static IEnumerator DelayedAction<T>(Action<T> action, float delayAmount, T parameter) {
            yield return GetWait(delayAmount);
            action.Invoke(parameter);
        }

        public static IEnumerator DelayedAction<T1, T2>(Action<T1, T2> action, float delayAmount, T1 parameter1, T2 parameter2) {
            yield return GetWait(delayAmount);
            action.Invoke(parameter1, parameter2);
        }

        // ------------------------------- RESOURCE ----------------------------------
        /// <summary>
        /// Load volume profile from given path.
        /// </summary>
        /// <param name="path">Path from where volume profile should be loaded.</param>
        public static void LoadVolumeProfile(this Volume volume, string path) {
            var profile = Resources.Load<VolumeProfile>(path);
            volume.profile = profile;
        }
    }
}