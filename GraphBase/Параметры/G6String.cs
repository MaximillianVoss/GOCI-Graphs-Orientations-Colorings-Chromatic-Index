using System;
using System.Collections.Generic;
using System.Text;

namespace GraphBase.Параметры
{
    public class G6String
    {
        #region Поля
        private string g6;
        #endregion

        #region Свойства
        public string G6
        {
            get => this.g6;
            private set => this.g6 = value;
        }
        #endregion

        #region Конструкторы/Деструкторы
        public G6String(AdjacencyMatrix adjacencyMatrix)
        {
            this.G6 = ConvertAdjacencyMatrixToG6String(adjacencyMatrix);
        }
        #endregion

        #region Методы
        private string ConvertAdjacencyMatrixToG6String(AdjacencyMatrix adjacencyMatrix)
        {
            var sb = new StringBuilder();
            sb.Append((char)(adjacencyMatrix.Matrix.GetLength(0) + 63));

            int bitIndex = 0;
            int currentByte = 0;
            for (int i = 0; i < adjacencyMatrix.Matrix.GetLength(0); i++)
            {
                for (int j = 0; j < i; j++)
                {
                    if (adjacencyMatrix.Matrix[i, j] == 1)
                    {
                        currentByte |= 1 << (5 - bitIndex);
                    }

                    bitIndex++;
                    if (bitIndex == 6)
                    {
                        sb.Append((char)(currentByte + 63));
                        bitIndex = 0;
                        currentByte = 0;
                    }
                }
            }

            if (bitIndex > 0)
            {
                sb.Append((char)(currentByte + 63));
            }

            return sb.ToString();
        }

        public AdjacencyMatrix ToAdjacencyMatrix()
        {
            int n = this.G6[0] - 63;
            if (n < 0)
                throw new ArgumentException("Invalid G6 string.");

            string R_x = Rx(1, this.G6.Length - 1, this.G6);
            int[,] matrix = new int[n, n];
            int k = 0;
            for (int j = 1; j < n; j++)
            {
                for (int i = 0; i < j; i++)
                {
                    matrix[i, j] = matrix[j, i] = R_x[k] == '1' ? 1 : 0;
                    k++;
                }
            }

            return new AdjacencyMatrix(matrix);
        }

        private static string Rx(int init, int end, string g6)
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

        #region Операторы
        // Место для операторов, если они будут добавлены в будущем
        #endregion

        #region Обработчики событий
        // Место для обработчиков событий, если они будут добавлены в будущем
        #endregion
    }
}
