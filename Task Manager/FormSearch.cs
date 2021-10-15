using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Task_Manager
{
    public partial class FormSearch : Form
    {
        public FormSearch()
        {
            InitializeComponent();
        }

        public FormSearch(List<Task> tasks)
        {
            InitializeComponent();
            Base = tasks;
            monthCalendar1.MaxSelectionCount = 1;
        }

        public List<Task> Tasks { get; set; }
        List<Task> Base { get; set; }

        private void CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonPriority.Checked)
            {
                monthCalendar1.Enabled = false;
                numericUpDown1.Enabled = true;
            }

            if (radioButtonDate.Checked)
            {
                numericUpDown1.Enabled = false;
                monthCalendar1.Enabled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButtonPriority.Checked)
            {
                if (numericUpDown1.Value >= 0)
                {
                    Tasks = new List<Task>();
                    foreach (var task in Base)
                        if (task.Priority == numericUpDown1.Value)
                            Tasks.Add(task);

                    Close();
                }
                else
                    MessageBox.Show("Wrong priority!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            if (radioButtonDate.Checked)
            {
                if (monthCalendar1.SelectionStart != null)
                {
                    Tasks = new List<Task>();
                    foreach (var task in Base)
                        if (task.Date.ToShortDateString() == monthCalendar1.SelectionStart.ToShortDateString())
                            Tasks.Add(task);

                    Close();
                }
                else
                    MessageBox.Show("Date is not selected! Select a date!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
        }
    }
}
