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

using MbUnit.Framework;

namespace NCron.Scheduling
{
    [TestFixture]
    public class PatternTests
    {
        [RowTest]
        [Row(5, 5, 1, 2, 0, 12, false, 3)]
        [Row(5, 5, 1, 5, 0, 12, false, 0)]
        [Row(5, 5, 1, 5, 0, 12, true, 12)]
        [Row(5, 5, 1, 8, 0, 12, false, 9)]
        [Row(2, 5, 1, 0, 0, 24, false, 2)]
        [Row(2, 5, 1, 2, 0, 24, false, 0)]
        [Row(2, 5, 1, 2, 0, 24, true, 1)]
        [Row(2, 5, 1, 4, 0, 24, false, 0)]
        [Row(2, 5, 1, 7, 0, 24, false, 19)]
        [Row(0, 9, 3, 0, 0, 31, false, 0)]
        [Row(0, 9, 3, 3, 0, 31, false, 0)]
        [Row(0, 9, 3, 3, 0, 31, true, 3)]
        [Row(0, 9, 3, 8, 6, 31, false, 1)]
        [Row(0, 9, 3, 8, 7, 31, false, 23)]
        [Row(0, 9, 3, 20, 0, 31, true, 11)]
        [Row(0, int.MaxValue, 10, 0, 0, 24, false, 0)]
        [Row(0, int.MaxValue, 10, 0, 0, 24, true, 10)]
        [Row(0, int.MaxValue, 10, 1, 0, 24, false, 9)]
        [Row(0, int.MaxValue, 10, 21, 20, 24, false, 9)]
        public void TestComputeOffset(int lower, int upper, int step, int current, int previous, int turn, bool force, int expected)
        {
            Pattern p = new Pattern(lower, upper, step);
            Assert.AreEqual(expected, p.ComputeOffset(current, previous, turn, force));
        }
    }
}
