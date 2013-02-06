

namespace TempoDB
{
    public class Result<T>
    {
        private T value;
        private bool success;

        public T Value
        {
            get { return value; }
            private set { this.value = value; }
        }

        public bool Success
        {
            get { return success; }
            private set { this.success = value; }
        }

        public Result(T value, bool success)
        {
            this.value = value;
            this.success = success;
        }
    }
}
