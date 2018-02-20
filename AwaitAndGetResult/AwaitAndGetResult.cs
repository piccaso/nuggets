using System.Threading;
using System.Threading.Tasks;

namespace AwaitAndGetResult
{
    public static class TaskExtensions
    {

        public static T AwaitAndGetResult<T>(this Task<T> task) => AwaitAndGetResult(task, new TaskFactory());

        public static T AwaitAndGetResult<T>(this Task<T> task, TaskFactory taskFactory)
        {
            return taskFactory
                .StartNew(async () => await task.ConfigureAwait(false))
                .Unwrap<T>()
                .GetAwaiter()
                .GetResult();
        }
    }
}