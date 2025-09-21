using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace _00.Work.CIW.Code
{
    [CreateAssetMenu(fileName = "Obj Storage", menuName = "SO/Object/Storage", order = 0)]
    public class ObjectStorageSO : ScriptableObject
    {
        private GameObject _obj;

        public void SetObject(GameObject obj)
            => _obj = obj;

        public T GetComponentFromObject<T>() where T : Component
        {
            if (_obj.TryGetComponent<T>(out T comp)) 
                return comp;

            Debug.LogWarning($"이 녀석 {typeof(T)} Comp를 안 넣었잖아? 매차쿠차 해버려야겠군wwwwwwww");
            return null;
        } 
    }
}
