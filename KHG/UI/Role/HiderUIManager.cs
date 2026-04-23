using KHG.Events;
using UnityEngine;
using UnityEngine.InputSystem;

namespace KHG.UIs
{
    public class HiderUIManager : UIManager
    {
        [SerializeField] private bool enableDebugTitleBySpace;
        private int _debugIndex;

        private void Update()
        {
            if (!enableDebugTitleBySpace || Keyboard.current == null)
                return;

            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                SetTitle($"Title:{_debugIndex}", TitleMode.SubTitle, new TitleData
                {
                    fadeInTime = 0.3f,
                    lifeTime = 0.75f,
                    fadeOutTime = 1f
                });
                _debugIndex++;
            }
        }
    }
}
