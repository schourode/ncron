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
    public class PatternCollectionTests
    {
        [RowTest]
        [Row(0, 30, false, 1)]
        [Row(1, 30, false, 0)]
        [Row(1, 30, true, 1)]
        [Row(2, 30, false, 0)]
        [Row(3, 30, false, 0)]
        [Row(4, 30, false, 1)]
        [Row(12, 30, false, 1)]
        [Row(13, 30, false, 0)]
        [Row(14, 30, false, 7)]
        [Row(21, 30, true, 10)]
        [Row(22, 30, false, 9)]
        public void TestComputeOffset(int current, int turn, bool force, int expected)
        {
            PatternCollection pc = new PatternCollection().Exact(1, 2, 3, 5, 8, 13, 21);
            Assert.AreEqual(expected, pc.ComputeOffset(current, turn, force));
        }
    }
}
