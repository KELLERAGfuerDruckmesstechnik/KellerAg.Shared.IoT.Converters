using System;
using System.Collections.Generic;
using System.Linq;


namespace KellerAg.Shared.Entities.DownSampling
{
    /// <summary>
    /// Largest-Triangle-Three Bucket Downsampling Graphs in C#
    /// From https://gist.github.com/DanielWJudge/63300889f27c7f50eeb7
    /// (There is another alternative: https://gist.github.com/adrianseeley/264417d295ccd006e7fd
    /// </summary>
    public static class DownSampling
    {
        /// <summary>
        /// Unfortunately ToOADate()/FromOADate() is unknown in Core1.1
        /// So this is a version to convert DateTime to Double using TimeSpans
        /// </summary>
        /// <param name="data"></param>
        /// <param name="threshold"></param>
        /// <returns></returns>
        public static List<Tuple<System.DateTime, double>> LargestTriangleThreeBuckets(List<Tuple<System.DateTime, double>> data, int threshold)
        {
            var dataWithDateTime = new List<Tuple<double, double>>(data.Count);
            var startMoment = new System.DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            dataWithDateTime.AddRange(data.Select(tuple => new Tuple<double, double>((tuple.Item1 - startMoment).TotalSeconds, tuple.Item2)));
            List<Tuple<double, double>> filteredDataWithDoublesOnly = LargestTriangleThreeBuckets(dataWithDateTime, threshold);
            return filteredDataWithDoublesOnly.Select(x => new Tuple<System.DateTime, double>(startMoment + TimeSpan.FromSeconds(x.Item1), x.Item2)).ToList();
        }


        public static List<Tuple<double, double>> LargestTriangleThreeBuckets(List<Tuple<double, double>> data, int threshold)
        {
            int dataLength = data.Count;
            if (threshold >= dataLength || threshold == 0)
                return data; // Nothing to do


            var sampled = new List<Tuple<double, double>>(threshold);

            // Bucket size. Leave room for start and end data points
            double every = (double)(dataLength - 2) / (threshold - 2);

            int a = 0;
            var maxAreaPoint = new Tuple<double, double>(0, 0);
            int nextA = 0;

            sampled.Add(data[a]); // Always add the first point

            for (int i = 0; i < threshold - 2; i++)
            {
                // Calculate point average for next bucket (containing c)
                double avgX = 0;
                double avgY = 0;
                int avgRangeStart = (int)(Math.Floor((i + 1) * every) + 1);
                int avgRangeEnd = (int)(Math.Floor((i + 2) * every) + 1);
                avgRangeEnd = avgRangeEnd < dataLength ? avgRangeEnd : dataLength;

                int avgRangeLength = avgRangeEnd - avgRangeStart;

                for (; avgRangeStart < avgRangeEnd; avgRangeStart++)
                {
                    avgX += data[avgRangeStart].Item1; // * 1 enforces Number (value may be Date)
                    avgY += data[avgRangeStart].Item2;
                }

                avgX /= avgRangeLength;

                avgY /= avgRangeLength;

                // Get the range for this bucket
                int rangeOffs = (int)(Math.Floor((i + 0) * every) + 1);
                int rangeTo = (int)(Math.Floor((i + 1) * every) + 1);

                // Point a
                double pointAx = data[a].Item1; // enforce Number (value may be Date)
                double pointAy = data[a].Item2;

                double maxArea = -1;

                for (; rangeOffs < rangeTo; rangeOffs++)
                {
                    // Calculate triangle area over three buckets
                    double area = Math.Abs((pointAx - avgX) * (data[rangeOffs].Item2 - pointAy) -
                                           (pointAx - data[rangeOffs].Item1) * (avgY - pointAy)) * 0.5;
                    if (!(area > maxArea)) continue;

                    maxArea = area;
                    maxAreaPoint = data[rangeOffs];
                    nextA = rangeOffs; // Next a is this b
                }

                sampled.Add(maxAreaPoint); // Pick this point from the bucket
                a = nextA; // This a is the next a (chosen b)
            }

            sampled.Add(data[dataLength - 1]); // Always add last

            return sampled;
        }
    }
}