/* Copyright (c) 2008, Joern Schou-Rode <jsr@malamute.dk>

Permission to use, copy, modify, and/or distribute this software for any
purpose with or without fee is hereby granted, provided that the above
copyright notice and this permission notice appear in all copies.

THE SOFTWARE IS PROVIDED "AS IS" AND THE AUTHOR DISCLAIMS ALL WARRANTIES
WITH REGARD TO THIS SOFTWARE INCLUDING ALL IMPLIED WARRANTIES OF
MERCHANTABILITY AND FITNESS. IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR
ANY SPECIAL, DIRECT, INDIRECT, OR CONSEQUENTIAL DAMAGES OR ANY DAMAGES
WHATSOEVER RESULTING FROM LOSS OF USE, DATA OR PROFITS, WHETHER IN AN
ACTION OF CONTRACT, NEGLIGENCE OR OTHER TORTIOUS ACTION, ARISING OUT OF
OR IN CONNECTION WITH THE USE OR PERFORMANCE OF THIS SOFTWARE. */

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
