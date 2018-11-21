namespace DevWeek.Algo
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Unpretentious job scheduler
    /// </summary>
    public interface IRunBackgroundJob
    {
        /// <summary>
        /// Queues an action to execute in the background.
        /// </summary>
        /// <param name="action">what to do</param>
        /// <returns>.Net task</returns>
        Task Enqueue(Action action);

        Task<T> Enqueue<T>(Func<T> func);
    }
}
