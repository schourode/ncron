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
using System.Collections;
using System.Collections.Generic;

namespace NCron.Framework.Scheduling
{
    public class Plan : IEnumerable<DateTime>
    {
        #region Public properties and constructors

        public PatternCollection Minutes { get; private set; }
        public PatternCollection Hours { get; private set; }
        public PatternCollection DaysOfWeek { get; private set; }
        public PatternCollection DaysOfMonth { get; private set; }
        public PatternCollection Months { get; private set; }

        public Plan()
        {
            this.Minutes = new PatternCollection();
            this.Hours = new PatternCollection();
            this.DaysOfWeek = new PatternCollection();
            this.DaysOfMonth = new PatternCollection();
            this.Months = new PatternCollection();
        }

        #endregion

        #region Algorithm for computation of new execution DateTime

        public DateTime ComputeNextExecution(DateTime previous, bool firstTime)
        {
            Queue<Field> workList = new Queue<Field>();
            workList.Enqueue(Field.Minute);
            if (firstTime)
            {
                workList.Enqueue(Field.Hour);
                workList.Enqueue(Field.Day);
                workList.Enqueue(Field.Month);
            }

            int pMin = previous.Minute, pHour = previous.Hour, pDay = previous.Day - 1, pMonth = previous.Month;

            while (workList.Count > 0)
            {
                DateTime dt = previous;
                switch (workList.Dequeue())
                {
                    case Field.Minute:
                        dt = previous.AddMinutes(Minutes.ComputeOffset(dt.Minute, pMin, 60, !firstTime));
                        if (dt.Hour != previous.Hour) workList.Enqueue(Field.Hour);
                        firstTime = true;
                        break;

                    case Field.Hour:
                        dt = dt.AddHours(Hours.ComputeOffset(dt.Hour, pHour, 24, false));
                        if (dt.Hour != previous.Hour)
                        {
                            if (dt.Day != previous.Day) workList.Enqueue(Field.Day);
                            dt = dt.AddMinutes(-dt.Minute);
                            pMin = 0;
                            workList.Enqueue(Field.Minute);
                        }
                        break;

                    case Field.Day:
                        if (DaysOfWeek.Count > 0 || DaysOfMonth.Count > 0)
                        {
                            int dow = (DaysOfWeek.Count == 0) ? int.MaxValue : DaysOfWeek.ComputeOffset((int)dt.DayOfWeek, pDay, 7, false);
                            int dom = (DaysOfMonth.Count == 0) ? int.MaxValue : DaysOfMonth.ComputeOffset(dt.Day - 1, pDay, DateTime.DaysInMonth(dt.Year, dt.Month), false);
                            dt = dt.AddDays(dow < dom ? dow : dom);
                            if (dt.Day != previous.Day)
                            {
                                if (dt.Month != previous.Month) workList.Enqueue(Field.Month);
                                dt = dt.AddHours(-dt.Hour).AddMinutes(-dt.Minute);
                                pHour = pMin = 0;
                                workList.Enqueue(Field.Hour);
                                workList.Enqueue(Field.Minute);
                            }
                        }
                        break;

                    case Field.Month:
                        dt = dt.AddMonths(Months.ComputeOffset(dt.Month, pMonth, 12, false));
                        if (dt.Month != previous.Month)
                        {
                            dt = dt.AddDays(-dt.Day).AddHours(-dt.Hour).AddMinutes(-dt.Minute);
                            pDay = pHour = pMin = 0;
                            workList.Enqueue(Field.Day);
                            workList.Enqueue(Field.Hour);
                            workList.Enqueue(Field.Minute);
                        }
                        break;
                }
                previous = dt;
            }

            return previous;
        }

        private enum Field { Minute, Hour, Day, Month }

        #endregion

        #region IEnumerable and IEnumerable<DateTime> implementation

        public IEnumerator<DateTime> GetEnumerator(DateTime start)
        {
            return new PlanEnumerator(this, start);
        }

        public IEnumerator<DateTime> GetEnumerator()
        {
            return GetEnumerator(DateTime.Now);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator(DateTime.Now);
        }

        private class PlanEnumerator : IEnumerator<DateTime>
        {
            Plan plan;
            DateTime start, current;

            public PlanEnumerator(Plan plan, DateTime start)
            {
                this.plan = plan;
                this.start = start;
                this.current = DateTime.MinValue;
            }

            public DateTime Current
            {
                get { return this.current; }
            }

            object IEnumerator.Current
            {
                get { return this.current; }
            }

            public bool MoveNext()
            {
                this.current = (this.current == DateTime.MinValue)
                    ? this.plan.ComputeNextExecution(this.start, true)
                    : this.plan.ComputeNextExecution(this.current, false);
                return true;
            }

            public void Reset()
            {
                this.current = DateTime.MinValue;
            }

            public void Dispose()
            {
            }
        }

        #endregion
    }
}
