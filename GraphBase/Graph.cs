namespace GraphBase
{
    public class Graph
    {
        #region Поля

        private int[,] _adjacencyMatrix;

        #endregion

        #region Свойства

        /// <summary>
        /// Получает матрицу смежности графа.
        /// </summary>
        public int[,] AdjacencyMatrix
        {
            get => this._adjacencyMatrix;
            private set => this._adjacencyMatrix = value;
        }

        /// <summary>
        /// Получает количество вершин в графе.
        /// </summary>
        public int VerticesCount => this._adjacencyMatrix.GetLength(0);

        #endregion

        #region Конструкторы/Деструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Graph"/> с использованием заданной матрицы смежности.
        /// </summary>
        /// <param name="adjacencyMatrix">Матрица смежности графа.</param>
        public Graph(int[,] adjacencyMatrix)
        {
            this.AdjacencyMatrix = adjacencyMatrix;
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Graph"/> из строки в формате G6.
        /// </summary>
        /// <param name="g6">Строка, представляющая граф в формате G6.</param>
        public Graph(string g6)
        {
            int n;

            if (g6.Length == 0)
                throw new ArgumentException("The G6 string is empty.");

            string R_x;
            if (g6[0] == '~')
            {
                if (g6.Length > 1 && g6[1] == '~')
                    throw new ArgumentException("Graph6 string indicates a graph too large to handle."); // n > 258047
                n = Convert.ToInt32(Rx(1, 3, g6), 2);
                R_x = Rx(4, g6.Length - 1, g6);
            }
            else
            {
                n = g6[0] - 63;
                R_x = Rx(1, g6.Length - 1, g6);
            }

            this._adjacencyMatrix = new int[n, n];
            int k = 0;
            for (int j = 1; j < n; j++)
            {
                for (int i = 0; i < j; i++)
                {
                    this._adjacencyMatrix[i, j] = R_x[k] == '1' ? 1 : 0;
                    this._adjacencyMatrix[j, i] = R_x[k] == '1' ? 1 : 0;
                    k++;
                }
            }
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Graph"/> с использованием вектора степеней.
        /// Создаёт граф, в котором каждая вершина соединена с каждой, если степень больше 0.
        /// </summary>
        /// <param name="degreeVector">Вектор степеней вершин.</param>
        public Graph(int[] degreeVector)
        {
            int n = degreeVector.Length;
            this._adjacencyMatrix = new int[n, n];

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (i != j && degreeVector[i] > 0 && degreeVector[j] > 0)
                    {
                        this._adjacencyMatrix[i, j] = 1;
                    }
                }
            }
        }
        #endregion

        #region Методы

        #region Формат G6
        /// <summary>
        /// Создаёт граф из строки в формате G6.
        /// </summary>
        /// <param name="g6">Строка, представляющая граф в формате G6.</param>
        /// <returns>Экземпляр класса <see cref="Graph"/>.</returns>
        public static Graph FromG6(string g6)
        {
            int n;

            if (g6.Length == 0)
                return new Graph(new int[0, 0]);

            string R_x;
            if (g6.ElementAt(0) == '~')
            {
                if (g6.ElementAt(1) == '~')
                    throw new ArgumentException("Graph6 string indicates a graph too large to handle."); // n > 258047
                n = Convert.ToInt32(Rx(1, 3, g6), 2);
                R_x = Rx(4, g6.Length - 1, g6);
            }
            else
            {
                n = g6.ElementAt(0) - 63;
                R_x = Rx(1, g6.Length - 1, g6);
            }

            int[,] AdjMatrix = new int[n, n];
            int k = 0;
            for (int j = 1; j < n; j++)
            {
                for (int i = 0; i < j; i++)
                {
                    AdjMatrix[i, j] = R_x[k] == '1' ? 1 : 0;
                    AdjMatrix[j, i] = R_x[k] == '1' ? 1 : 0;
                    k++;
                }
            }
            return new Graph(AdjMatrix);
        }
        /// <summary>
        /// Конвертирует граф в строковое представление в формате G6.
        /// </summary>
        /// <returns>Строка, представляющая граф в формате G6.</returns>
        public string ToG6()
        {
            var sb = new System.Text.StringBuilder();
            _ = sb.Append((char)(this.VerticesCount + 63));  // Используем 63 как стартовый символ для кодирования числа вершин

            int bitIndex = 0;
            int currentByte = 0;
            for (int i = 0; i < this.VerticesCount; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    // Добавляем бит в текущий байт
                    if (this.AdjacencyMatrix[i, j] == 1)
                    {
                        currentByte |= 1 << (5 - bitIndex); // 5 - bitIndex потому что мы начинаем с самого старшего бита
                    }

                    bitIndex++;
                    if (bitIndex == 6) // Когда набрали 6 битов, конвертируем их в символ и добавляем к строке
                    {
                        _ = sb.Append((char)(currentByte + 63));
                        bitIndex = 0;
                        currentByte = 0;
                    }
                }
            }

            if (bitIndex > 0) // Добавляем оставшийся байт, если он есть
            {
                _ = sb.Append((char)(currentByte + 63));
            }

            return sb.ToString();
        }
        /// <summary>
        /// Преобразует заданный сегмент строки в формате G6 в бинарное представление.
        /// </summary>
        /// <param name="init">Начальный индекс сегмента строки G6 для преобразования.</param>
        /// <param name="end">Конечный индекс сегмента строки G6 для преобразования.</param>
        /// <param name="g6">Строка в формате G6, представляющая граф.</param>
        /// <returns>Бинарная строка, представляющая сегмент строки G6.</returns>
        /// <remarks>
        /// Каждый символ строки G6, начиная с индекса <paramref name="init"/> до <paramref name="end"/>,
        /// преобразуется в 6-битное бинарное число, которое добавляется к результирующей строке.
        /// Строка G6 кодирует граф, где каждый символ (после учета смещения ASCII на 63) представляет 6 бит,
        /// соответствующих рёбрам графа.
        /// </remarks>
        private static String Rx(int init, int end, string g6)
        {
            string R_x = "";

            for (int i = init; i <= end; i++)
            {
                string bin6 = Convert.ToString(g6.ElementAt(i) - 63, 2).PadLeft(6, '0');
                R_x += bin6;
            }
            return R_x;
        }
        #endregion

        #region Вектор степеней
        // Статический метод для генерации вектора степеней
        public static int[] GenerateDegreeVector(Graph graph)
        {
            var degreeVector = new int[graph.VerticesCount];
            for (int i = 0; i < graph.VerticesCount; i++)
            {
                degreeVector[i] = graph.GetRow(i).Sum();
            }
            return degreeVector;
        }
        // Вспомогательный метод для получения строки матрицы смежности
        public int[] GetRow(int rowIndex)
        {
            var row = new int[AdjacencyMatrix.GetLength(1)];
            for (int i = 0; i < AdjacencyMatrix.GetLength(1); i++)
                row[i] = AdjacencyMatrix[rowIndex, i];
            return row;
        }
        public int[] GenerateDegreeVector()
        {
            return Graph.GenerateDegreeVector(this);
        }
        #endregion

        #region Канонический код

        #endregion

        #endregion

        #region Операторы

        // Операторы

        #endregion

        #region Обработчики событий

        // Обработчики событий

        #endregion
    }
}
