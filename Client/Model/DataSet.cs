using Newtonsoft.Json;
using System;
using System.Collections.Generic;


namespace Client.Model
{
    /// <summary>
    ///  Respresents data from a time range of a series. This is essentially a list
    ///  of DataPoints with some added metadata. This is the object returned from a query.
    ///  The DataSet contains series metadata, the start/end times for the queried range,
    ///  a list of the DataPoints and a statistics summary table. The Summary table contains
    ///  statistics for the time range (sum, mean, min, max, count, etc.)
    /// </summary>
    public class DataSet
    {
        public Series Series { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public List<DataPoint> Data { get; set; }
        public Dictionary<string, double> Summary { get; set; }
    }
}
