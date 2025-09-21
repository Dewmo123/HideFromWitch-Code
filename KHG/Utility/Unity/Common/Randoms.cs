using System.Collections.Generic;
using UnityEngine;

namespace Utility.Unity.Common
{
    public static class Randoms
    {
        public static Vector3 RandomLinnerVector3(Vector3 v1, Vector3 v2)
        {
            Vector3 vector = new Vector3(
                UnityEngine.Random.Range(v1.x, v2.x),
                UnityEngine.Random.Range(v1.y, v2.y),
                UnityEngine.Random.Range(v1.z, v2.z)
                );
            return vector;
        }
        public static Color RandomColor(float alpha = 1f)
        {
            Color color = new Color(
                UnityEngine.Random.Range(0f, 1f),
                UnityEngine.Random.Range(0f, 1f),
                UnityEngine.Random.Range(0f, 1f),
                alpha
                );
            return color;
        }
        public static T RandomPick<T>(List<T> list)
        {
            T target = list[UnityEngine.Random.Range(0, list.Count)];
            return target;
        }
    }
}
