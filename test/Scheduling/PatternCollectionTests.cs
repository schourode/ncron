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

using MbUnit.Framework;

namespace NCron.Scheduling
{
    [TestFixture]
    public class PatternCollectionTests
    {
        [RowTest]
        [Row(0, 0, 30, false, 1)]
        [Row(1, 0, 30, false, 0)]
        [Row(1, 0, 30, true, 1)]
        [Row(2, 0, 30, false, 0)]
        [Row(3, 0, 30, false, 0)]
        [Row(4, 0, 30, false, 1)]
        [Row(12, 0, 30, false, 1)]
        [Row(13, 0, 30, false, 0)]
        [Row(14, 0, 30, false, 7)]
        [Row(21, 0, 30, true, 10)]
        [Row(22, 0, 30, false, 9)]
        public void TestComputeOffset(int current, int previous, int turn, bool force, int expected)
        {
            PatternCollection pc = new PatternCollection().Exact(1, 2, 3, 5, 8, 13, 21);
            Assert.AreEqual(expected, pc.ComputeOffset(current, previous, turn, force));
        }
    }
}
