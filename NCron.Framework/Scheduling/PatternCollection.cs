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

namespace NCron.Scheduling
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
