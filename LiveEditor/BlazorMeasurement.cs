namespace LiveEditor
{
    public class BlazorMeasurement
    {
        /// <summary>
        /// DateTime in UTC
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// List of measurement values indexed by the channels -> int[] ChannelIds 
        /// </summary>
        public float[] Values { get; set; }
    }
}