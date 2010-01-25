/*
 * Copyright 2010 Joern Schou-Rode
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using NCron.Service;

namespace NCron.Fluent
{
    public class SchedulePart : Part
    {
        public SchedulePart(SchedulingService service, QueueEntry queueEntry)
            : base(service, queueEntry)
        {
        }

        public JobPart Run(Func<ICronJob> jobCallback)
        {
            QueueEntry.ExecuteCallback = (a) =>
            {
                using (var job = jobCallback())
                {
                    a(job);
                }
            };

            return new JobPart(Service, QueueEntry);
        }

        public SchedulePart<TContext> With<TContext>(Func<TContext> contextCallback)
            where TContext : IDisposable
        {
            return new SchedulePart<TContext>(Service, QueueEntry, contextCallback);
        }
    }

    public class SchedulePart<TContext> : Part
        where TContext : IDisposable
    {
        private readonly Func<TContext> _contextCallback;

        public SchedulePart(SchedulingService service, QueueEntry queueEntry, Func<TContext> contextCallback)
            : base(service, queueEntry)
        {
            _contextCallback = contextCallback;
        }

        public JobPart Run(Func<TContext, ICronJob> jobCallback)
        {
            QueueEntry.ExecuteCallback = (a) =>
            {
                using (var context = _contextCallback())
                using (var job = jobCallback(context))
                {
                    a(job);
                }
            };

            return new JobPart(Service, QueueEntry);
        }
    }
}
