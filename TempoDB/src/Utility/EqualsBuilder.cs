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
                // TODO: Implement array case
                isEqual = false;
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

        public EqualsBuilder Append(float lhs, float rhs)
        {
            if (isEqual == false)
            {
                return this;
            }
            isEqual = (lhs == rhs);
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

        public bool IsEquals()
        {
            return isEqual;
        }
    }
}
