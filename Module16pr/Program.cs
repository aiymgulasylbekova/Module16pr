using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Module16pr
{
    public class Program
    {
        static void LogChange(string changeType, string path)
        {
            string logMessage = $"{DateTime.Now} - {changeType}: {path}";

            try
            {
                File.AppendAllText("logFilePath", logMessage + Environment.NewLine);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при записи в лог-файл: {ex.Message}");
            }

            Console.WriteLine(logMessage);
        }
        static void Main()
        {
            Console.WriteLine("Программа отслеживания изменений в директории");

            Console.Write("Введите путь к отслеживаемой директории: ");
            string directoryPath = Console.ReadLine();

            Console.Write("Введите путь к лог-файлу: ");
            string logFilePath = Console.ReadLine();

            using (var watcher = new FileSystemWatcher(directoryPath))
            {
                watcher.IncludeSubdirectories = true;
                watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;

                watcher.Changed += (sender, e) => LogChange("Изменение", e.FullPath);
                watcher.Created += (sender, e) => LogChange("Создание", e.FullPath);
                watcher.Deleted += (sender, e) => LogChange("Удаление", e.FullPath);
                watcher.Renamed += (sender, e) => LogChange("Переименование", $"{e.OldFullPath} -> {e.FullPath}");

                watcher.EnableRaisingEvents = true;

                Console.WriteLine("Отслеживание запущено. Нажмите любую клавишу для завершения.");
                Console.ReadKey();
            }
            Console.ReadKey();
        }
    }

}
