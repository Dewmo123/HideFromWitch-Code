using KHG.Events;
using UnityEngine.InputSystem;

namespace KHG.UIs
{
    public class HiderUIManager : UIManager
    {
        int i = 0;
        private void Update()
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                SetTitle("Title:" + i, TitleMode.SubTitle, new TitleData() { fadeInTime = 0.3f, fadeOutTime = 1f });
                i++;
            }
        }
    }

}