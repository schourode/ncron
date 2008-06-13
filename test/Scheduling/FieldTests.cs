using System;

using MbUnit.Framework;

namespace NCron.Scheduling
{
    [TestFixture]
    public class FieldTests
    {
        [RowTest]
        [Row("")]
        [Row("*")]
        [Row(null)]
        public void TestParseAny(string txt)
        {
            Assert.AreEqual(Field.Any, Field.Parse(txt));
        }

        [RowTest]
        [Row("0", 0)]
        [Row("7", 7)]
        [Row("31", 31)]
        public void TestParseSingle(string txt, int cnst)
        {
            Assert.AreEqual(Field.List(cnst), Field.Parse(txt));
        }

        [RowTest]
        [Row("0-5", 0, 5)]
        [Row("12-12", 12, 12)]
        [Row("5-0", 5, 0, ExpectedException = typeof(ArgumentException))]
        [Row("0-5-9", 0, 9, ExpectedException = typeof(FormatException))]
        public void TestParseRange(string txt, int from, int to)
        {
            Assert.AreEqual(Field.Range(from, to), Field.Parse(txt));
        }

        [RowTest]
        [Row(0)]
        [Row(1)]
        [Row(31)]
        public void TestAnyMatches(int value)
        {
            int ceil;
            Assert.IsTrue(Field.Any.Matches(value, out ceil));
            Assert.AreEqual(value, ceil);
        }
    }
}
