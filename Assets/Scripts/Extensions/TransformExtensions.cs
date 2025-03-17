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
