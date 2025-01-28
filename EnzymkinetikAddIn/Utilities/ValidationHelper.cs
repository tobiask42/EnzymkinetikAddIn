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
            // Steuerzeichen (Backspace, Delete etc.) zulassen
            if (char.IsControl(inputChar))
                return true;

            // Prüfen ob das erste Zeichen ein Punkt, Komma ist
            if ((inputChar == ',' || inputChar == '.') && string.IsNullOrEmpty(textBox.Text))
                return false;

            // Prüfen, ob bereits ein Punkt oder Komma existiert
            if ((inputChar == ',' || inputChar == '.') && (textBox.Text.Contains(",") || textBox.Text.Contains(".")))
                return false;

            // Nur Ziffern, Punkte oder Kommas zulassen
            return char.IsDigit(inputChar) || inputChar == ',' || inputChar == '.';
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
