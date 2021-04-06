namespace TRuDI.Models
{
    using System;
    using NodaTime;

    public static class NodaTimeExtensions
    {
        public static readonly ZonedDateTime MinValue = DateTimeZone.Utc.AtStrictly(new LocalDate(1900, 1, 1).AtMidnight());
        public static readonly ZonedDateTime MaxValue = DateTimeZone.Utc.AtStrictly(new LocalDate(2300, 1, 1).AtMidnight());

        public static ZonedDateTime ToZonedDateTime(this DateTimeOffset timestamp, DateTimeZone timeZone = null)
        {
            if (timeZone == null)
            {
                timeZone = DateTimeZoneProviders.Tzdb["Europe/Berlin"];
            }

            if (timestamp == DateTimeOffset.MinValue || timestamp.Year < 1900)
            {
                return MinValue;
            }

            if (timestamp == DateTimeOffset.MaxValue || timestamp.Year > 2300)
            {
                return MaxValue;
            }

            var localDateTime = new LocalDateTime(timestamp.Year, timestamp.Month, timestamp.Day, timestamp.Hour, timestamp.Minute, timestamp.Second);

            if (timestamp.Offset == TimeSpan.Zero)
            {
                // treat as UTC
                return (new ZonedDateTime(localDateTime, DateTimeZone.Utc, Offset.FromTimeSpan(timestamp.Offset))).WithZone(timeZone);
            }

            return new ZonedDateTime(localDateTime, timeZone, Offset.FromTimeSpan(timestamp.Offset));
        }

        public static ZonedDateTime ToZonedDateTime(this DateTime timestamp, DateTimeZone timeZone = null)
        {
            return ToZonedDateTime(new DateTimeOffset(timestamp), timeZone);
        }
    }
}
