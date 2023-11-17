﻿using System;
using System.Collections.Generic;
using System.Text;

namespace GraphBase.Параметры
{
    public class CanonicalGraphCode
    {
        #region Поля
        private string hash;
        private List<Tuple<int, int>> edges; // Список рёбер для восстановления графа
        #endregion

        #region Свойства
        public string Hash
        {
            get => this.hash;
            private set => this.hash = value;
        }

        public List<Tuple<int, int>> Edges
        {
            get => this.edges;
            private set => this.edges = value;
        }
        #endregion

        #region Конструкторы/Деструкторы
        public CanonicalGraphCode(AdjacencyMatrix matrix)
        {
            this.hash = GenerateHashFromMatrix(matrix);
            this.edges = ExtractEdgesFromMatrix(matrix);
        }
        #endregion

        #region Методы
        private string GenerateHashFromMatrix(AdjacencyMatrix matrix)
        {
            var stringBuilder = new StringBuilder();
            for (int i = 0; i < matrix.Matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.Matrix.GetLength(1); j++)
                {
                    stringBuilder.Append(matrix.Matrix[i, j]);
                }
            }
            return stringBuilder.ToString().GetHashCode().ToString();
        }

        private List<Tuple<int, int>> ExtractEdgesFromMatrix(AdjacencyMatrix matrix)
        {
            List<Tuple<int, int>> edges = new List<Tuple<int, int>>();
            for (int i = 0; i < matrix.Matrix.GetLength(0); i++)
            {
                for (int j = i + 1; j < matrix.Matrix.GetLength(1); j++)
                {
                    if (matrix.Matrix[i, j] == 1)
                        edges.Add(Tuple.Create(i, j));
                }
            }
            return edges;
        }

        public AdjacencyMatrix ToAdjacencyMatrix()
        {
            int size = this.edges.Count > 0 ? this.edges.Max(edge => Math.Max(edge.Item1, edge.Item2)) + 1 : 0;
            var matrix = new int[size, size];

            foreach (var edge in this.edges)
            {
                matrix[edge.Item1, edge.Item2] = 1;
                matrix[edge.Item2, edge.Item1] = 1;
            }

            return new AdjacencyMatrix(matrix);
        }

        public static CanonicalGraphCode FromAdjacencyMatrix(AdjacencyMatrix matrix)
        {
            return new CanonicalGraphCode(matrix);
        }
        #endregion

        #region Операторы
        // Место для операторов, если они будут добавлены в будущем
        #endregion

        #region Обработчики событий
        // Место для обработчиков событий, если они будут добавлены в будущем
        #endregion
    }
}
