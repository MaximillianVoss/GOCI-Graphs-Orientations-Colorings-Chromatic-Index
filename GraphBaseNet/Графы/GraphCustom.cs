using GraphBase.Параметры;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace GraphBase.Графы
{
    public class GraphCustom : Graph
    {
        #region Поля

        #endregion

        #region Свойства

        #endregion

        #region Конструкторы/Деструкторы
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="GraphCustom"/> с использованием заданной матрицы смежности.
        /// </summary>
        /// <param name="adjacencyMatrix">Матрица смежности графа.</param>
        public GraphCustom(int[,] adjacencyMatrix)
        {
            this._adjacencyMatrix = adjacencyMatrix ?? throw new ArgumentNullException(nameof(adjacencyMatrix));
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="GraphCustom"/> из объекта <see cref="DegreeVector"/>.
        /// </summary>
        /// <param name="degreeVector">Объект <see cref="DegreeVector"/>, представляющий вектор степеней вершин.</param>
        public GraphCustom(DegreeVector degreeVector)
        {
            if (degreeVector == null)
                throw new ArgumentNullException(nameof(degreeVector));

            this._adjacencyMatrix = degreeVector.ToAdjacencyMatrix().Matrix;
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="GraphCustom"/> из объекта <see cref="G6String"/>.
        /// </summary>
        /// <param name="g6String">Объект <see cref="G6String"/>, представляющий граф в формате G6.</param>
        public GraphCustom(G6String g6String)
        {
            if (g6String == null)
                throw new ArgumentNullException(nameof(g6String));

            this._adjacencyMatrix = g6String.ToAdjacencyMatrix().Matrix;
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="GraphCustom"/> из объекта <see cref="CanonicalGraphCode"/>.
        /// </summary>
        /// <param name="canonicalCode">Объект <see cref="CanonicalGraphCode"/>, представляющий канонический код графа.</param>
        public GraphCustom(CanonicalGraphCode canonicalCode)
        {
            if (canonicalCode == null)
                throw new ArgumentNullException(nameof(canonicalCode));

            this._adjacencyMatrix = canonicalCode.ToAdjacencyMatrix().Matrix;
        }
        #endregion

        #region Методы

        #region Хроматическое число графа
        //Хроматическое число графа - это минимальное количество цветов, 
        //необходимое для окраски всех вершин графа таким образом, 
        //чтобы никакие две смежные вершины не были окрашены в один и тот же цвет.

        ///<summary>
        ///Вычисляет хроматическое число графа,
        ///используя жадный алгоритм.Хроматическое число - это минимальное количество цветов,
        ///необходимое для окраски вершин графа,
        ///чтобы никакие две смежные вершины не были окрашены в один цвет.
        ///</summary>
        ///<remarks>
        ///Этот метод использует простой жадный алгоритм для окраски вершин.
        ///Алгоритм начинает с первой вершины и окрашивает каждую вершину в минимально доступный цвет, 
        ///который ещё не использовался среди её соседей.
        ///Хотя этот метод не гарантирует нахождение оптимального хроматического числа для всех графов, 
        ///он эффективен и дает приемлемые результаты для многих типов графов, 
        ///особенно для малых и средних по размеру.
        ///</remarks>
        /// <returns>
        ///Возвращает минимальное количество цветов, 
        ///необходимое для окраски всех вершин графа, 
        ///чтобы никакие две смежные вершины не были окрашены в один цвет.
        ///</returns>
        public override int GetChromaticNumberGreedy()
        {
            int[] colorAssignments = new int[this.VerticesCount];
            for (int i = 0; i < this.VerticesCount; i++)
                colorAssignments[i] = -1;

            colorAssignments[0] = 0; // Назначаем первому узлу цвет 0

            bool[] availableColors = new bool[this.VerticesCount];
            for (int i = 0; i < this.VerticesCount; i++)
                availableColors[i] = true;

            for (int u = 1; u < this.VerticesCount; u++)
            {
                for (int i = 0; i < this.VerticesCount; i++)
                {
                    if (this._adjacencyMatrix[u, i] == 1 && colorAssignments[i] != -1)
                        availableColors[colorAssignments[i]] = false;
                }

                int cr;
                for (cr = 0; cr < this.VerticesCount; cr++)
                {
                    if (availableColors[cr])
                        break;
                }

                colorAssignments[u] = cr; // Назначаем узлу u минимально доступный цвет

                for (int i = 0; i < this.VerticesCount; i++)
                    availableColors[i] = true; // Сброс доступных цветов для следующего узла
            }

            return colorAssignments.Max() + 1; // Возвращает количество использованных цветов
        }

        /// <summary>
        /// Вычисляет хроматическое число графа, используя переборный алгоритм.
        /// </summary>
        /// <remarks>
        /// Этот метод исследует все возможные способы раскраски графа, чтобы найти минимальное количество цветов,
        /// необходимое для того, чтобы никакие две смежные вершины не были окрашены в один и тот же цвет.
        /// Алгоритм полного перебора может быть очень ресурсоемким для больших графов, так как число комбинаций
        /// раскрасок растет экспоненциально с увеличением количества вершин.
        /// </remarks>
        /// <returns>Минимальное количество цветов, необходимое для раскраски всех вершин графа.</returns>
        public int GetChromaticNumberBruteForce()
        {
            int n = this.VerticesCount; // Количество вершин в графе
            int[] colors = new int[n]; // Массив для хранения цветов вершин

            int minColors = n; // Начальное значение - максимально возможное количество цветов

            // Рекурсивный метод для перебора всех раскрасок
            void TryColors(int v)
            {
                if (v == n) // Если все вершины окрашены
                {
                    minColors = Math.Min(minColors, colors.Distinct().Count()); // Обновляем минимальное количество цветов
                    return;
                }

                for (int color = 1; color <= minColors; color++) // Перебор цветов
                {
                    bool safeToColor = true;
                    for (int i = 0; i < n; i++) // Проверка смежности с другими вершинами
                    {
                        if (this._adjacencyMatrix[v, i] == 1 && colors[i] == color) // Если смежная вершина уже окрашена в этот цвет
                        {
                            safeToColor = false;
                            break;
                        }
                    }

                    if (safeToColor) // Если можно окрасить в данный цвет
                    {
                        colors[v] = color; // Окрашиваем вершину
                        TryColors(v + 1); // Продолжаем для следующей вершины
                        colors[v] = 0; // Сброс цвета (для следующих итераций)
                    }
                }
            }

            TryColors(0); // Начинаем с первой вершины
            return minColors; // Возвращаем минимальное найденное количество цветов
        }

        #endregion

        #region Хроматический индекс графа
        //Хроматический индекс графа - это минимальное количество цветов, 
        //необходимое для окраски всех рёбер графа так, чтобы рёбра, 
        //инцидентные одной вершине, были разных цветов.

        ///<summary>
        ///Вычисляет хроматический индекс графа, используя жадный алгоритм.Хроматический индекс - это минимальное количество цветов, необходимое для окраски всех рёбер графа таким образом, чтобы смежные рёбра имели разные цвета.
        ///</summary>
        ///<remarks>
        ///Этот метод использует жадный подход для окраски каждого ребра графа. Для каждого ребра метод проверяет доступные цвета, исключая цвета, уже используемые смежными рёбрами, и выбирает минимально доступный цвет.Этот алгоритм не гарантирует нахождение оптимального хроматического индекса для всех графов, но он эффективен и даёт приемлемые результаты для многих типов графов, особенно для небольших.
        ///</remarks>
        ///<returns>
        ///Возвращает хроматический индекс графа, то есть минимальное количество цветов, необходимое для окраски всех рёбер графа, так чтобы смежные рёбра имели разные цвета.
        ///</returns>
        public override int GetChromaticIndex()
        {
            // Предполагаем, что каждое ребро изначально не окрашено
            int[,] edgeColors = new int[this.VerticesCount, this.VerticesCount];
            for (int i = 0; i < this.VerticesCount; i++)
            {
                for (int j = 0; j < this.VerticesCount; j++)
                    edgeColors[i, j] = -1;
            }

            int maxColor = 0;

            // Жадно окрашиваем каждое ребро
            for (int u = 0; u < this.VerticesCount; u++)
            {
                for (int v = 0; v < this.VerticesCount; v++)
                {
                    if (this._adjacencyMatrix[u, v] != 1 || edgeColors[u, v] != -1)
                        continue;

                    bool[] availableColors = new bool[this.VerticesCount];
                    for (int i = 0; i < availableColors.Length; i++)
                    {
                        availableColors[i] = true;
                    }


                    // Проверяем цвета смежных рёбер
                    for (int i = 0; i < this.VerticesCount; i++)
                    {
                        if (this._adjacencyMatrix[u, i] == 1 && edgeColors[u, i] != -1)
                        {
                            if (edgeColors[u, i] < this.VerticesCount) // Убедимся, что индекс находится в пределах массива
                            {
                                availableColors[edgeColors[u, i]] = false;
                            }
                        }
                        if (this._adjacencyMatrix[v, i] == 1 && edgeColors[v, i] != -1)
                        {
                            if (edgeColors[v, i] < this.VerticesCount) // Аналогично для v
                            {
                                availableColors[edgeColors[v, i]] = false;
                            }
                        }
                    }


                    // Выбираем первый доступный цвет
                    int cr;
                    for (cr = 0; cr < this.VerticesCount; cr++)
                    {
                        if (availableColors[cr])
                            break;
                    }

                    edgeColors[u, v] = edgeColors[v, u] = cr; // Окрашиваем ребро
                    maxColor = Math.Max(maxColor, cr + 1);
                }
            }

            return maxColor;
        }


        #endregion

        #region Различительное число
        public static IEnumerable<List<int>> GetPermutations(List<int> list)
        {
            list.Sort(); // Начинаем с сортированного списка
            yield return list.ToList(); // Возвращаем первую перестановку

            int n = list.Count;
            while (true)
            {
                int i;
                for (i = n - 2; i >= 0; i--)
                {
                    if (list[i] < list[i + 1])
                        break;
                }

                if (i < 0)
                    yield break; // Все перестановки сгенерированы

                int j;
                for (j = n - 1; j > i; j--)
                {
                    if (list[j] > list[i])
                        break;
                }

                // Обмен значениями
                (list[i], list[j]) = (list[j], list[i]);

                // Переворот списка от i+1 до конца
                list.Reverse(i + 1, n - (i + 1));

                yield return list.ToList();
            }
        }

        public bool IsAutomorphism(List<int> permutation, int[] colors)
        {
            int n = this._adjacencyMatrix.GetLength(0);
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (this._adjacencyMatrix[i, j] != this._adjacencyMatrix[permutation[i], permutation[j]] ||
                        colors[i] != colors[permutation[i]])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public override int GetDistinguishingNumber(int maxColors = 1024)
        {
            int n = this._adjacencyMatrix.GetLength(0);
            int[] colors = new int[n];

            for (int currentMaxColors = 1; currentMaxColors <= maxColors; currentMaxColors++)
            {
                if (this.TryColoring(colors, 0, currentMaxColors))
                    return currentMaxColors;
            }

            return -1; // Возвращает -1, если различительное число больше maxColors
        }

        private bool TryColoring(int[] colors, int index, int maxColors)
        {
            if (index == colors.Length)
            {
                return this.CheckAllAutomorphismsBroken(colors);
            }

            for (int color = 1; color <= maxColors; color++)
            {
                colors[index] = color;
                if (this.TryColoring(colors, index + 1, maxColors))
                    return true;
            }

            return false;
        }

        private bool CheckAllAutomorphismsBroken(int[] colors)
        {
            IEnumerable<List<int>> permutations = GetPermutations(Enumerable.Range(0, this._adjacencyMatrix.GetLength(0)).ToList());
            return permutations.All(permutation => !this.IsAutomorphism(permutation, colors));
        }

        #endregion

        #region Подсчет характеристик

        public override string GetInfo(int numberOfColors = 0)
        {
            // Получаем G6-представление графа
            AdjacencyMatrix adjacencyMatrix = new AdjacencyMatrix(this.AdjacencyMatrix);
            string g6String = new G6String(adjacencyMatrix).G6;

            // Измеряем время выполнения жадного метода
            Stopwatch stopwatch = Stopwatch.StartNew();
            int chromaticNumberGreedy = this.GetChromaticNumberGreedy();
            stopwatch.Stop();
            long timeGreedy = stopwatch.ElapsedMilliseconds;

            // Измеряем время выполнения переборного метода
            stopwatch.Restart();
            int chromaticNumberBruteForce = this.GetChromaticNumberBruteForce();
            stopwatch.Stop();
            long timeBruteForce = stopwatch.ElapsedMilliseconds;

            // Если результаты различаются, используем жадный результат
            //if (chromaticNumberBruteForce != chromaticNumberGreedy)
            //    chromaticNumberBruteForce = chromaticNumberGreedy;

            // Формируем итоговую строку
            return $"G6-представление: {g6String}, " +
                   $"Вектор степеней: {adjacencyMatrix.ToDegreeVector()}, " +
                   $"Хроматическое число (жадный метод): {chromaticNumberGreedy} (выполнено за {timeGreedy} мс), " +
                   $"Хроматическое число (переборный метод): {chromaticNumberBruteForce} (выполнено за {timeBruteForce} мс)";
        }

        #endregion

        #endregion
    }
}
