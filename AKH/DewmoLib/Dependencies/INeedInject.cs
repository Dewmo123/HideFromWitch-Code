using DewmoLib.Utiles;
using System.Threading.Tasks;
using UnityEngine;

namespace DewmoLib.Dependencies
{
    public interface INeedInject
    {
        public bool Injected { get; set; }
        public AwaitableCompletionSource WaitInject { get; }
        public async Task Init()
        {
            if (!Injected)
            {
                var evt = DependencyEvents.DependencyEvent;
                evt.needInject = this;
                EventBus.InvokeEvent(evt);
                await WaitInject.Awaitable;
                Injected = true;
            }
        }
    }
}
