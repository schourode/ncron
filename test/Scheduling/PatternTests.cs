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

        [RowTest]
        [Row("*", 0, int.MaxValue, 1)]
        [Row("*/5", 0, int.MaxValue, 5)]
        [Row("3", 3, 3, 1)]
        [Row("10-20", 10, 20, 1)]
        [Row("10-20/2", 10, 20, 2)]
        public void TestParse(string text, int lower, int upper, int step)
        {
            Pattern p = Pattern.Parse(text);
            Assert.AreEqual(lower, p.LowerBound);
            Assert.AreEqual(upper, p.UpperBound);
            Assert.AreEqual(step, p.StepSize);
        }
    }
}
