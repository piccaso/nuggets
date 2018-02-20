using System.Threading;
using System.Threading.Tasks;

namespace AwaitAndGetResult
{
    public static class TaskExtensions
    {

        private static readonly TaskFactory Factory = new TaskFactory(CancellationToken.None, TaskCreationOptions.None, TaskContinuationOptions.None, TaskScheduler.Default);

        //public static T AwaitAndGetResult<T>(this Task<T> t) => Factory
        //    .StartNew(async () => await t.ConfigureAwait(false))
        //    .Unwrap<T>()
        //    .GetAwaiter()
        //    .GetResult();
    }
}