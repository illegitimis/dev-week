namespace DevWeek.Algo
{
    using System;
    using System.Threading.Tasks;

    public class BackgroundJobRunner : IRunBackgroundJob
    {
        public BackgroundJobRunner()
        {
        }

        /// <summary>
        /// Creates and starts a NON-THREAD POOL task.
        /// Does not block in a web context.
        /// </summary>
        /// <param name="action">what to execute</param>
        /// <remarks>
        /// Hangfire Background Process permalink
        /// https://github.com/HangfireIO/Hangfire/blob/f970f56a1bacbfa762a9db9030cadabc5e0b9cb2/src/Hangfire.Core/Server/ServerProcessExtensions.cs#L57
        /// 
        /// Provides a hint to the System.Threading.Tasks.TaskScheduler that oversubscription may be warranted.
        /// Oversubscription lets you create more threads than the available number of hardware threads. 
        /// It also provides a hint to the task scheduler that an additional thread might be required for the task 
        /// so that it does not block the forward progress of other threads or work items on the local thread-pool queue.
        /// </remarks>
        public Task Enqueue(Action action) =>
            Task.Factory.StartNew(() => Failsafe(action), TaskCreationOptions.LongRunning);

        public Task<T> Enqueue<T>(Func<T> func) =>
            Task.Factory.StartNew(() => 
            {
                T t = default(T);
                Failsafe(() => t = func());
                return t;
            }, TaskCreationOptions.LongRunning);

        private void Failsafe(Action action)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                if (ex is OperationCanceledException)
                {
                    // Graceful shutdown
                    // logger.Trace($"Background process '{process}' was stopped due to a shutdown request.");
                }
                else
                {
                    /*
                    logger.FatalException(
                        $"Fatal error occurred during execution of '{process}' process. It will be stopped. See the exception for details.",
                        ex);
                    */
                    // throw;
                }
            }
        }
    }
}
