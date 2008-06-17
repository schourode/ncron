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

namespace NCron.Scheduling
{
    public class PatternCollection : ICollection<Pattern>
    {
        List<Pattern> patterns = new List<Pattern>();

        public int ComputeOffset(int current, int turn, bool force)
        {
            // If no patterns specified, anything will do!
            if (patterns.Count == 0) return force ? 1 : 0;

            // Try out each contained pattern, and return the minimum value.
            int min = int.MaxValue;
            foreach (Pattern pattern in this.patterns)
            {
                int cur = pattern.ComputeOffset(current, turn, force);
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
