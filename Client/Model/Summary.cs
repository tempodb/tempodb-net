using System;
using System.Collections.Generic;


namespace Client.Model
{
    /// <summary>
    ///  Respresents summary statistics from a time range of a series. The Summary table contains
    ///  statistics for the time range (sum, mean, min, max, count, etc.)
    /// </summary>
    public class Summary : Dictionary<string, double>
    {
    }
}
