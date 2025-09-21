using _00.Work.CIW.Code;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace Assets._00.Work.CIW.Code
{
    [DefaultExecutionOrder(-5)]
    public class ObjectStorageInjector : MonoBehaviour
    {
        [SerializeField] private SerializedDictionary<ObjectStorageSO, GameObject> storageDict;

        private void Awake()
        {
            foreach (var pair in storageDict)
                pair.Key.SetObject(pair.Value);
        }
    }
}
