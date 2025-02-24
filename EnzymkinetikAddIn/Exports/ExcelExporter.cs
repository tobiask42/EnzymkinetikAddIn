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
        public static void ExportToExcel(Dictionary<string, DataTable> tableDictionary)
        {
            // Excel-Applikation starten
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

                // Iteriere durch alle Tabellen im Dictionary
                foreach (var kvp in tableDictionary)
                {
                    string tableName = kvp.Key;
                    DataTable table = kvp.Value;

                    if (table == null) continue;

                    // Schreibe den Tabellennamen als Überschrift
                    worksheet.Cells[currentRow, currentCol] = tableName;
                    worksheet.Cells[currentRow, currentCol].Font.Bold = true;  // Fett formatieren
                    currentRow++;  // Gehe zur nächsten Zeile

                    // Schreibe die Spaltenüberschriften
                    for (int i = 0; i < table.Columns.Count; i++)
                    {
                        worksheet.Cells[currentRow, currentCol + i] = table.Columns[i].ColumnName;
                        worksheet.Cells[currentRow, currentCol + i].Font.Bold = true;
                    }

                    currentRow++;  // Gehe zur nächsten Zeile für die Daten

                    // Schreibe die Datenzeilen
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        for (int j = 0; j < table.Columns.Count; j++)
                        {
                            object value = table.Rows[i][j];
                            if (value.ToString().Equals("NaN")){
                                continue;
                            }
                            worksheet.Cells[currentRow + i, currentCol + j] = value;
                        }
                    }

                    currentRow += table.Rows.Count + 3;  // Nach unten verschieben für die nächste Tabelle
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
    }
}