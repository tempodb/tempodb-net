namespace Client.Model
{
    public static class FoldingFunction
    {
        public static readonly string Count = "count";
        public static readonly string Max = "max";
        public static readonly string Mean = "mean";
        public static readonly string Min = "min";
        public static readonly string StandardDeviation = "stddev";
        public static readonly string Sum = "sum";
        public static readonly string SumOfSquares = "ss";
    }

    public static class IntervalParameter
    {
        public static string Days(int days)
        {
            return string.Format("{0}day", days);
        }

        public static string Hours(int hours)
        {
            return string.Format("{0}hour", hours);
        }

        public static string Minutes(int minutes)
        {
            return string.Format("{0}min", minutes);
        }

        public static string Months(int months)
        {
            return string.Format("{0}month", months);
        }

        public static string Years(int years)
        {
            return string.Format("{0}year", years);
        }

        public static string Raw()
        {
            return "raw";
        }

    }

    public static class SeriesProperty
    {
        public static readonly string Id = "id";
        public static readonly string Key = "key";
    }

    public static class QueryStringParameter
    {
        public static readonly string Id = "id";
        public static readonly string Key = "key";
        public static readonly string Start = "start";
        public static readonly string End = "end";
        public static readonly string Function = "function";
        public static readonly string Interval = "interval";
    }
}
