using System;
using System.Collections.Generic;
using System.Linq;

namespace KellerAg.Shared.Entities.FileFormat
{
    public class CloudMeasurementSeries
    {
        public int MeasurementDefinitionId { get; set; }

        public ICollection<CloudMeasurement> MeasurementSeries { get; set; }


        /// <summary>
        /// From a list of series with different sizes
        /// a new list is produced that has all DateTime on one side and the appropriate values on the other side (or null when there are no values on this DateTime)
        /// Additionally, the MeasurementDefinitionIds are 
        /// </summary>
        /// <param name="sets"></param>
        /// <returns></returns>
        public static Tuple<int[], IEnumerable<Measurements>> CreateBodyFromMeasurements(IList<CloudMeasurementSeries> sets)
        {
            int[] seriesIds = ExtractMeasurementDefinitionIds(sets);

            // Create a dictionary with all DateTime as key and add values to the correct place (setIndex)
            int setIndex = 0;
            Dictionary<System.DateTime, double?[]> allSetsMapped = new Dictionary<System.DateTime, double?[]>();
            foreach (var set in sets)
            {
                foreach (CloudMeasurement meas in set.MeasurementSeries)
                {
                    if (!allSetsMapped.ContainsKey(meas.Time))
                    {
                        allSetsMapped.Add(meas.Time, new double?[sets.Count]);
                    }
                    allSetsMapped[meas.Time][setIndex] = meas.Value;
                }
                setIndex++;
            }

            //Sort the dictionary and transform to wanted list
            List<KeyValuePair<System.DateTime, double?[]>> bodyAsKeyValuePair = allSetsMapped.OrderBy(m => m.Key).ToList();
            IList<Measurements> body = bodyAsKeyValuePair.Select(meas => new Measurements { Time = meas.Key, Values = meas.Value }).ToList();

            return new Tuple<int[], IEnumerable<Measurements>>(seriesIds, body);
        }

        private static int[] ExtractMeasurementDefinitionIds(IList<CloudMeasurementSeries> sets)
        {
            var seriesIds = new int[sets.Count];
            for (var i = 0; i < sets.Count; i++)
            {
                CloudMeasurementSeries set = sets[i];
                seriesIds[i] = set.MeasurementDefinitionId;
            }
            return seriesIds;
        }
    }

    public class CloudMeasurement
    {
        public System.DateTime Time { get; set; }
        public double? Value { get; set; }
    }
}