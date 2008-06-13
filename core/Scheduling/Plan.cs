using System;

namespace NCron.Scheduling
{
    public struct Plan
    {
        public Field Minutes, Hours, Days, Months, WeekDays;

        public DateTime ComputeNextExecution(DateTime dt)
        {
            // Remove seconds and milliseconds from input.
            dt = dt.AddMilliseconds(-dt.Millisecond - 1000 * dt.Second);


            /*
            if (!Days.Matches(dt.Day) || !WeekDays.Matches(dt.DayOfWeek)) dt = dt.AddDays(1).Date;
            while (!Days.Matches(dt.Day) || !WeekDays.Matches(dt.DayOfWeek)) dt = dt.AddDays(1);

            if (!Hours.Matches(dt.Hour)) dt = dt.AddMinutes(60 - dt.Minute);
            while (!Hours.Matches(dt.Hour)) dt = dt.AddHours(1);

            while (!Minutes.Matches(dt.Minute)) dt = dt.AddMinutes(1);*/

            return dt;
        }
    }
}
