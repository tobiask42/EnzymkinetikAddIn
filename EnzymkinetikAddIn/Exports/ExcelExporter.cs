using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnzymkinetikAddIn.Interfaces;
using Excel = Microsoft.Office.Interop.Excel;

namespace EnzymkinetikAddIn.Exports
{
    internal class ExcelExporter
    {

        public static void ExportToExcel(List<List<DataTable>> tables)
        {
            DataTable[,] arrayTables = new DataTable[tables.Count, tables[0].Count];

            for (int i = 0; i < tables.Count; i++)
            {
                for (int j = 0; j < tables[i].Count; j++)
                {
                    arrayTables[i, j] = tables[i][j];
                }
            }

            ExportToExcel(arrayTables);
        }

        public static void ExportToExcel(DataTable[,] tables)
        {
            if (tables == null || tables.Length == 0)
                throw new ArgumentException("Keine Tabellen zum Exportieren vorhanden!");

            Excel.Application excelApp = null;
            Excel.Worksheet worksheet = null;

            try
            {
                // Excel-Applikation abrufen
                excelApp = (Excel.Application)System.Runtime.InteropServices.Marshal.GetActiveObject("Excel.Application");
                Excel.Workbook workbook = excelApp.ActiveWorkbook;
                worksheet = workbook.ActiveSheet;

                // Startpunkt = aktuell ausgewählte Zelle
                Excel.Range startCell = excelApp.ActiveCell;
                int startRow = startCell.Row;
                int startCol = startCell.Column;

                int currentRow = startRow;
                int currentCol = startCol;

                // Iteriere durch alle Tabellen (z.B. Tabellen in Spalten anordnen)
                for (int i = 0; i < tables.GetLength(0); i++)
                {
                    for (int j = 0; j < tables.GetLength(1); j++)
                    {
                        DataTable table = tables[i, j];
                        if (table == null) continue;

                        // Tabelle in Excel schreiben
                        WriteTableToExcel(worksheet, table, currentRow, currentCol);

                        // Nach rechts verschieben für die nächste Tabelle
                        currentCol += table.Columns.Count + 2; // 2 Spalten als Abstand
                    }

                    // Nach unten verschieben für die nächste Tabellenreihe
                    currentCol = startCol;
                    currentRow += tables[0, 0].Rows.Count + 3; // 3 Zeilen Abstand
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Fehler beim Excel-Export: " + ex.Message);
            }
            finally
            {
                // Excel Speicherbereinigung
                if (worksheet != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
                if (excelApp != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }
        }

        private static void WriteTableToExcel(Excel.Worksheet sheet, DataTable table, int row, int col)
        {
            // Spaltenüberschriften
            for (int i = 0; i < table.Columns.Count; i++)
            {
                sheet.Cells[row, col + i] = table.Columns[i].ColumnName;
                sheet.Cells[row, col + i].Font.Bold = true; // Fettdruck für Header
            }

            // Datenzeilen
            for (int i = 0; i < table.Rows.Count; i++)
            {
                for (int j = 0; j < table.Columns.Count; j++)
                {
                    object value = table.Rows[i][j];
                    sheet.Cells[row + i + 1, col + j] = value == DBNull.Value ? "" : value;
                }
            }
        }
    }
}
