using System;
using System.Collections.Generic;

namespace NCron.Scheduling
{
    public struct Field
    {
        int[] values;

        public bool Matches(int value, out int ceil)
        {
            // Handle the "Any" case, where anything is matched by the field.
            if (values == null || values.Length == 0)
            {
                ceil = value;
                return true;
            }

            ceil = -1;
            for (int i = values.Length - 1; i >= 0; i--)
            {
                int current = values[i];
                if (current < value) break;
                ceil = current;
                if (current == value) return true;
            }

            return false;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Field)) return false;
            Field other = (Field)obj;
            if (this.values == other.values) return true;
            if (this.values.Length != other.values.Length) return false;
            for (int i = 0; i < values.Length; i++)
                if (this.values[i] != other.values[i]) return false;
            return true;
        }

        public static readonly Field Any = new Field();

        public static Field List(params int[] list)
        {
            Array.Sort<int>(list);
            Field f = new Field();
            f.values = list;
            return f;
        }

        public static Field Range(int from, int to)
        {
            if (to < from) throw new ArgumentException(string.Format("Illegal range specfied: {0}-{1}", from, to));

            Field f = new Field();
            f.values = new int[to - from + 1];
            for (int i = 0; i < f.values.Length; i++)
                f.values[i] = from + i;
            return f;
        }

        public static Field Parse(string txt)
        {
            // Handle "Any" cases (null, empty, *) and remove white-space.
            if (txt == null) return Any;
            txt = txt.Replace(" ", string.Empty);
            if (txt.Length == 0 || txt == "*") return Any;

            string[] range = txt.Split('-');
            if (range.Length > 2) throw new FormatException(string.Format("The specified range \"{0}\" contains too many hyphens.", txt));
            if (range.Length == 2) return Range(int.Parse(range[0]), int.Parse(range[1]));

            string[] listStr = txt.Split(',');
            int[] listInt = new int[listStr.Length];
            for (int i = 0; i < listStr.Length; i++)
                listInt[i] = int.Parse(listStr[i]);
            return List(listInt);
        }
    }
}
