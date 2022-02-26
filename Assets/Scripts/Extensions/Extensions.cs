using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Extensions
{
    public static class Extensions
    {
        public static float Map(this float value, float oldFrom, float oldTo, float newFrom, float newTo) =>
            (value - oldFrom) / (oldTo - oldFrom) * (newTo - newFrom) + newFrom;
        public static IEnumerable<(T value, int index)> WithIndexes<T>(this IEnumerable<T> values) =>
            values.Select((value, index) => (value, index));
        public static T GetRandomElement<T>(this IEnumerable<T> values) =>
            values.ElementAt(Random.Range(0, values.Count()));
    }
}
