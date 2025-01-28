using System;
using System.Windows.Forms;

internal class DataGridViewTextBoxHandler
{
    public static void AttachTextBoxEventHandler(
        DataGridView dataGridView,
        string columnName,
        KeyPressEventHandler keyPressHandler)
    {
        dataGridView.EditingControlShowing += (sender, e) =>
        {
            if (e.Control is TextBox textBox && IsTargetColumn(dataGridView, columnName))
            {
                // Entferne frühere Handler und binde neuen Handler
                textBox.KeyPress -= keyPressHandler;
                textBox.KeyPress += keyPressHandler;
            }
            else if (e.Control is TextBox otherTextBox)
            {
                // Entferne Handler aus nicht relevanten Spalten
                otherTextBox.KeyPress -= keyPressHandler;
            }
        };
    }

    private static bool IsTargetColumn(DataGridView dataGridView, string columnName)
    {
        return dataGridView.CurrentCell?.OwningColumn.Name == columnName;
    }
}
