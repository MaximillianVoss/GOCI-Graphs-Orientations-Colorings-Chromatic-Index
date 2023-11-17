using System;
using GraphBase.Параметры;

namespace GraphBase.Графы
{
    public abstract class Graph
    {
        #region Поля
        public int[,] _adjacencyMatrix;
        #endregion

        #region Свойства
        /// <summary>
        /// Получает матрицу смежности графа.
        /// </summary>
        public int[,] AdjacencyMatrix => _adjacencyMatrix;

        /// <summary>
        /// Получает количество вершин в графе.
        /// </summary>
        public int VerticesCount => _adjacencyMatrix.GetLength(0);
        #endregion

        #region Конструкторы/Деструкторы
        // Конструкторы остаются без изменений
        #endregion

        #region Абстрактные методы
        public abstract int GetChromaticNumber();
        public abstract int GetChromaticIndex();
        public abstract int GetDistinguishingNumber(int maxColors);
        #endregion
    }
}
