using System;
using System.Collections.Generic;
using System.Windows;

namespace BaseWindow_WPF
{
    public class BaseWindow : Window
    {

        #region Поля
        private BaseDialogs baseDialogs = new BaseDialogs();
        #endregion

        #region Свойства

        #endregion

        #region Методы

        #region Загрузка файлов
        public string GetFolderPath()
        {
            return this.baseDialogs.GetFolderPath();
        }
        public List<string> GetFilesPath(string filter = "Все файлы (*.*)|*.*")
        {
            return this.baseDialogs.GetFilesPath(filter);
        }
        public string[] GetLoadFilePath(
            string filter = "Все файлы (*.*)|*.*",
            bool isMulti = false,
            int filterIndex = 1,
            string defaultExtension = "txt",
            bool checkFileExists = false,
            bool checkPathExists = true,
            string Title = "Сохранение файла"
            )
        {
            return this.baseDialogs.GetLoadFilePath(filter, isMulti, filterIndex, defaultExtension, checkFileExists, checkPathExists, Title);
        }
        #endregion

        #region Сохранение файлов
        /// <summary>
        /// Получает путь для сохранения файла
        /// </summary>
        /// <returns></returns>
        public string GetSaveFilePath()
        {
            return this.baseDialogs.GetSaveFilePath();
        }
        #endregion

        #region Уведомления
        public void ShowError(string message)
        {
            this.baseDialogs.ShowError(message);
        }
        public void ShowError(Exception ex, bool isShowInner = true)
        {
            this.baseDialogs.ShowError(ex, isShowInner);
        }
        public void ShowMessage(string message, string title = "Уведомление")
        {
            this.baseDialogs.ShowMessage(message, title);
        }
        public void ShowWarning(string message, string title = "Предупреждение")
        {
            this.baseDialogs.ShowWarning(message, title);
        }
        #endregion

        /// <summary>
        /// Задает стартовую локацию окна по центру
        /// </summary>
        public void SetCenter(object sender)
        {
            var window = sender as Window;
            if (window.Owner != null)
            {
                // Центрирование относительно родительского окна
                window.Left = window.Owner.Left + (window.Owner.Width - window.ActualWidth) / 2;
                window.Top = window.Owner.Top + (window.Owner.Height - window.ActualHeight) / 2;
            }
            else
            {
                // Центрирование на экране, если нет родительского окна
                var screenWidth = SystemParameters.PrimaryScreenWidth;
                var screenHeight = SystemParameters.PrimaryScreenHeight;
                var windowWidth = window.ActualWidth;
                var windowHeight = window.ActualHeight;
                window.Left = (screenWidth / 2) - (windowWidth / 2);
                window.Top = (screenHeight / 2) - (windowHeight / 2);
            }
        }
        #endregion

        #region Конструкторы/Деструкторы
        public BaseWindow()
        {
            //this.SetCenter();
        }
        #endregion

        #region Операторы

        #endregion

        #region Обработчики событий

        #endregion

    }
}
