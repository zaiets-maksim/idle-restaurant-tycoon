using System;
using System.Collections;
using System.Threading.Tasks;
using tetris.Scripts.Extensions;
using Unity.VisualScripting;
using UnityEngine;

namespace Extensions
{
    public static class AnimationExtensions
    {
        public static async void AnimatePingPong(this Transform transform, Func<Transform, Vector3> getValue, Action<Transform, Vector3> setValue, Vector3 targetValue, float duration)
        {
            var defaultValue = getValue(transform);

            transform.AnimateOverTime(
                getValue, 
                setValue, 
                targetValue, 
                duration
            );
            
            await Task.Delay(duration.ToMiliseconds());

            transform.AnimateOverTime(
                getValue, 
                setValue, 
                defaultValue,
                duration
            );
        }
        
        public static void AnimateOverTime<T>(this Transform transform, Func<Transform, T> getValue, Action<Transform, T> setValue, T targetValue, float duration) where T : struct => 
            CoroutineRunner.instance.StartCoroutine(ChangeValueOverTime(transform, getValue, setValue, targetValue, duration));

        private static IEnumerator ChangeValueOverTime<T>(Transform transform, Func<Transform, T> getValue, Action<Transform, T> setValue, T targetValue, float duration) where T : struct
        {
            T initialValue = getValue(transform);
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;
                setValue(transform, LerpValue(initialValue, targetValue, t));
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            setValue(transform, targetValue);
        }

        private static T LerpValue<T>(T start, T end, float t) where T : struct
        {
            if (typeof(T) == typeof(Vector3)) return (T)(object)Vector3.LerpUnclamped((Vector3)(object)start, (Vector3)(object)end, t);
            if (typeof(T) == typeof(Vector2)) return (T)(object)Vector2.LerpUnclamped((Vector2)(object)start, (Vector2)(object)end, t);
            if (typeof(T) == typeof(float)) return (T)(object)Mathf.LerpUnclamped((float)(object)start, (float)(object)end, t);
            if (typeof(T) == typeof(Quaternion)) return (T)(object)Quaternion.SlerpUnclamped((Quaternion)(object)start, (Quaternion)(object)end, t);

            throw new NotSupportedException($"LerpValue does not support type {typeof(T)}");
        }
        
        
        public static void AnimateOverTime<T>(this RectTransform rectTransform, Func<RectTransform, T> getValue, Action<RectTransform, T> setValue, T targetValue, float duration) => 
            CoroutineRunner.instance.StartCoroutine(rectTransform.ChangeValueOverTime(getValue, setValue, targetValue, duration));

        public static IEnumerator ChangeValueOverTime<T>(this RectTransform rectTransform, Func<RectTransform, T> getValue, Action<RectTransform, T> setValue, T targetValue, float duration)
        {
            T initialValue = getValue(rectTransform);
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                T newValue = Lerp(initialValue, targetValue, elapsedTime / duration);
                setValue(rectTransform, newValue);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            setValue(rectTransform, targetValue);
        }

        private static T Lerp<T>(T from, T to, float t)
        {
            if (from is float fromF && to is float toF)
                return (T)(object)Mathf.Lerp(fromF, toF, t);

            if (from is int fromI && to is int toI)
                return (T)(object)Mathf.RoundToInt(Mathf.Lerp(fromI, toI, t));

            return from;
        }

    }
}
