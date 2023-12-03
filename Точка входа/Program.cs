using GraphBase.Генераторы;
using GraphBase.Графы;
using GraphBase.Параметры;
using System.Diagnostics;

class Program
{
    /// <summary>
    /// Для тестирования некоторых графов непосредственно
    /// </summary>
    static void Test()
    {
        var g6Test = new G6String("GV~~~{");
        var vectorTst = g6Test.ToAdjacencyMatrix().DegreeVector;
        var adjacencyMatrixTest = g6Test.ToAdjacencyMatrix();
        var testGraph = new GraphCustom(g6Test);
        var infoTets = testGraph.ToString();
        var testGraph2 = new GraphCustom(adjacencyMatrixTest.ToDegreeVector());
        var testGraph3 = new GraphCustom(adjacencyMatrixTest.ToCanonicalGraphCode());
    }
    static void Main(string[] args)
    {
        //Test();
        bool DEBUG = false; // Значение флага DEBUGk
        try
        {
            Console.Write("Введите количество вершин для генерации графов: ");
            int vertexCount = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Выберите метод генерации графов:");
            Console.WriteLine("1 - Генерация с помощью вектора степеней");
            Console.WriteLine("2 - Генерация с помощью канонического кода");
            int methodChoice = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Выберите тип графа:");
            Console.WriteLine("1 - Пользовательский");
            Console.WriteLine("2 - QuickGraph");
            int graphTypeChoice = Convert.ToInt32(Console.ReadLine());

            var generator = new GeneratorNaughty(vertexCount);
            string generationMethod = methodChoice == 1 ? "Вектор степеней" : "Канонический код";
            string graphType = graphTypeChoice == 1 ? "Пользовательский" : "QuickGraph";

            string reportFileName = $"Отчет граф {vertexCount} вершин - {generationMethod} - {graphType}.txt";

            using (var reportFile = new StreamWriter(reportFileName))
            {
                var totalStopwatch = Stopwatch.StartNew();
                int graphNumber = 1; // Счетчик номера графа

                //foreach (string g6 in generator.GenerateAllGraphsG6(vertexCount, GeneratorType.BY_CANONICAL_CODE))
                foreach (string g6 in generator.GenerateAllGraphsG6(vertexCount, GeneratorType.CONNECTED_GRAPHS))
                {
                    try
                    {
                        var adjacencyMatrix = new G6String(g6).ToAdjacencyMatrix();
                        Graph graph = null;

                        switch (methodChoice)
                        {
                            case 1:
                                var degreeVector = adjacencyMatrix.ToDegreeVector();
                                //graph = graphTypeChoice == 1 ? new GraphCustom(degreeVector) : new GraphQuickGraph(adjacencyMatrix.Matrix);
                                graph = new GraphCustom(degreeVector);
                                break;
                            case 2:
                                var canonicalGraphCode = adjacencyMatrix.ToCanonicalGraphCode();
                                //graph = graphTypeChoice == 1 ? new GraphCustom(canonicalGraphCode) : new GraphQuickGraph(adjacencyMatrix.Matrix);
                                graph = new GraphCustom(canonicalGraphCode);
                                break;
                            default:
                                throw new InvalidOperationException("Неизвестный метод генерации графов.");
                        }

                        // Выводим номер графа перед информацией
                        string graphNumStr = $"Граф #{graphNumber}:";
                        Console.Write(graphNumStr);
                        reportFile.Write(graphNumStr);


                        string graphInfo = graph.ToString();
                        Console.WriteLine(graphInfo);
                        reportFile.WriteLine(graphInfo);

                        graphNumber++; // Увеличиваем счетчик номера графа
                    }
                    catch (Exception ex)
                    {
                        if (DEBUG)
                        {
                            Console.WriteLine($"Ошибка при обработке графа {g6}: {ex.Message}");
                            reportFile.WriteLine($"Ошибка при обработке графа: {ex.Message}");
                        }
                    }
                }

                totalStopwatch.Stop();
                string totalTimeInfo = $"Общее время выполнения: {totalStopwatch.ElapsedMilliseconds} мс";

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
