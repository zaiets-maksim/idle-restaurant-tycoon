using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

namespace Extensions
{
    public static class TaskExtension
    {
        public static UniTask WaitFor(Action<Action> action)
        {
            var tcs = new UniTaskCompletionSource<bool>();
            action(() => tcs.TrySetResult(true));
            return tcs.Task;
        }
    }
}