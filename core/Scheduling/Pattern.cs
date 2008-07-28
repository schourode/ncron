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
using System.Text.RegularExpressions;

namespace NCron.Scheduling
{
    public struct Pattern
    {
        int lower, upper, step;

        public int LowerBound
        {
            get { return this.lower; }
        }

        public int UpperBound
        {
            get { return this.upper; }
        }

        public int StepSize
        {
            get { return this.step; }
        }

        public Pattern(int lowerBound, int upperBound, int stepSize)
        {
            if (lowerBound < 0 || stepSize < 0)
                throw new ArgumentException("Negative values are not allowed for any of the pattern members.");

            if (upperBound < lowerBound)
                throw new ArgumentException("The upper bound of a pattern must be equal to or larger than the lower bound.");

            this.lower = lowerBound;
            this.upper = upperBound;
            this.step = stepSize;
        }

        public int ComputeOffset(int current, int previous, int turn, bool force)
        {
            // If out of bounds, return the offset to reach the lower bound.
            if (current < this.lower) return this.lower - current;
            if (current > this.upper) return turn - current + this.lower;

            if (current < previous) current += turn;
            int offset = step - ((current - previous) % step);

            if (offset == step && !force) return 0;

            int next = current + offset;

            if (next < turn)
            {
                if (next <= this.upper) return offset;
                return turn - current + this.lower;
            }
            else
            {
                if (turn - 1 > this.upper) return turn - current + this.lower;
                return offset;
            }
        }

        static Regex PATTERN_PARSER = new Regex(@"^(((?<lower>\d+)(-(?<upper>\d+))?)|\*)(/(?<step>\d+))?$", RegexOptions.Compiled | RegexOptions.ExplicitCapture);

        public static Pattern Parse(string text)
        {
            Match match = PATTERN_PARSER.Match(text);
            if (!match.Success) throw new FormatException(string.Format("The string {0} could not be parsed as a Pattern.", text));

            int lower, upper, step;
            lower = ParseGroup(match, "lower", -1);
            if (lower < 0)
            {
                lower = 0;
                upper = int.MaxValue;
            }
            else
            {
                upper = ParseGroup(match, "upper", lower);
            }
            step = ParseGroup(match, "step", 1);

            return new Pattern(lower, upper, step);
        }

        private static int ParseGroup(Match match, string groupName, int defaultValue)
        {
            Group group = match.Groups[groupName];
            return group.Success ? int.Parse(group.Value) : defaultValue;
        }
    }
}
