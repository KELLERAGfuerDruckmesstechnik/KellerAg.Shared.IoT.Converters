using NodaTime;
using System;
using System.Collections.Generic;
using System.Linq;
using TimeZoneConverter;

namespace KellerAg.Shared.Entities.Localization
{
    /// <summary>
    /// Helper class to easier transform UTC-DateTime strings into DateTime of specific TimeZone
    /// It uses the IANA TimeZones e.g. "Europe/Zurich" or "Etc/GMT-1"
    /// It does not use Windows PC TimeZone names "(UTC+01:00) Amsterdam, Berlin, Bern, Rome, Stockholm, Vienna"
    /// In case the used TimeZone can not be used, the local time will be used instead TODO: Is this a good idea? No warning?
    /// See unit test for usage.
    /// Some methods are static some need an instance!
    /// Needs NodaTime and TimeZoneConverter Nuget packages
    /// </summary>
    public class DateTimeHelper
    {
        private readonly DateTimeZone _zone;

        /// <summary>
        /// Use the class with this constructor and a compatible IANA TimeZone name e.g. "Europe/Zurich" or "Etc/GMT-1"
        /// </summary>
        /// <param name="ianaTimeZoneName"></param>
        public DateTimeHelper(string ianaTimeZoneName)
        {
            _zone = GetZoneOrLocalZone(ianaTimeZoneName);
        }

        /// <summary>
        /// Not recommended: This constructor uses the local system TimeZone
        /// </summary>
        public DateTimeHelper()
        {
            TimeZoneInfo tzi = GetLocalTimeZoneInfo();
            string ianaName = DeGeneralizeIanaName(tzi.Id);
            _zone = GetZoneOrLocalZone(ianaName);
        }

        private DateTimeZone GetZoneOrLocalZone(string ianaTimeZoneName)
        {
            DateTimeZone zone;
            try
            {
                zone = DateTimeZoneProviders.Tzdb[ianaTimeZoneName];
            }
            catch (Exception)
            {
                // ianaTimeZoneName is not known!
                // e.g. "(UTC+01:00) Amsterdam, Berlin, Bern, Rome, Stockholm, Vienna"
                // take local timezone
                zone = GetLocalDateTimeZone();
            }
            return zone;
        }

        /// <summary>
        /// Checks if IANA time zone string is known. If not return false.
        /// This is used by the Cloud when LocalDateTime is unusable.
        /// </summary>
        /// <param name="ianaTimeZoneName"></param>
        /// <returns></returns>
        public static bool IsValidIanaTimeZone(string ianaTimeZoneName)
        {
            try
            {
                var zone = DateTimeZoneProviders.Tzdb[ianaTimeZoneName];
            }
            catch (Exception)
            {
                // ianaTimeZoneName is not known!
                return false;
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="datetime">localized datetime</param>
        /// <returns></returns>
        public TimeSpan GetTimezoneOffsetAt(DateTime datetime)
        {
            var offset = _zone.GetUtcOffset(Instant.FromDateTimeUtc(DeLocalizeDateTime(datetime)));
            return offset.ToTimeSpan();
        }

        /// <summary>
        /// Transform a DateTime string (from json) and transform it into a DateTime-string of the stored zone
        /// </summary>
        /// <param name="utcDateTimeText"></param>
        /// <returns></returns>
        public DateTime LocalizeDateTime(string utcDateTimeText)
        {
            //DateTime someDateTime = DateTime.ParseExact(utcDateTimeText, "s", CultureInfo.InvariantCulture);
            DateTime someDateTime = DateTime.Parse(utcDateTimeText);
            return LocalizeDateTime(someDateTime);
        }

        /// <summary>
        /// Adds the TimeSpan difference of the stored TimeZone to the UTC-Datetime "utcDateTime"
        /// </summary>
        /// <param name="utcDateTime"></param>
        /// <returns></returns>
        public DateTime LocalizeDateTime(DateTime utcDateTime)
        {
            try
            {
                var specifiedUtcDateTime = DateTime.SpecifyKind(utcDateTime, DateTimeKind.Utc);
                var instant = Instant.FromDateTimeUtc(specifiedUtcDateTime);
                var specifiedLocalDateTime = instant.InZone(_zone).ToDateTimeUnspecified();
                return specifiedLocalDateTime;
            }
            catch (InvalidOperationException) // DateTime out of range
            {
                return utcDateTime;
            }
            catch (ArgumentOutOfRangeException) // DateTime out of range
            {
                return utcDateTime;
            }
        }

        /// <summary>
        /// Uses an localDateTime and subtracts the TimeSpan difference between UTC and the stored _zone
        /// </summary>
        /// <param name="localDateTime"></param>
        /// <returns></returns>
        public DateTime DeLocalizeDateTime(DateTime localDateTime)
        {
            try
            {
                var localTime = LocalDateTime.FromDateTime(localDateTime).InZoneLeniently(_zone);
                var utcTime = localTime.ToDateTimeUtc();
                return utcTime;
            }
            catch (InvalidOperationException) // DateTime out of range
            {
                return localDateTime;
            }
            catch (ArgumentOutOfRangeException) // DateTime out of range
            {
                return localDateTime;
            }
        }

        /// <summary>
        /// Gets actual TimeZone as NodaTime DateTimeZone
        /// If you only want the name then better use GetLocalDateTimeZoneId() or GetLocalDateTimeZoneDisplayName()
        /// </summary>
        /// <returns></returns>
        public static DateTimeZone GetLocalDateTimeZone()
        {
            var tzi = GetLocalTimeZoneInfo();
            string ianaName = DeGeneralizeIanaName(tzi.Id);
            return DateTimeZoneProviders.Tzdb[ianaName];
        }

        /// <summary>
        /// Gets system's generalized TimeZone id such as "Europe/Berlin"
        /// This is not the stored IANA TimeZone used in the constructor
        /// </summary>
        /// <returns></returns>
        public static string GetLocalSystemIanaTimeZoneName()
        {
            return GetLocalDateTimeZone().Id;
        }

        /// <summary>
        /// Gets the system's TimeZone-Id such as "(UTC+01:00) Amsterdam, Berlin, Bern, Rome, Stockholm, Vienna"
        /// </summary>
        /// <returns></returns>
        public static string GetLocalSystemTimeZoneDisplayName()
        {
            return GetLocalTimeZoneInfo().DisplayName;
        }

        /// <summary>
        /// Input:  "Europe/Zurich"
        /// Output: "W. Europe Standard Time"
        /// </summary>
        /// <param name="ianaIdName"></param>
        /// <returns></returns>
        public static string GeneralizeIanaName(string ianaIdName)
        {
            return TZConvert.TryIanaToWindows(ianaIdName, out string name) ? name : ianaIdName;
        }

        /// <summary>
        /// Input:  "W. Europe Standard Time"
        /// Output: "Europe/Berlin"  (There are multiple IANA names possible of EST, but it takes the first)
        /// </summary>
        /// <param name="windowsName"></param>
        /// <returns></returns>
        public static string DeGeneralizeIanaName(string windowsName)
        {
            return TZConvert.TryWindowsToIana(windowsName, out string name) ? name : windowsName;
        }

        /// <summary>
        /// Output: e.g. "27.10.2018"
        /// Use this only when the (Swiss) format fits your need (e.g. file names)
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string GetDateText(DateTime dt)
        {
            return $"{dt:dd.MM.yyyy}";
        }

        /// <summary>
        /// Output: e.g. "18:42:59"
        /// Use this only when the (Swiss) format fits your need (e.g. Hydra format)
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string GetTimeText(DateTime dt)
        {
            return $"{dt:HH:mm:ss}";
        }

        /// <summary>
        /// Gets a list of all compatible IANA TimeZone names
        /// Lists European countries first.
        /// </summary>
        /// <returns></returns>
        public static ICollection<string> GetIanaTimeZoneNames()
        {
            var allTimeZoneNames = TZConvert.KnownIanaTimeZoneNames;
            var europeZonesNames = allTimeZoneNames.Where(n => n.ToLower().StartsWith("europe")).OrderBy(n => n).ToList();
            var nonEuropeZonesNames = allTimeZoneNames.Where(n => !n.ToLower().StartsWith("europe")).OrderBy(n => n).ToList();
            var allTimeZoneNamesButEuropeComesFirst = europeZonesNames;
            allTimeZoneNamesButEuropeComesFirst.AddRange(nonEuropeZonesNames);
            return allTimeZoneNamesButEuropeComesFirst;
        }

        private static TimeZoneInfo GetLocalTimeZoneInfo()
        {
            return TimeZoneInfo.Local;
        }
    }
}
