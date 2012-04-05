using System;
using System.Threading;
using NCron;
using NCron.Service;
using Xunit;

namespace Tests.NCron
{
    public class CronJobTests
    {
        public class CronJobTestsBase : IDisposable
        {
            private readonly SchedulingService _schedulingService;
            private readonly SystemClock _systemClock;

            protected CronJobTestsBase()
            {
                _systemClock = new SystemClock(new DateTime(2012, 1, 1));
                SystemTime.Resolver = () => _systemClock.UtcNow;
                _schedulingService = new SchedulingService();
            }

            protected SchedulingService SchedulingService
            {
                get { return _schedulingService; }
            }

            #region IDisposable Members

            public void Dispose()
            {
                _schedulingService.Dispose();
                SystemTime.Resolver = null;
            }

            #endregion
        }

        public class Given_a_scheduled_CronJob : CronJobTestsBase
        {
            private readonly TestCronJob _sut;

            public Given_a_scheduled_CronJob()
            {
                _sut = new TestCronJob();
                SchedulingService.AddScheduledJob(time => time.AddSeconds(1), (Action<ICronJob> callback) => callback(_sut));
            }

            [Fact]
            public void When_start_SchedulingService_Then_job_should_execute()
            {
                SchedulingService.Start();
                Assert.True(_sut.ExecuteWaitHandle.WaitOne(TimeSpan.FromSeconds(5)));
            }
        }

        public class TestCronJob : CronJob
        {
            private readonly ManualResetEvent _executeWaitHandle = new ManualResetEvent(false);

            public ManualResetEvent ExecuteWaitHandle
            {
                get { return _executeWaitHandle; }
            }

            public override void Execute()
            {
                _executeWaitHandle.Set();
            }
        }
    }
}