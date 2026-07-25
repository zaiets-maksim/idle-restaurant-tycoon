using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Extensions
{
    public static class TransformExtensions
    {
        public static Vector3 ToVector3(this Vector2 v, float y = 0f) => 
            new(v.x, y, v.y);

        public static Vector2 ToVector2(this Vector3 v) => 
            new(v.x, v.z);
        
        public static List<Vector3> ToVector3List(this List<Vector2> list, float y = 0f) => 
            list.Select(v => new Vector3(v.x, y, v.y)).ToList();

        public static List<Vector2> ToVector2List(this List<Vector3> list) => 
            list.Select(v => new Vector2(v.x, v.z)).ToList();


        public static T NearestTo<T>(Transform target, IEnumerable<T> objects) where T : Component =>
            objects
                .Where(obj => obj != null)
                .OrderBy(obj => Vector3.Distance(target.position, obj.transform.position))
                .FirstOrDefault();

        public static List<T> SortedByDistance<T>(Transform target, List<T> objects) where T : Component =>
            objects
                .Where(obj => obj != null)
                .OrderBy(obj => Vector3.Distance(target.position, obj.transform.position))
                .ToList();

        public static IEnumerator RotateTo(Transform obj, Transform target, float speed = 7f, Action rollback = null)
        {
            while (Quaternion.Angle(obj.rotation, target.rotation) > 0.5f)
            {
                obj.rotation = Quaternion.Lerp(obj.rotation, target.rotation, speed * Time.deltaTime);
                yield return null;
            }

            obj.rotation = target.rotation;
            rollback?.Invoke();
        }

        public static IEnumerator RotateTo(Transform obj, Vector3 targetPosition, float speed = 7f, Action rollback = null)
        {
            Vector3 direction = (targetPosition - obj.position).normalized;
            if (direction == Vector3.zero)
            {
                rollback?.Invoke();
                yield break;
            }

            Quaternion targetRotation = Quaternion.LookRotation(direction);

            while (Quaternion.Angle(obj.rotation, targetRotation) > 0.5f)
            {
                obj.rotation = Quaternion.Lerp(obj.rotation, targetRotation, speed * Time.deltaTime);
                yield return null;
            }

            obj.rotation = targetRotation;
            rollback?.Invoke();
        }
    }
}
