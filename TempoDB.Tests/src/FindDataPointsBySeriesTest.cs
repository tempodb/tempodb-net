using Moq;
using NodaTime;
using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using TempoDB.Exceptions;


namespace TempoDB.Tests
{
    [TestFixture]
    class FindDataPointsBySeriesTest
    {
        private static DateTimeZone zone = DateTimeZone.Utc;
        private static Series series = new Series("key1");

        private static string json = @"{
            ""tz"":""UTC"",
            ""data"":[
                {
                    ""interval"":{""start"":""2012-01-01T00:00:00.000+00:00"",""end"":""2012-01-02T00:00:00.000+00:00""},
                    ""found"":{""t"":""2012-01-01T00:00:01.000+00:00"",""v"":12.34}
                }
            ],
            ""predicate"":{
                ""function"":""max"",
                ""period"":""PT1M""
            }
        }";

        private static string json1 = @"{
            ""data"":[
                {
                    ""interval"":{""start"":""2012-03-27T00:00:00.000+00:00"",""end"":""2012-03-27T00:01:00.000+00:00""},
                    ""found"":{""t"":""2012-03-27T00:00:00.000-05:00"",""v"":12.34}
                },
                {
                    ""interval"":{""start"":""2012-03-27T00:01:00.000+00:00"",""end"":""2012-03-27T00:02:00.000+00:00""},
                    ""found"":{""t"":""2012-03-27T00:01:00.000-05:00"",""v"":23.45}
                }
            ],
            ""tz"":""UTC"",
            ""predicate"":{
                ""function"":""max"",
                ""period"":""PT1M""
            }
        }";

        private static string json2 = @"{
            ""data"":[
                {
                    ""interval"":{""start"":""2012-03-27T00:02:00.000+00:00"",""end"":""2012-03-27T00:03:00.000+00:00""},
                    ""found"":{""t"":""2012-03-27T00:02:00.000-05:00"",""v"":34.56}
                }
            ],
            ""tz"":""UTC"",
            ""predicate"":{
                ""function"":""max"",
                ""period"":""PT1M""
            }
        }";

        private static string jsonTz = @"{
            ""data"":[
                {
                    ""interval"":{""start"":""2012-03-27T00:00:00.000+00:00"",""end"":""2012-03-27T00:01:00.000+00:00""},
                    ""found"":{""t"":""2012-03-27T00:00:00.000-05:00"",""v"":12.34}
                },
                {
                    ""interval"":{""start"":""2012-03-27T00:01:00.000+00:00"",""end"":""2012-03-27T00:02:00.000+00:00""},
                    ""found"":{""t"":""2012-03-27T00:01:00.000-05:00"",""v"":23.45}
                }
            ],
            ""tz"":""America/Chicago"",
            ""predicate"":{
                ""function"":""max"",
                ""period"":""PT1M""
            }
        }";

        private static ZonedDateTime start = zone.AtStrictly(new LocalDateTime(2012, 3, 27, 0, 0, 0));
        private static ZonedDateTime end = zone.AtStrictly(new LocalDateTime(2012, 3, 28, 0, 0, 0));
        private static Interval interval = new Interval(start.ToInstant(), end.ToInstant());
        private static Predicate predicate = new Predicate(Period.FromMinutes(1), "max");

        private static DateTimeZone utc = DateTimeZone.Utc;
        private static ZonedDateTime dt0 = utc.AtStrictly(new LocalDateTime(2012, 3, 27, 0, 0, 0));
        private static ZonedDateTime dt1 = utc.AtStrictly(new LocalDateTime(2012, 3, 27, 0, 1, 0));
        private static ZonedDateTime dt2 = utc.AtStrictly(new LocalDateTime(2012, 3, 27, 0, 2, 0));
        private static ZonedDateTime dt3 = utc.AtStrictly(new LocalDateTime(2012, 3, 27, 0, 3, 0));

        [Test]
        public void SmokeTest()
        {
            var response = TestCommon.GetResponse(200, json1);
            var client = TestCommon.GetClient(response);

            var result = client.FindDataPoints(series, interval, predicate);

            var expected = new List<DataPointFound> {
                new DataPointFound(new Interval(dt0.ToInstant(), dt1.ToInstant()), new DataPoint(zone.AtStrictly(new LocalDateTime(2012, 3, 27, 5, 0, 0)), 12.34)),
                new DataPointFound(new Interval(dt1.ToInstant(), dt2.ToInstant()), new DataPoint(zone.AtStrictly(new LocalDateTime(2012, 3, 27, 5, 1, 0)), 23.45))
            };
            var output = new List<DataPointFound>();
            foreach(DataPointFound dp in result)
            {
                output.Add(dp);
            }

            Assert.AreEqual(expected, output);
        }

        [Test]
        public void SmokeTestTz()
        {
            var zone = DateTimeZoneProviders.Tzdb["America/Chicago"];
            var response = TestCommon.GetResponse(200, jsonTz);
            var client = TestCommon.GetClient(response);

            var result = client.FindDataPoints(series, interval, predicate);

            var expected = new List<DataPointFound> {
                new DataPointFound(new Interval(dt0.ToInstant(), dt1.ToInstant()), new DataPoint(zone.AtStrictly(new LocalDateTime(2012, 3, 27, 0, 0, 0)), 12.34)),
                new DataPointFound(new Interval(dt1.ToInstant(), dt2.ToInstant()), new DataPoint(zone.AtStrictly(new LocalDateTime(2012, 3, 27, 0, 1, 0)), 23.45))
            };
            var output = new List<DataPointFound>();
            foreach(DataPointFound dp in result)
            {
                output.Add(dp);
            }

            Assert.AreEqual(expected, output);
        }

        [Test]
        public void MultipleSegmentSmokeTest()
        {
            var response1 = TestCommon.GetResponse(200, json1);
            response1.Headers.Add(new Parameter {
                Name = "Link",
                Value = "</v1/series/key/key1/find/?start=2012-03-27T00:02:00.000-05:00&end=2012-03-28>; rel=\"next\""
            });
            var response2 = TestCommon.GetResponse(200, json2);

            var calls = 0;
            RestResponse[] responses = { response1, response2 };
            var mockclient = new Mock<RestClient>();
            mockclient.Setup(cl => cl.Execute(It.IsAny<RestRequest>())).Returns(() => responses[calls]).Callback(() => calls++);

            var client = TestCommon.GetClient(mockclient.Object);
            var result = client.FindDataPoints(series, interval, predicate);

            var expected = new List<DataPointFound> {
                new DataPointFound(new Interval(dt0.ToInstant(), dt1.ToInstant()), new DataPoint(zone.AtStrictly(new LocalDateTime(2012, 3, 27, 5, 0, 0)), 12.34)),
                new DataPointFound(new Interval(dt1.ToInstant(), dt2.ToInstant()), new DataPoint(zone.AtStrictly(new LocalDateTime(2012, 3, 27, 5, 1, 0)), 23.45)),
                new DataPointFound(new Interval(dt2.ToInstant(), dt3.ToInstant()), new DataPoint(zone.AtStrictly(new LocalDateTime(2012, 3, 27, 5, 2, 0)), 34.56))
            };
            var output = new List<DataPointFound>();
            foreach(DataPointFound dp in result)
            {
                output.Add(dp);
            }

            Assert.AreEqual(expected, output);
        }

        [Test]
        public void RequestMethod()
        {
            var response = TestCommon.GetResponse(200, json);
            var mockclient = TestCommon.GetMockRestClient(response);
            var client = TestCommon.GetClient(mockclient.Object);

            client.FindDataPoints(series, interval, predicate);

            mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => req.Method == Method.GET)));
        }

        [Test]
        public void RequestUrl()
        {
            var response = TestCommon.GetResponse(200, json);
            var mockclient = TestCommon.GetMockRestClient(response);
            var client = TestCommon.GetClient(mockclient.Object);

            client.FindDataPoints(series, interval, predicate);

            mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => req.Resource == "/{version}/series/key/{key}/find/")));
        }

        [Test]
        public void RequestParameters()
        {
            var response = TestCommon.GetResponse(200, json);
            var mockclient = TestCommon.GetMockRestClient(response);
            var client = TestCommon.GetClient(mockclient.Object);
            var start = zone.AtStrictly(new LocalDateTime(2012, 1, 1, 0, 0, 0));
            var end = zone.AtStrictly(new LocalDateTime(2012, 1, 2, 0, 0, 0));
            var interval = new Interval(start.ToInstant(), end.ToInstant());

            client.FindDataPoints(series, interval, predicate);

            mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => TestCommon.ContainsParameter(req.Parameters, "key", "key1"))));
            mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => TestCommon.ContainsParameter(req.Parameters, "start", "2012-01-01T00:00:00+00:00"))));
            mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => TestCommon.ContainsParameter(req.Parameters, "end", "2012-01-02T00:00:00+00:00"))));
            mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => TestCommon.ContainsParameter(req.Parameters, "predicate.function", "max"))));
            mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => TestCommon.ContainsParameter(req.Parameters, "predicate.period", "PT1M"))));
            mockclient.Verify(cl => cl.Execute(It.Is<RestRequest>(req => TestCommon.ContainsParameter(req.Parameters, "tz", "UTC"))));
        }

        [Test]
        [ExpectedException(typeof(TempoDBException))]
        public void Error()
        {
            var response = TestCommon.GetResponse(403, "You are forbidden");
            var client = TestCommon.GetClient(response);

            var result = client.FindDataPoints(series, interval, predicate);

            var output = new List<DataPointFound>();
            foreach(DataPointFound dp in result)
            {
                output.Add(dp);
            }
        }
    }
}
