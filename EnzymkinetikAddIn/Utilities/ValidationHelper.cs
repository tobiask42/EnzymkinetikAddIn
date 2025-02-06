using System.Windows.Forms;

namespace EnzymkinetikAddIn.Utilities
{
    internal class ValidationHelper
    {

        /// <summary>
        /// Validiert Eingaben für Double-Spalten: Nur Zahlen, ein Komma oder Punkt.
        /// </summary>
        public static bool IsValidDoubleInput(TextBox textBox, char inputChar)
        {
            // Steuerzeichen (Backspace, Delete etc.) immer zulassen
            if (char.IsControl(inputChar))
                return true;

            // Zahlen zulassen
            if (char.IsDigit(inputChar))
                return true;

            // Prüfen, ob bereits ein Punkt oder Komma vorhanden ist
            if ((inputChar == ',' || inputChar == '.') && (textBox.Text.Contains(",") || textBox.Text.Contains(".")))
                return false;

            // Punkt oder Komma als erste Eingabe verbieten
            if ((inputChar == ',' || inputChar == '.') && string.IsNullOrEmpty(textBox.Text))
                return false;

            // Punkt und Komma als Dezimaltrennzeichen erlauben
            return inputChar == ',' || inputChar == '.';
        }


        /// <summary>
        /// Validiert Eingaben für Int-Spalten: Nur Zahlen.
        /// </summary>
        public static bool IsValidIntInput(char inputChar)
        {
            // Steuerzeichen (Backspace, Delete etc.) zulassen
            if (char.IsControl(inputChar))
                return true;

            // Nur Ziffern zulassen
            return char.IsDigit(inputChar);
        }
    }
}
