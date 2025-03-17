using System;
using System.Threading.Tasks;

namespace Extensions
{
    public static class TaskExtension
    {
        public static Task WaitFor(Action<Action> action)
        {
            var tcs = new TaskCompletionSource<bool>();
            action(() => tcs.SetResult(true));
            return tcs.Task;
        }
    }
}