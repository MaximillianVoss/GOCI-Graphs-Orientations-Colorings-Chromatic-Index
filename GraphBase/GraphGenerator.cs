using System;
using System.Text;

namespace GraphBase
{
    /// <summary>
    /// Класс для генерации графов в формате G6.
    /// </summary>
    public class GraphGenerator
    {
        #region Поля

        private Random random;

        #endregion

        #region Конструкторы/Деструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="GraphGenerator"/>.
        /// </summary>
        public GraphGenerator()
        {
            random = new Random();
        }

        #endregion

        #region Методы

        /// <summary>
        /// Генерирует случайный граф в формате G6.
        /// </summary>
        /// <param name="vertices">Количество вершин в графе.</param>
        /// <returns>Строка, представляющая граф в формате G6.</returns>
        public string GenerateGraphG6(int vertices)
        {
            StringBuilder graphString = new StringBuilder();

            // Добавляем количество вершин в кодировке
            char vertexCount = (char)(vertices + 63);
            graphString.Append(vertexCount);

            int totalPairs = (vertices * (vertices - 1)) / 2;
            int byteValue = 0;
            int bitPosition = 0;

            // Генерация рёбер графа
            for (int i = 0; i < totalPairs; i++)
            {
                if (random.Next(0, 2) == 1)
                {
                    byteValue |= 1 << bitPosition;
                }

                bitPosition++;

                if (bitPosition == 6 || i == totalPairs - 1)
                {
                    graphString.Append((char)(byteValue + 63));
                    byteValue = 0;
                    bitPosition = 0;
                }
            }

            return graphString.ToString();
        }

        /// <summary>
        /// Создает объект <see cref="Graph"/> из строки в формате G6.
        /// </summary>
        /// <param name="g6">Строка, представляющая граф в формате G6.</param>
        /// <returns>Экземпляр класса <see cref="Graph"/>.</returns>
        public Graph CreateGraphFromG6(string g6)
        {
            int[,] adjacencyMatrix = ConvertG6ToAdjacencyMatrix(g6);
            return new Graph(adjacencyMatrix);
        }

        /// <summary>
        /// Преобразует строку в формате G6 в матрицу смежности.
        /// </summary>
        /// <param name="g6">Строка в формате G6.</param>
        /// <returns>Матрица смежности графа.</returns>
        private int[,] ConvertG6ToAdjacencyMatrix(string g6)
        {
            if (string.IsNullOrEmpty(g6))
            {
                throw new ArgumentException("G6 string cannot be null or empty");
            }

            int n = g6[0] - 63; // Получение количества вершин

            int[,] adjacencyMatrix = new int[n, n];

            int index = 1; // Стартовый индекс для чтения битов рёбер
            int bitCount = 0; // Счетчик битов

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    if (bitCount == 0)
                    {
                        if (index < g6.Length)
                        {
                            bitCount = 6; // Обновление счетчика битов
                            index++; // Переход к следующему символу
                        }
                        else
                        {
                            throw new ArgumentException("Invalid G6 format: unexpected end of string");
                        }
                    }

                    // Установка значения в матрице смежности
                    adjacencyMatrix[i, j] = adjacencyMatrix[j, i] = (g6[index - 1] - 63) >> (6 - bitCount) & 1;

                    bitCount--;
                }
            }

            return adjacencyMatrix;
        }

        #endregion
    }
}
