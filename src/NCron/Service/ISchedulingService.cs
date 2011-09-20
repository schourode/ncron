using System;
using NCron.Logging;

namespace NCron.Service
{
    /// <summary>
    /// Executes jobs according to specified schedules.
    /// </summary>
    public interface ISchedulingService
    {
        /// <summary>
        /// Sets the log factory that is used to create a log for each job execution.
        /// </summary>
        ILogFactory LogFactory { set; }

        /// <summary>
        /// Adds a scheduled job for automatic execution by this service.
        /// </summary>
        /// <param name="schedule">A method for computation of occurrences in the schedule, taking last execution as parameter.</param>
        /// <param name="executionWrapper">The execution wrapper method to be invoked on each occurence of the job.</param>
        /// <returns>The newly scheduled job.</returns>
        ScheduledJob AddScheduledJob(Func<DateTime, DateTime> schedule, JobExecutionWrapper executionWrapper);

        /// <summary>
        /// Adds a named job for manual execution by this service.
        /// </summary>
        /// <param name="name">The name under which the job should be registered.</param>
        /// <param name="executionWrapper">The execution wrapper method to be invoked on each invocation of the job.</param>
        void AddNamedJob(string name, JobExecutionWrapper executionWrapper);

        /// <summary>
        /// Starts the scheduling service.
        /// </summary>
        void Start();

        /// <summary>
        /// Stops the scheduling service.
        /// </summary>
        void Stop();
    }
}