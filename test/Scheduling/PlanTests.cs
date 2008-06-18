// Copyright (c) 2007-2008, Joern Schou-Rode <jsr@malamute.dk>
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions
// are met:
// 
// * Redistributions of source code must retain the above copyright
//   notice, this list of conditions and the following disclaimer.
// 
// * Redistributions in binary form must reproduce the above copyright
//   notice, this list of conditions and the following disclaimer in the
//   documentation and/or other materials provided with the distribution.
// 
// * Neither the name of the NCron nor the names of its contributors may
//   be used to endorse or promote products derived from this software
//   without specific prior written permission.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
// ``AS IS'' AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
// LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
// A PARTICULAR PURPOSE ARE DISCLAIMED.  IN NO EVENT SHALL THE REGENTS OR
// CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
// EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
// PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
// PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
// LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
// NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using System;
using System.Collections.Generic;

using MbUnit.Framework;

namespace NCron.Scheduling
{
    [TestFixture]
    public class PlanTests
    {
        private void AssertEnumerationResults(Plan plan, DateTime start, DateTime[] expected)
        {
            IEnumerator<DateTime> e = plan.GetEnumerator(start);
            foreach (DateTime dt in expected)
            {
                e.MoveNext();
                Assert.AreEqual(dt, e.Current);
            }
        }

        [Test]
        public void TestComputeNextExecution_StepHours()
        {
            Plan plan = new Plan();
            plan.Hours.Steps(10);
            plan.Minutes.Exact(0);

            AssertEnumerationResults(plan, new DateTime(2000, 1, 1, 0, 0, 0), new DateTime[] {
                new DateTime(2000, 1, 1, 0, 0, 0),
                new DateTime(2000, 1, 1, 10, 0, 0),
                new DateTime(2000, 1, 1, 20, 0, 0),
                new DateTime(2000, 1, 2, 6, 0, 0),
                new DateTime(2000, 1, 2, 16, 0, 0)
            });
        }

        [Test]
        public void TestComputeNextExecution_StepMinuteRange()
        {
            Plan plan = new Plan();
            plan.Minutes.StepsBetween(0, 15, 5);

            AssertEnumerationResults(plan, new DateTime(2000, 1, 1, 0, 0, 0), new DateTime[] {
                new DateTime(2000, 1, 1, 0, 0, 0),
                new DateTime(2000, 1, 1, 0, 5, 0),
                new DateTime(2000, 1, 1, 0, 10, 0),
                new DateTime(2000, 1, 1, 0, 15, 0),
                new DateTime(2000, 1, 1, 1, 0, 0),
                new DateTime(2000, 1, 1, 1, 5, 0),
                new DateTime(2000, 1, 1, 1, 10, 0),
                new DateTime(2000, 1, 1, 1, 15, 0)
            });
        }

        [Test]
        public void TestComputeNextExecution_Daily()
        {
            Plan plan = new Plan();
            plan.DaysOfWeek.Between(1, 5);
            plan.Hours.Exact(3);
            plan.Minutes.Exact(0);

            AssertEnumerationResults(plan, new DateTime(2000, 1, 1, 0, 0, 0), new DateTime[] {
                new DateTime(2000, 1, 3, 3, 0, 0),
                new DateTime(2000, 1, 4, 3, 0, 0),
                new DateTime(2000, 1, 5, 3, 0, 0),
                new DateTime(2000, 1, 6, 3, 0, 0),
                new DateTime(2000, 1, 7, 3, 0, 0),
                new DateTime(2000, 1, 10, 3, 0, 0)
            });
        }

        [Test]
        public void TestComputeNextExecution_StepMonths()
        {
            Plan plan = new Plan();
            plan.Months.Steps(2);
            plan.DaysOfMonth.Exact(0);
            plan.Minutes.Exact(0);
            plan.Hours.Exact(0);

            AssertEnumerationResults(plan, new DateTime(2000, 1, 1, 0, 0, 0), new DateTime[] {
                new DateTime(2000, 1, 1, 0, 0, 0),
                new DateTime(2000, 3, 1, 0, 0, 0),
                new DateTime(2000, 5, 1, 0, 0, 0),
                new DateTime(2000, 7, 1, 0, 0, 0)
            });
        }
    }
}
