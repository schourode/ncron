/*
 * Copyright 2008, 2009 Joern Schou-Rode
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
using System.Text.RegularExpressions;

namespace NCron.Framework.Scheduling
{
    /// <summary>
    /// Represents a numeric pattern used to match a single field in a <see cref="DateTime"/> (second, minute, etc.).
    /// The pattern is defined by a lower and an upper bound, combined with a step length, providing the ability to match only every Nth value.
    /// </summary>
    public struct Pattern
    {
        int lower, upper, step;

        /// <summary>
        /// Gets the lower bound of the pattern as defined at construction time.
        /// </summary>
        public int LowerBound
        {
            get { return this.lower; }
        }

        /// <summary>
        /// Gets the upper bound of the pattern as defined at construction time.
        /// </summary>
        public int UpperBound
        {
            get { return this.upper; }
        }

        /// <summary>
        /// Gets the size of the steps between each matching value.
        /// </summary>
        public int StepSize
        {
            get { return this.step; }
        }

        /// <summary>
        /// Creates a new <see cref="Pattern"/> with the specified bounds and step size.
        /// </summary>
        /// <param name="lowerBound">the lower bound of the numeric pattern to be matched</param>
        /// <param name="upperBound">the upper bound of the numeric pattern to be matched</param>
        /// <param name="stepSize">the size of the steps between each matching value</param>
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

        /// <summary>
        /// Computes the offset from a specified numeric value, to the next value matched by the pattern.
        /// </summary>
        /// <param name="current">the current value - this might not be a legal value of this pattern</param>
        /// <param name="previous">the last known acceptable value</param>
        /// <param name="turn">the upper bound of the current series of values - if no legal value is found within this bound, searching continues from value 0</param>
        /// <param name="force">if set to true, the method will never return the offset 0, even if the current value is matched by the pattern</param>
        /// <returns></returns>
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

        /// <summary>
        /// Converts the specified string representation of a numeric pattern into its <see cref="Pattern"/> equivalent.
        /// </summary>
        /// <remarks>
        /// The syntax follows that of the typical Unix "cron" implementation:
        /// - An asterix matches all values.
        /// - You can use dashes to specify ranges.
        /// - You can use forward slash to specify a repeating range.
        /// </remarks>
        /// <param name="text">a string containing a pattern to convert</param>
        /// <returns>the <see cref="Pattern"/> equivalent to the inputted string</returns>
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
