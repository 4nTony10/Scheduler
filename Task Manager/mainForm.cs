using System;
using System.Diagnostics;
using System.Windows.Forms;


namespace Task_Manager
{
    public partial class mainForm : Form
    {
        TaskRepo taskRepo = new TaskRepo();

        public mainForm()
        {
            InitializeComponent();
            dateTimePicker.MinDate = DateTime.Now;
        }

        #region Table
        private void FillDataGrid()//Заполнение таблицы
        {
            dataGridView1.Rows.Clear();
            foreach(var task in taskRepo.GetTasks())
            {
                dataGridView1.Rows.Add(task.Name, task.Description, task.Date.ToShortDateString(), task.Priority, task.isDone);
            }
        }
        private void dataGridView1_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)//Удаление ряда
        {
            taskRepo.RemoveTask(e.Row.Index);
        }
        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)//Редактирование ячеек
        {
            int col_ind = e.ColumnIndex;
            int row_ind = e.RowIndex;
            switch (col_ind)
            {
                case 0:
                    taskRepo.editName(dataGridView1.Rows[row_ind].Cells[col_ind].Value.ToString(), row_ind);
                    break;
                case 1:
                    taskRepo.editDescription(dataGridView1.Rows[row_ind].Cells[col_ind].Value.ToString(), row_ind);
                    break;
                case 2:
                    taskRepo.editDate(Convert.ToDateTime(dataGridView1.Rows[row_ind].Cells[col_ind].Value.ToString()), row_ind);
                    break;
                case 3:
                    taskRepo.editPriority(Convert.ToInt32(dataGridView1.Rows[row_ind].Cells[col_ind].Value.ToString()), row_ind);
                    break;
                case 4:
                    taskRepo.editStatus(bool.Parse(dataGridView1.Rows[row_ind].Cells[col_ind].Value.ToString()), row_ind);
                    break;
            }
        }
        #endregion

        #region Buttons
        private void buttonAdd_Click(object sender, EventArgs e)//Добавление задания
        {
            if (String.IsNullOrWhiteSpace(textBoxName.Text))
            {
                MessageBox.Show("Внимание!\nВведите название задачи!", "Внимание!",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (String.IsNullOrWhiteSpace(textBoxDesc.Text))
            {
                MessageBox.Show("Внимание!\nВведите описание задачи!", "Внимание!",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                taskRepo.AddTask(textBoxName.Text, textBoxDesc.Text, dateTimePicker.Value, (int)numericUpDownPriority.Value);

                textBoxName.Text = String.Empty;
                textBoxDesc.Text = String.Empty;
                dateTimePicker.Value = DateTime.Now;
                numericUpDownPriority.Value = 0;

                FillDataGrid();
            }
        }
        private void buttonClear_Click(object sender, EventArgs e)//Очистка таблицы и списка заданий
        {
            taskRepo.Clear();
            dataGridView1.Rows.Clear();
        }
        private void buttonSearch_Click(object sender, EventArgs e)//Сортировка
        {
            dataGridView1.Rows.Clear();

            foreach (var task in taskRepo.Search())
            {
                dataGridView1.Rows.Add(task.Name, task.Description, task.Date.ToShortDateString(), task.Priority, task.isDone);
            }
        }
        #endregion

        #region MenuStrip
        private void LoadToolStripMenuItem_Click(object sender, EventArgs e)//Загрузка с файла
        {
            taskRepo.Load();
            FillDataGrid();
            taskRepo.Today();
        }
        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)//Сохранение в файл
        {
            if (dataGridView1.Rows.Count > 0)
                taskRepo.Save();
        }
        private void v1ToolStripMenuItem_Click(object sender, EventArgs e)//Открыте первого пдф
        {
            Process.Start(@"Saves\Tasks.pdf");
        }
        private void v2ToolStripMenuItem_Click(object sender, EventArgs e)//открыте второго пдф
        {
            Process.Start(@"Saves\pdfTable.pdf");
        }
        #endregion

        #region DragDrop
        private void dataGridView1_DragDrop(object sender, DragEventArgs e)//Перетаскивание файла на/в таблицу
        {
            var dropped_data = e.Data.GetData(DataFormats.FileDrop);
            if (dropped_data != null)
            {
                var filename = dropped_data as string[];
                string path = filename[0];
                dataGridView1.Rows.Clear();
                taskRepo.DragDrop(path);
                FillDataGrid();
                taskRepo.Today();
            }
        }

        private void dataGridView1_DragOver(object sender, DragEventArgs e)//Изменение значка
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Link;
            else
                e.Effect = DragDropEffects.None;
        }
        #endregion
    }
}
