using UnityEngine;

namespace Utility.Unity.Common
{
    public class RayUtility
    {
        //-----3D
        public bool CheckObject(Vector3 start, Vector3 direction, int distance, LayerMask layerMask, bool debug = false)
        {
            if (debug) RayDebug(start, direction * distance, Color.red);
            return Physics.Raycast(start, direction, distance, layerMask);
        }

        public RaycastHit GetObjInfo(Vector3 start, Vector3 direction, int distance, LayerMask layerMask, bool debug = false)
        {
            if (debug) RayDebug(start, direction * distance, Color.red);
            Physics.Raycast(start, direction, out RaycastHit hit, distance, layerMask);
            return hit;
        }

        public RaycastHit[] GetObjInfos(Vector3 start, Vector3 direction, int distance, LayerMask layerMask, bool debug = false)
        {
            if (debug) RayDebug(start, direction * distance, Color.red);
            return Physics.RaycastAll(start, direction, distance, layerMask); ;
        }

        //-----2D
        public RaycastHit2D GetObjInfo(Vector2 start, Vector2 direction, int distance, LayerMask layerMask, bool debug = false)
        {
            if (debug) RayDebug(start, direction * distance, Color.red);
            return Physics2D.Raycast(start, direction, distance, layerMask); ;
        }

        public RaycastHit2D[] GetObjInfos(Vector2 start, Vector2 direction, int distance, LayerMask layerMask, bool debug = false)
        {
            if (debug) RayDebug(start, direction * distance, Color.red);
            return Physics2D.RaycastAll(start, direction, distance, layerMask);
        }


        private void RayDebug(Vector3 start, Vector3 dir, Color color)
        {
            Debug.DrawRay(start, dir, Color.red);
        }
    }
}
