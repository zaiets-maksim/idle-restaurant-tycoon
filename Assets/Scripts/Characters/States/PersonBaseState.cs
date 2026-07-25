using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Characters.PersonStateMachine
{
    public abstract class PersonBaseState
    {
        private CancellationTokenSource _cts;

        public void EnterSafe()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = new CancellationTokenSource();
            var token = _cts.Token;
            _ = EnterAsync(token);
        }

        private async Task EnterAsync(CancellationToken ct)
        {
            try
            {
                await Enter(ct);
            }
            catch (System.OperationCanceledException) { }
            catch (System.Exception e)
            {
                Debug.LogError($"[PersonBaseState] {GetType().Name} failed: {e}");
            }
        }

        protected abstract Task Enter(CancellationToken ct);
        public abstract void Exit();

        public void Cancel()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
        }

        public virtual void Update() { }
    }
}