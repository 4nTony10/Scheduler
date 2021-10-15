using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Aspose.Pdf;

namespace Task_Manager
{
    public class TaskRepo : IRepository
    {
        /// <summary>
        /// Список заданий
        /// </summary>
        private List<Task> tasks;

        /// <summary>
        /// Конструктор
        /// </summary>
        public TaskRepo()
        {
            tasks = new List<Task>();
        }

        #region Add
        public void AddTask(string name, string description, DateTime date, int priority = 0, bool status = false)
        {
            Task task = new Task();

            task.Name = name;
            task.Description = description;
            task.Date = date;
            task.Priority = priority;
            task.isDone = status;

            if (date >= DateTime.Now)
                tasks.Add(task);
            else
                MessageBox.Show("You can not add task in past!", "Past travel?", MessageBoxButtons.OK, MessageBoxIcon.Question);
        }
        public void AddTask(Task item)
        {
            if (item.Date >= DateTime.Now)
                tasks.Add(item);
            else
                MessageBox.Show("You can not add task in past!", "Past travel?", MessageBoxButtons.OK, MessageBoxIcon.Question);
        }
        #endregion

        #region Remove & Clear
        public void RemoveTask(int index)
        {
            tasks.RemoveAt(index);
        }
        public void Clear()
        {
            tasks.Clear();
        }
        #endregion

        #region Edits
        public void editName(string name, int index)
        {
            tasks[index].Name = name;
        }
        public void editDescription(string desc, int index)
        {
            tasks[index].Description = desc;
        }
        public void editDate(DateTime date, int index)
        {
            if (date >= DateTime.Now)
                tasks[index].Date = date;
            else
                MessageBox.Show("Wrong time!", "Date warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        public void editPriority(int priority, int index)
        {
            tasks[index].Priority = priority;
        }
        public void editStatus(bool status, int index)
        {
            tasks[index].isDone = status;
        }
        #endregion
        
        public List<Task> GetTasks()
        {
            return tasks;
        }
        public List<Task> Search()
        {
            FormSearch form = new FormSearch(tasks);
            form.ShowDialog();

                if (form.Tasks.Count == 0)
                {
                    MessageBox.Show("There is nothing by your parametrs", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return tasks;
                }

                return form.Tasks;
        }

        #region File Work
        public void Save()
        {
            List<string> lines = new List<string>();
            foreach (var item in tasks)
                lines.Add($"{item.Name}, {item.Description}, {item.Date.ToShortDateString()}, {item.Priority}, {item.isDone}");

            if (!File.Exists(@"Saves\Tasks.txt"))
                File.Create(@"Saves\Tasks.txt");

            File.WriteAllLines(@"Saves\Tasks.txt", lines);

            #region Aspsoe
            Aspose.Pdf.Document document = new Aspose.Pdf.Document();
            Page page = document.Pages.Add();

            int counter = 1;

            page.Paragraphs.Add(new Aspose.Pdf.Text.TextFragment("[Task #]" + "[Name]\t" + "[Description]\t" + "[Date]\t" + "[Priority]\t" + "[Status]\n"));

            foreach (var line in lines)
            {
                page.Paragraphs.Add(new Aspose.Pdf.Text.TextFragment("Task #" + counter + ": " + line));
                counter++;
            }

            document.Save(@"Saves\Tasks.pdf");
            #endregion

            #region ITextSharp
            iTextSharp.text.Document doc = new iTextSharp.text.Document();
            PdfWriter.GetInstance(doc, new FileStream(@"Saves\pdfTable.pdf", FileMode.Create));
            doc.Open();

            BaseFont baseFont = BaseFont.CreateFont(@"C:\Windows\Fonts\arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            iTextSharp.text.Font font = new iTextSharp.text.Font(baseFont, iTextSharp.text.Font.DEFAULTSIZE, iTextSharp.text.Font.NORMAL);

            PdfPTable table = new PdfPTable(tasks.Count);
            PdfPCell cell = new PdfPCell(new Phrase("Задания, таблица №1", font));

            cell.Colspan = tasks.Count;
            cell.HorizontalAlignment = 2;

            cell.Border = 0;
            table.AddCell(cell);

            table.AddCell(new Phrase("[Name]", font));
            table.AddCell(new Phrase("[Description]", font));
            table.AddCell(new Phrase("[Date]", font));
            table.AddCell(new Phrase("[Priority]", font));

            foreach (var line in lines)
            {
                string[] data = line.Split(',');
                table.AddCell(new Phrase(data[0], font));
                table.AddCell(new Phrase(data[1], font));
                table.AddCell(new Phrase(data[2], font));
                table.AddCell(new Phrase(data[3], font));
            }

            doc.Add(table);   
            doc.Close();

            #endregion 

            MessageBox.Show("Pdf-документ сохранен", "INFO", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        public List<Task> Load()
        {
            if (!File.Exists(@"Saves\Tasks.txt"))
                return null;

            List<string> lines = File.ReadAllLines(@"Saves\Tasks.txt").ToList();

            foreach (var line in lines)
            {
                string[] data = line.Split(',');
                tasks.Add(new Task
                {
                    Name = data[0],
                    Description = data[1],
                    Date = Convert.ToDateTime(data[2]),
                    Priority = Convert.ToInt32(data[3]),
                    isDone = Convert.ToBoolean(data[4])
                });
            }

            return tasks;
        }
        #endregion

        public void DragDrop(string filename)
        {
            if (File.Exists(filename))
            {
                List<string> lines = File.ReadAllLines(filename).ToList();

                foreach (var line in lines)
                {
                    string[] data = line.Split(',');
                    tasks.Add(new Task
                    {
                        Name = data[0],
                        Description = data[1],
                        Date = Convert.ToDateTime(data[2]),
                        Priority = Convert.ToInt32(data[3]),
                        isDone = Convert.ToBoolean(data[4])
                    });
                }
            }

        }

        public void Today()
        {
            foreach (var task in tasks)
            {
                if ((task.Date == DateTime.Today || task.Date < DateTime.Today) && task.isDone == false)
                    MessageBox.Show("Задание " + task.Name + " просрочено!", "Просрочка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
