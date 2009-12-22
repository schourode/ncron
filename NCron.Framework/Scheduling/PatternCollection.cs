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
using System.Collections.Generic;

namespace NCron.Framework.Scheduling
{
    public class PatternCollection : ICollection<Pattern>
    {
        List<Pattern> patterns = new List<Pattern>();

        public int ComputeOffset(int current, int previous, int turn, bool force)
        {
            // If no patterns specified, anything will do!
            if (patterns.Count == 0) return force ? 1 : 0;

            // Try out each contained pattern, and return the minimum value.
            int min = int.MaxValue;
            foreach (Pattern pattern in this.patterns)
            {
                int cur = pattern.ComputeOffset(current, previous, turn, force);
                if (cur < min) min = cur;
            }
            return min;
        }

        #region Helper methods for adding patterns to the collection - supports chaining!

        public PatternCollection Exact(params int[] values)
        {
            foreach (int val in values)
                this.patterns.Add(new Pattern(val, val, 1));
            return this;
        }

        public PatternCollection Steps(int stepSize)
        {
            this.patterns.Add(new Pattern(0, int.MaxValue, stepSize));
            return this;
        }

        public PatternCollection Between(int lowerBound, int upperBound)
        {
            this.patterns.Add(new Pattern(lowerBound, upperBound, 1));
            return this;
        }

        public PatternCollection StepsBetween(int lowerBound, int upperBound, int stepSize)
        {
            this.patterns.Add(new Pattern(lowerBound, upperBound, stepSize));
            return this;
        }

        #endregion

        #region ICollection<Pattern> implementation.

        public void Add(Pattern item)
        {
            patterns.Add(item);
        }

        public void Clear()
        {
            patterns.Clear();
        }

        public bool Contains(Pattern item)
        {
            return patterns.Contains(item);
        }

        public void CopyTo(Pattern[] array, int arrayIndex)
        {
            patterns.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return patterns.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(Pattern item)
        {
            return patterns.Remove(item);
        }

        public IEnumerator<Pattern> GetEnumerator()
        {
            return patterns.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return patterns.GetEnumerator();
        }

        #endregion
    }
}
