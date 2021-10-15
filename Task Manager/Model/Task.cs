using System;

namespace Task_Manager
{
    [Serializable]
    public class Task
    {
        public string Name { get; set; }//Имя заданий
        public string Description { get; set; }//Описание задания
        public DateTime Date { get; set; }//Дата задания
        public int Priority { get; set; }//Приоритетность задания
        public bool isDone { get; set; }//Состояние задания
    }
}
