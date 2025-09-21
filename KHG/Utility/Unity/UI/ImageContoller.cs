using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Utility.Unity.UI
{
    public class ImageContoller : MonoBehaviour
    {
        public List<Image> panels;

        public Image AddPanel(Image target)
        {
            panels.Add(target);
            return target;
        }
        public void AddPanel(List<Image> targets) => panels.AddRange(targets);
        public void RemovePanel(Image target) => panels.Remove(target);
        public void RemovePanel(int index)
        {
            if (index > panels.Count || index < 0) Debug.LogError($"Panel인덱스 범위 초과.");
            panels.RemoveAt(index);
        }

        public void RemoveAll() => panels.Clear();

        public void OpenAll() => panels.ForEach(img => Open(img));
        public Image Open(Image target)
        {
            target.enabled = true;
            return target;
        }
        public void closeAll() => panels.ForEach(img => Close(img));
        public Image Close(Image target)
        {
            target.enabled = false;
            return target;
        }

        public void reverseAll() => panels.ForEach(img => Reverse(img));
        public Image Reverse(Image target)
        {
            target.enabled = !target.enabled;
            return target;
        }
    }
}
