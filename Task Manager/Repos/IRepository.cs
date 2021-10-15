using System;
using System.Collections.Generic;

namespace Task_Manager
{
    interface IRepository
    {
        /// <summary>
        /// Создание и добавление задания
        /// </summary>
        /// <param name="name">Имя задания</param>
        /// <param name="description">Описание задания</param>
        /// <param name="date">Дата задания</param>
        /// <param name="priority">Проиоритетность задания</param>
        /// <param name="status">Состояние задания</param>
        void AddTask(string name, string description, DateTime date, int priority = 0, bool status = false);

        /// <summary>
        /// Добавление уже готового задания
        /// </summary>
        /// <param name="item">Само задние для добавления</param>
        void AddTask(Task item);

        /// <summary>
        /// Удаление задания
        /// </summary>
        /// <param name="index">Индекс елемента для удаления в списке </param>
        void RemoveTask(int index);

        /// <summary>
        /// Очистка списка заданий
        /// </summary>
        void Clear();

        /// <summary>
        /// Изменение имени задания
        /// </summary>
        /// <param name="name">Новое имя</param>
        /// <param name="index">Индекс елемента в списке</param>
        void editName(string name, int index);

        /// <summary>
        /// Изменение описания задания
        /// </summary>
        /// <param name="desc">Новое описание</param>
        /// <param name="index">Индекс елемента в списке</param>
        void editDescription(string desc, int index);

        /// <summary>
        /// Изменение даты задания
        /// </summary>
        /// <param name="date">Новая дата</param>
        /// <param name="index">Индекс елемента в списке</param>
        void editDate(DateTime date, int index);

        /// <summary>
        /// Изменение приоритета задания
        /// </summary>
        /// <param name="priority">Новое значение приоритета</param>
        /// <param name="index">Индекс елемента в списке</param>
        void editPriority(int priority, int index);

        /// <summary>
        /// Изменение состояния задания
        /// </summary>
        /// <param name="status">Новое значение статуса</param>
        /// <param name="index">Индекс елемента в списке</param>
        void editStatus(bool status, int index);

        /// <summary>
        /// Получения списка заданий
        /// </summary>
        /// <returns>Список заданий</returns>
        List<Task> GetTasks();

        /// <summary>
        /// Фильтрация
        /// </summary>
        /// <returns>Фильтрованый список</returns>
        List<Task> Search();

        /// <summary>
        /// Сохранение списка заданий в текстовый-файл и пдф-файл
        /// </summary>
        void Save();

        /// <summary>
        /// Загрузка списка задний из файла
        /// </summary>
        /// <returns>Список заданий из файла</returns>
        List<Task> Load();

        /// <summary>
        /// Механизм загрузки списка посредством переноса файла на форму
        /// </summary>
        /// <param name="filename">Путь к файлу</param>
        void DragDrop(string filename);

        /// <summary>
        /// Проверка на просроченость задания
        /// </summary>
        void Today();
    }
}
