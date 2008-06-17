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

namespace NCron.Scheduling
{
    public struct Pattern
    {
        int lower, upper, step;

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

        public int ComputeOffset(int current, int turn, bool force)
        {
            if (current < this.lower) return this.lower - current;
            if (current > this.upper) return turn - current + this.lower;

            if (!force) return 0;

            int next = current + step;

            if (next < turn)
            {
                if (next <= this.upper) return step;
                return turn - current + this.lower;
            }
            else
            {
                if (turn - 1 > this.upper) return turn - current + this.lower;
                return step;
            }
        }
    }
}
