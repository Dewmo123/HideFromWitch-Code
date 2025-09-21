using Assets._00.Work.CDH.Code.Events;
using DewmoLib.Utiles;
using UnityEngine;

namespace Assets._00.Work.CDH.Code.Sound
{
    public class SoundPlayCompo : MonoBehaviour
    {
        [SerializeField] private EventChannelSO soundEventChannel;
        [SerializeField] private SoundSO testClip;

        [ContextMenu("PlaySound")]
        public void PlaySound()
        {
            var evt = SoundEvents.PlaySFXEvent.Initializer(transform.position, testClip);
            soundEventChannel.InvokeEvent(evt);
        }
        public void PlaySound(Transform trm)
        {
            var evt = SoundEvents.PlaySFXEvent.Initializer(trm.position, testClip);
            soundEventChannel.InvokeEvent(evt);
        }

        public void PlayBGM()
        {
            var evt = SoundEvents.PlayBGMEvent.Initializer(testClip);
            soundEventChannel.InvokeEvent(evt);
        }
    }
}
