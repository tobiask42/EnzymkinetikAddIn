using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnzymkinetikAddIn.Data;
using System.Windows.Forms;
using EnzymkinetikAddIn.Forms;
using EnzymkinetikAddIn.Utilities;

namespace EnzymkinetikAddIn.Generators
{
    internal class EditFormGenerator
    {
        public BaseForm GenerateForm(string tableName)
                {
                    // Lade die Daten aus der Datenbank
                    DataTable tableData = DatabaseHelper.LoadTable(tableName);

                    // Erstelle das Formular
                    BaseForm form = new BaseForm();

                    // Zugriff auf das DataGridView des Formulars
                    var dataGridView = form.GetDataGridView();

                    // Initialisiere die Spalten über den ColumnManager
                    ColumnManager columnManager = new ColumnManager(dataGridView);

                    // Über die Spalten des DataTables iterieren
                    foreach (DataColumn column in tableData.Columns)
                    {
                        string columnLabel = column.ColumnName;
                        if (columnLabel.Contains("Zeit"))
                        {
                            string timeunit = Char.ToString(columnLabel.ElementAt(6));
                            columnManager.SetTimeUnit(timeunit);
                            columnManager.InitializeTimeColumn();
                        }
                        else if (columnLabel.Contains("Verdünnung"))
                        {
                            string number = Char.ToString(columnLabel.ElementAt(2));
                            columnManager.InitializeIntCol("dilution_" + number, "c_" + number + "\nVerdünnung");

                        }
                        else if (columnLabel.Contains("Messwert"))
                        {
                            string sampleunit = "";
                            if (columnLabel.Contains("g_L"))
                            {
                                sampleunit = "g/L";
                            }
                            else
                            {
                                sampleunit = "mg/dL";
                            }
                            char c = columnLabel.ElementAt(2);
                            char num = columnLabel.ElementAt(13);
                            if (num == '1')
                            {
                                columnManager.InitializeDoubleCol("reading_1_" + c, "c_" + c + "\nMesswert 1\n" + sampleunit);
                            }
                            else if (num == '2')
                            {
                                columnManager.InitializeDoubleCol("reading_2_" + c, "c_" + c + "\nMesswert 2\n" + sampleunit);
                            }
                            else
                            {
                                MessageBox.Show("Fehler beim Initialisieren der Messwert-Spalte", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            columnManager.InitializeBaseCol("comment", "Kommentar");
                        }
                    }


                    return form;
                }
    
            }
        }
