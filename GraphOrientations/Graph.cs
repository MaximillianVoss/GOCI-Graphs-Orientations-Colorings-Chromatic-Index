using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphOrientations
{
    public class Graph
    {
        #region Поля
        #endregion

        #region Свойства
        /// <summary>
        /// Граф в формате G6
        /// </summary>
        public String G6 { set; get; }
        /// <summary>
        /// Матрица смежности
        /// </summary>
        public int[] AdjacencyMatrix { set; get; }
        /// <summary>
        /// Число вершин
        /// </summary>
        public int VertexCount => this.AdjacencyMatrix.Length;
        #endregion

        #region Методы
        /// <summary>
        /// Выполняет преобразование графа в формате G6 в матрицу смежности в виде одномерного массива целых чисел.
        /// </summary>
        /// <param name="strG6">
        /// Формат G6 (Graph6) является компактным способом представления неориентированных графов. 
        /// Он использует шестидесятичное кодирование для представления связей между вершинами графа.
        /// </param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public int[] FromG6(string strG6)
        {
            var n = strG6[0] - '?';
            var result = new int[n];

            Parallel.For(1, n, i =>
            {
                var rOffset = 32;
                var k = 1;
                var val = strG6[k] - '?';

                var offset = 1;

                for (int j = 0; j < i; j++)
                {
                    if ((val & rOffset) > 0)
                    {
                        lock (result)
                        {
                            result[i] |= offset;
                            result[j] |= 1 << i;
                        }
                    }

                    if ((rOffset >>= 1) == 0)
                    {
                        rOffset = 32;
                        if (++k < strG6.Length)
                        {
                            val = strG6[k] - '?';
                        }
                        else if (j != i - 1)
                        {
                            throw new Exception("Ошибка формата strG6");
                        }
                    }

                    offset <<= 1;
                }
            });

            return result;
        }

        public int[] FromG6()
        {
            return this.FromG6(this.G6);
        }

        // Хроматический индекс графа определяет минимальное количество цветов, которые можно использовать для покраски его рёбер таким образом, 
        // чтобы любые два смежных ребра имели разные цвета.Проблема нахождения хроматического индекса является NP-трудной для общего случая. 
        // Однако для некоторых классов графов существуют полиномиальные алгоритмы.
        // Для поиска хроматического индекса можно использовать метод ветвей и границ. Однако этот метод может быть медленным для больших графов.
        // Другой подход заключается в использовании метода поиска по графу цветов.
        // Вот простой метод для нахождения хроматического индекса с использованием "жадного" метода:
        public int ChromaticIndex()
        {
            int n = VertexCount;
            int[] color = new int[n];

            for (int i = 0; i < n; i++)
            {
                color[i] = -1;
            }

            List<int> availableColors = new List<int>(Enumerable.Range(0, n));

            for (int u = 0; u < n; u++)
            {
                List<int> neighbors = new List<int>();
                for (int v = 0; v < n; v++)
                {
                    if ((AdjacencyMatrix[u] & (1 << v)) != 0 && u != v)
                    {
                        neighbors.Add(v);
                    }
                }

                foreach (int v in neighbors)
                {
                    if (color[v] != -1)
                    {
                        availableColors.Remove(color[v]);
                    }
                }

                color[u] = availableColors.FirstOrDefault();

                foreach (int v in neighbors)
                {
                    if (color[v] != -1)
                    {
                        availableColors.Add(color[v]);
                    }
                }

                availableColors = new List<int>(Enumerable.Range(0, n));
            }

            return color.Max() + 1;
        }
        #endregion

        #region Конструкторы/Деструкторы
        public Graph(String g6)
        {
            this.G6 = g6 ?? throw new Exception("G6 строка графа не сожет быть null");
            this.AdjacencyMatrix = this.FromG6(this.G6);
        }
        #endregion

        #region Операторы

        #endregion

        #region Обработчики событий

        #endregion


    }
}
