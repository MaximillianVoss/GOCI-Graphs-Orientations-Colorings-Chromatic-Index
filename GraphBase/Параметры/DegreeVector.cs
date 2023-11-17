﻿using System;
using System.Collections.Generic;

namespace GraphBase.Параметры
{
    public class DegreeVector
    {
        #region Поля
        private int[] degrees;
        private List<Tuple<int, int>> edges; // Список рёбер
        #endregion

        #region Свойства
        public int[] Degrees
        {
            get => this.degrees;
        }

        public List<Tuple<int, int>> Edges
        {
            get => this.edges;
        }
        #endregion

        #region Конструкторы/Деструкторы
        // Простой конструктор
        public DegreeVector(int[] degrees)
        {
            this.degrees = degrees;
            this.edges = null; // Список рёбер не предоставлен
        }

        // Конструктор с дополнительной информацией
        public DegreeVector(int[] degrees, List<Tuple<int, int>> edges)
        {
            this.degrees = degrees;
            this.edges = edges;
        }
        #endregion

        #region Методы
        public AdjacencyMatrix ToAdjacencyMatrix()
        {
            if (this.edges == null)
            {
                throw new InvalidOperationException("Недостаточно данных для восстановления матрицы смежности.");
            }

            int size = this.degrees.Length;
            var matrix = new int[size, size];

            foreach (var edge in this.edges)
            {
                matrix[edge.Item1, edge.Item2] = 1;
                matrix[edge.Item2, edge.Item1] = 1;
            }

            return new AdjacencyMatrix(matrix);
        }

        // Другие методы класса DegreeVector
        #endregion

        #region Операторы
        // Место для операторов, если они будут добавлены в будущем
        #endregion

        #region Обработчики событий
        // Место для обработчиков событий, если они будут добавлены в будущем
        #endregion
    }
}
