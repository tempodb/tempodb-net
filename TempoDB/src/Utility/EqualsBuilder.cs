using System;

namespace TempoDB.Utility
{

    public class EqualsBuilder
    {
        private bool isEqual;

        public EqualsBuilder()
        {
            isEqual = true;
        }

        public EqualsBuilder AppendSuper(bool superEquals)
        {
            if(isEqual == false)
            {
                return this;
            }
            isEqual = superEquals;
            return this;
        }

        public EqualsBuilder Append(Object lhs, Object rhs)
        {
            if(isEqual == false)
            {
                return this;
            }

            if(lhs == rhs)
            {
                return this;
            }

            if(lhs == null || rhs == null)
            {
                isEqual = false;
                return this;
            }

            Type lhsClass = lhs.GetType();
            if(!lhsClass.IsArray)
            {
                // Simple case, compare the elements
                isEqual = lhs.Equals(rhs);
            }
            else
            {
                EnsureArraysSameDimension(lhs, rhs);
                if(isEqual == false)
                {
                    return this;
                }

                if(lhs is long[])
                {
                    Append((long[])lhs, rhs as long[]);
                }
                else if(lhs is int[])
                {
                    Append((int[])lhs, rhs as int[]);
                }
                else if(lhs is short[])
                {
                    Append((short[])lhs, rhs as short[]);
                }
                else if(lhs is char[])
                {
                    Append((char[])lhs, rhs as char[]);
                }
                else if(lhs is byte[])
                {
                    Append((byte[])lhs, rhs as byte[]);
                }
                else if(lhs is double[])
                {
                    Append((double[])lhs, rhs as double[]);
                }
                else if(lhs is float[])
                {
                    Append((float[])lhs, rhs as float[]);
                }
                else if(lhs is bool[])
                {
                    Append((bool[])lhs, rhs as bool[]);
                }
                else if(lhs is object[])
                {
                    Append((object[])lhs, rhs as object[]);
                }
                else
                {
                    CompareArrays(lhs, rhs, 0, 0);
                }
            }
            return this;
        }

        public EqualsBuilder Append(long lhs, long rhs)
        {
            if (isEqual == false)
            {
                return this;
            }
            isEqual = (lhs == rhs);
            return this;
        }

        public EqualsBuilder Append(int lhs, int rhs)
        {
            if (isEqual == false)
            {
                return this;
            }
            isEqual = (lhs == rhs);
            return this;
        }

        public EqualsBuilder Append(short lhs, short rhs)
        {
            if (isEqual == false)
            {
                return this;
            }
            isEqual = (lhs == rhs);
            return this;
        }

        public EqualsBuilder Append(char lhs, char rhs)
        {
            if (isEqual == false)
            {
                return this;
            }
            isEqual = (lhs == rhs);
            return this;
        }

        public EqualsBuilder Append(byte lhs, byte rhs)
        {
            if (isEqual == false)
            {
                return this;
            }
            isEqual = (lhs == rhs);
            return this;
        }

        public EqualsBuilder Append(double lhs, double rhs)
        {
            if (isEqual == false)
            {
                return this;
            }
            isEqual = (lhs == rhs);
            return this;
        }

        public EqualsBuilder Append(double lhs, double rhs, double epsilon)
        {
            if (isEqual == false)
            {
                return this;
            }
            isEqual = Math.Abs(lhs - rhs) < epsilon;
            return this;
        }

        public EqualsBuilder Append(float lhs, float rhs)
        {
            if (isEqual == false)
            {
                return this;
            }
            isEqual = (lhs == rhs);
            return this;
        }

        public EqualsBuilder Append(float lhs, float rhs, float epsilon)
        {
            if (isEqual == false)
            {
                return this;
            }
            isEqual = Math.Abs(lhs - rhs) < epsilon;
            return this;
        }

        public EqualsBuilder Append(bool lhs, bool rhs)
        {
            if (isEqual == false)
            {
                return this;
            }
            isEqual = (lhs == rhs);
            return this;
        }

        public EqualsBuilder Append(Object[] lhs, Object[] rhs)
        {
            if (isEqual == false)
            {
                return this;
            }
            if (lhs == rhs)
            {
                return this;
            }
            if (lhs == null || rhs == null)
            {
                isEqual = false;
                return this;
            }
            if (lhs.Length != rhs.Length)
            {
                isEqual = false;
                return this;
            }
            for (int i = 0; i < lhs.Length && isEqual; ++i)
            {
                if (lhs[i] != null)
                {
                    Type lhsClass = lhs[i].GetType();
                    if (!lhsClass.IsInstanceOfType(rhs[i]))
                    {
                        isEqual = false; //If the types don't match, not equal
                        break;
                    }
                }
                Append(lhs[i], rhs[i]);
            }
            return this;
        }

        public EqualsBuilder Append(long[] lhs, long[] rhs)
        {
            if (isEqual == false)
            {
                return this;
            }
            if (lhs == rhs)
            {
                return this;
            }
            if (lhs == null || rhs == null)
            {
                isEqual = false;
                return this;
            }
            if (lhs.Length != rhs.Length)
            {
                isEqual = false;
                return this;
            }
            for (int i = 0; i < lhs.Length && isEqual; ++i)
            {
                Append(lhs[i], rhs[i]);
            }
            return this;
        }

        public EqualsBuilder Append(int[] lhs, int[] rhs)
        {
            if (isEqual == false)
            {
                return this;
            }
            if (lhs == rhs)
            {
                return this;
            }
            if (lhs == null || rhs == null)
            {
                isEqual = false;
                return this;
            }
            if (lhs.Length != rhs.Length)
            {
                isEqual = false;
                return this;
            }
            for (int i = 0; i < lhs.Length && isEqual; ++i)
            {
                Append(lhs[i], rhs[i]);
            }
            return this;
        }

        public EqualsBuilder Append(short[] lhs, short[] rhs)
        {
            if (isEqual == false)
            {
                return this;
            }
            if (lhs == rhs)
            {
                return this;
            }
            if (lhs == null || rhs == null)
            {
                isEqual = false;
                return this;
            }
            if (lhs.Length != rhs.Length)
            {
                isEqual = false;
                return this;
            }
            for (int i = 0; i < lhs.Length && isEqual; ++i)
            {
                Append(lhs[i], rhs[i]);
            }
            return this;
        }

        public EqualsBuilder Append(char[] lhs, char[] rhs)
        {
            if (isEqual == false)
            {
                return this;
            }
            if (lhs == rhs)
            {
                return this;
            }
            if (lhs == null || rhs == null)
            {
                isEqual = false;
                return this;
            }
            if (lhs.Length != rhs.Length)
            {
                isEqual = false;
                return this;
            }
            for (int i = 0; i < lhs.Length && isEqual; ++i)
            {
                Append(lhs[i], rhs[i]);
            }
            return this;
        }

        public EqualsBuilder Append(byte[] lhs, byte[] rhs)
        {
            if (isEqual == false)
            {
                return this;
            }
            if (lhs == rhs)
            {
                return this;
            }
            if (lhs == null || rhs == null)
            {
                isEqual = false;
                return this;
            }
            if (lhs.Length != rhs.Length)
            {
                isEqual = false;
                return this;
            }
            for (int i = 0; i < lhs.Length && isEqual; ++i)
            {
                Append(lhs[i], rhs[i]);
            }
            return this;
        }

        public EqualsBuilder Append(double[] lhs, double[] rhs)
        {
            if (isEqual == false)
            {
                return this;
            }
            if (lhs == rhs)
            {
                return this;
            }
            if (lhs == null || rhs == null)
            {
                isEqual = false;
                return this;
            }
            if (lhs.Length != rhs.Length)
            {
                isEqual = false;
                return this;
            }
            for (int i = 0; i < lhs.Length && isEqual; ++i)
            {
                Append(lhs[i], rhs[i]);
            }
            return this;
        }

        public EqualsBuilder Append(float[] lhs, float[] rhs)
        {
            if (isEqual == false)
            {
                return this;
            }
            if (lhs == rhs)
            {
                return this;
            }
            if (lhs == null || rhs == null)
            {
                isEqual = false;
                return this;
            }
            if (lhs.Length != rhs.Length)
            {
                isEqual = false;
                return this;
            }
            for (int i = 0; i < lhs.Length && isEqual; ++i)
            {
                Append(lhs[i], rhs[i]);
            }
            return this;
        }

        public EqualsBuilder Append(bool[] lhs, bool[] rhs)
        {
            if (isEqual == false)
            {
                return this;
            }
            if (lhs == rhs)
            {
                return this;
            }
            if (lhs == null || rhs == null)
            {
                isEqual = false;
                return this;
            }
            if (lhs.Length != rhs.Length)
            {
                isEqual = false;
                return this;
            }
            for (int i = 0; i < lhs.Length && isEqual; ++i)
            {
                Append(lhs[i], rhs[i]);
            }
            return this;
        }

        public bool IsEquals()
        {
            return isEqual;
        }

        private void EnsureArraysSameDimension(object lhs, object rhs)
        {
            bool isArray1 = lhs is Array;
            bool isArray2 = rhs is Array;

            if(isArray1 != isArray2)
            {
                isEqual = false;
                return;
            }

            Array array1 = (Array)lhs;
            Array array2 = (Array)rhs;

            if(array1.Rank != array2.Rank)
            {
                isEqual = false;
            }

            if(array1.Length != array2.Length)
            {
                isEqual = false;
            }
        }

        private void CompareArrays(object parray1, object parray2, int prank, int pindex)
        {
            if (isEqual == false)
            {
                return;
            }
            if (parray1 == parray2)
            {
                return;
            }
            if (parray1 == null || parray2 == null)
            {
                isEqual = false;
                return;
            }

            Array array1 = (Array)parray1;
            Array array2 = (Array)parray2;
            int rank1 = array1.Rank;
            int rank2 = array2.Rank;

            if (rank1 != rank2)
            {
                isEqual = false;
                return;
            }

            int size1 = array1.GetLength(prank);
            int size2 = array2.GetLength(prank);

            if (size1 != size2)
            {
                isEqual = false;
                return;
            }

            if (prank == rank1 - 1)
            {
                int index = 0;

                int min = pindex;
                int max = min + size1;


                var enumerator1 = array1.GetEnumerator();
                var enumerator2 = array2.GetEnumerator();
                while (enumerator1.MoveNext())
                {
                    if (isEqual == false)
                    {
                        return;
                    }
                    enumerator2.MoveNext();


                    if ((index >= min) && (index < max))
                    {
                        object obj1 = enumerator1.Current;
                        object obj2 = enumerator2.Current;

                        bool isArray1 = obj1 is Array;
                        bool isArray2 = obj2 is Array;

                        if (isArray1 != isArray2)
                        {
                            isEqual = false;
                            return;
                        }

                        if (isArray1)
                        {
                            CompareArrays(obj1, obj2, 0, 0);
                        }
                        else
                        {
                            Append(obj1, obj2);
                        }
                    }

                    index++;
                }
            }
            else
            {
                int mux = 1;

                int currentRank = rank1 - 1;

                do
                {
                    int sizeMux1 = array1.GetLength(currentRank);
                    int sizeMux2 = array2.GetLength(currentRank);

                    if (sizeMux1 != sizeMux2)
                    {
                        isEqual = false;
                        return;
                    }

                    mux *= sizeMux1;
                    currentRank--;
                } while (currentRank > prank);
            }
        }
    }
}
