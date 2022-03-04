using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Extensions
{
    public static class Extensions
    {
        public static float Sigmoid(this float value) => 1.0f / (1.0f + (float) Math.Exp(-value));

        public static IEnumerable<(T value, int index)> WithIndexes<T>(this IEnumerable<T> values) =>
            values.Select((value, index) => (value, index));

        public static T GetRandomElement<T>(this IEnumerable<T> values) =>
            values.ElementAt(Random.Range(0, values.Count()));

        public static Vector3[] GetVertices(this BoxCollider boxCollider)
        {
            int verticesCount = 4;
            var vertices = new Vector3[verticesCount];

            var trans = boxCollider.transform;
            var center = boxCollider.center;
            var min = center - boxCollider.size * 0.5f;
            var max = center + boxCollider.size * 0.5f;

            vertices[0] = trans.TransformPoint(new Vector3(min.x, max.y, min.z));
            vertices[1] = trans.TransformPoint(new Vector3(max.x, max.y, min.z));
            vertices[2] = trans.TransformPoint(new Vector3(max.x, max.y, max.z));
            vertices[3] = trans.TransformPoint(new Vector3(min.x, max.y, max.z));

            return vertices;
        }

        public static Vector2 ToVector2XZ(this Vector3 vector)
        {
            return new Vector2(vector.x, vector.z);
        }
    }
}
