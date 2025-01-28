using System;

namespace EnzymkinetikAddIn.Utilities
{
    internal class TimeConverter
    {
        // Konstanten für die Anzahl der Sekunden pro Einheit
        private const double SecondsInHour = 3600;
        private const double SecondsInMinute = 60;
        private const double SecondsInDay = 86400;

        /// <summary>
        /// Konvertiert eine Zeitangabe in ein Excel-kompatibles Zeitformat (Tage).
        /// </summary>
        /// <param name="time">Der Zeitwert.</param>
        /// <param name="timeUnit">Die Einheit der Zeit (h, min, s).</param>
        /// <returns>Die Zeit im Excel-Format (Tage).</returns>
        public static double ConvertToExcelFormat(double time, string timeUnit)
        {
            if (timeUnit == "h")
            {
                return time / 24;       // Stunden -> Tage
            }
            else if (timeUnit == "min")
            {
                return time / 1440;     // Minuten -> Tage
            }
            else if (timeUnit == "s")
            {
                return time / 86400;    // Sekunden -> Tage
            }
            else
            {
                throw new ArgumentException("Ungültige Zeiteinheit.", nameof(timeUnit));
            }
        }

        /// <summary>
        /// Führt eine flexible Umrechnung zwischen Zeiteinheiten durch.
        /// Unterstützte Einheiten: h (Stunden), min (Minuten), s (Sekunden).
        /// </summary>
        /// <param name="value">Der Zeitwert.</param>
        /// <param name="fromUnit">Die Quell-Einheit.</param>
        /// <param name="toUnit">Die Ziel-Einheit.</param>
        /// <returns>Der umgerechnete Zeitwert.</returns>
        public static double ConvertTime(double value, string fromUnit, string toUnit)
        {
            if (fromUnit == toUnit)
            {
                return value;
            }

            // Zunächst den Wert in Sekunden umrechnen
            double valueInSeconds = 0;

            if (fromUnit == "h")
            {
                valueInSeconds = value * SecondsInHour; // Stunden in Sekunden
            }
            else if (fromUnit == "min")
            {
                valueInSeconds = value * SecondsInMinute; // Minuten in Sekunden
            }
            else if (fromUnit == "s")
            {
                valueInSeconds = value; // Sekunden bleiben unverändert
            }
            else
            {
                throw new ArgumentException("Ungültige Quell-Einheit.", nameof(fromUnit));
            }

            // Konvertiere die Sekunden in die Ziel-Einheit
            if (toUnit == "h")
            {
                return valueInSeconds / SecondsInHour; // Sekunden in Stunden
            }
            else if (toUnit == "min")
            {
                return valueInSeconds / SecondsInMinute; // Sekunden in Minuten
            }
            else if (toUnit == "s")
            {
                return valueInSeconds; // Sekunden bleiben unverändert
            }
            else
            {
                throw new ArgumentException("Ungültige Ziel-Einheit.", nameof(toUnit));
            }
        }
    }
}
