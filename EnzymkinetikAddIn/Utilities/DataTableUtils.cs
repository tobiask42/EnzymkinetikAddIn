using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnzymkinetikAddIn.Utilities
{
    internal class DataTableUtils
    {
        // Hilfsmethode zum Verschieben von Spalten
        public static void MoveColumns(DataTable table, int[] from, int[] to)
        {
            // Überprüfen, ob die Längen der Arrays übereinstimmen
            if (from.Length != to.Length)
            {
                throw new ArgumentException("Equal size of to and from arrays required");
            }

            // Verschieben der Spalten
            for (int i = 0; i < from.Length; i++)
            {
                int fromIndex = from[i];
                int toIndex = to[i];

                // Überprüfen, ob die Indizes gültig sind
                if (fromIndex < 0 || fromIndex >= table.Columns.Count || toIndex < 0 || toIndex >= table.Columns.Count)
                {
                    throw new ArgumentOutOfRangeException("Invalid Column index");
                }
                table.Columns[fromIndex].SetOrdinal(to[i]);
            }
        }
    }
}