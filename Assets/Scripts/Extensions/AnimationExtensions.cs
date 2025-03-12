using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace Extensions
{
    public static class AnimationExtensions
    {
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
