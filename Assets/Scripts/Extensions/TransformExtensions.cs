using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Extensions
{
    public static class TransformExtensions
    {
        public static T NearestTo<T>(Transform target, IEnumerable<T> objects) where T : Component =>
            objects
                .OrderBy(obj => Vector3.Distance(target.position, obj.transform.position))
                .FirstOrDefault();

        public static List<T> SortedByDistance<T>(Transform target, List<T> objects) where T : Component =>
            objects
                .OrderBy(obj => Vector3.Distance(target.position, obj.transform.position))
                .ToList();

        public static IEnumerator RotateTo(Transform obj, Transform target, float speed = 7f, Action rollback = null)
        {
            while (Vector3.Distance(obj.eulerAngles, target.eulerAngles) > 0.01f)
            {
                obj.rotation = Quaternion.Lerp(obj.rotation, target.rotation, speed * Time.deltaTime);
                yield return null;
            }

            obj.rotation = target.rotation;
            rollback?.Invoke();
        }
    }
}
