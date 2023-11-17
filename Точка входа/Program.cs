using GraphBase.Графы;
using GraphBase.Генераторы;
using System;
using System.Diagnostics;
using System.IO;
using GraphBase.Параметры;
using System.Windows.Forms;

class Program
{
    // Установите значение флага DEBUG в true, чтобы выводить сообщения об ошибках, или в false, чтобы их подавлять
    private const bool DEBUG = true;

    [STAThread] // Необходимо для корректной работы с элементами GUI в консольном приложении
    static void Main(string[] args)
    {
        try
        {
            Console.Write("Введите количество вершин для генерации графов: ");
            int vertexCount = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Выберите метод генерации графов:");
            Console.WriteLine("1 - Генерация с помощью вектора степеней");
            Console.WriteLine("2 - Генерация с помощью канонического кода");
            int methodChoice = Convert.ToInt32(Console.ReadLine());

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Текстовые файлы (*.txt)|*.txt",
                Title = "Сохранение отчета",
                FileName = $"Отчет граф {vertexCount} вершин.txt"
            };

            if (saveFileDialog.ShowDialog() != DialogResult.OK)
            {
                Console.WriteLine("Сохранение отчета отменено пользователем.");
                return;
            }

            string reportFilePath = saveFileDialog.FileName;
            var generator = new GeneratorNaughty(vertexCount);

            using (var reportFile = new StreamWriter(reportFilePath))
            {
                var totalStopwatch = Stopwatch.StartNew();

                foreach (var g6 in generator.GenerateAllGraphsG6())
                {
                    try
                    {
                        AdjacencyMatrix adjacencyMatrix = new G6String(g6).ToAdjacencyMatrix();
                        Graph graph = null;

                        switch (methodChoice)
                        {
                            case 1:
                                DegreeVector degreeVector = adjacencyMatrix.ToDegreeVector();
                                graph = new GraphCustom(degreeVector);
                                break;
                            case 2:
                                CanonicalGraphCode canonicalGraphCode = adjacencyMatrix.ToCanonicalGraphCode();
                                graph = new GraphCustom(canonicalGraphCode);
                                break;
                            default:
                                throw new InvalidOperationException("Неизвестный метод генерации графов.");
                        }

                        var graphInfo = graph.GetInfo();
                        Console.WriteLine(graphInfo);
                        reportFile.WriteLine(graphInfo);
                    }
                    catch (Exception ex)
                    {
                        if (DEBUG)
                        {
                            Console.WriteLine($"Ошибка при обработке графа: {ex.Message}");
                            reportFile.WriteLine($"Ошибка при обработке графа: {ex.Message}");
                        }
                    }
                }

                totalStopwatch.Stop();
                var totalTimeInfo = $"Общее время выполнения: {totalStopwatch.ElapsedMilliseconds} мс";

                Console.WriteLine(totalTimeInfo);
                reportFile.WriteLine(totalTimeInfo);
            }
        }
        catch (Exception ex)
        {
            if (DEBUG)
            {
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
            }
        }
    }
}
