﻿using QuickGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace GraphBase.Генераторы
{
    /// <summary>
    /// Класс для генерации графов в формате G6.
    /// </summary>
    public class GraphGeneratorCustom : GraphGeneratorBase
    {
        #region Поля
#pragma warning disable CS0108 // Член скрывает унаследованный член: отсутствует новое ключевое слово
        private int vertexCount;
#pragma warning restore CS0108 // Член скрывает унаследованный член: отсутствует новое ключевое слово
        #endregion

        #region Конструкторы/Деструкторы
        public GraphGeneratorCustom(int vertexCount) : base(vertexCount)
        {
        }
        #endregion

        #region Методы
        public override IEnumerable<string> GenerateAllGraphsG6()
        {
            long totalGraphs = 1L << (this.vertexCount * (this.vertexCount - 1) / 2);
            for (long graphNumber = 0; graphNumber < totalGraphs; graphNumber++)
            {
                yield return this.ConvertToG6(graphNumber);
            }
        }

        public override IEnumerable<string> GenerateAllGraphsG6(int vertexCount, GeneratorType generatorType)
        {
            throw new NotImplementedException();
        }

        private string ConvertToG6(long graphNumber)
        {
            var sb = new StringBuilder();
            _ = sb.Append((char)(this.vertexCount + 63)); // Добавляем количество вершин

            int bitsProcessed = 0;
            int currentByte = 0;

            for (int i = 0; i < this.vertexCount; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    if ((graphNumber & (1L << bitsProcessed)) != 0)
                    {
                        currentByte |= 1 << (5 - bitsProcessed % 6);
                    }

                    bitsProcessed++;
                    if (bitsProcessed % 6 == 0 || bitsProcessed == this.vertexCount * (this.vertexCount - 1) / 2)
                    {
                        _ = sb.Append((char)(currentByte + 63));
                        currentByte = 0;
                    }
                }
            }

            return sb.ToString();
        }
        #endregion
    }
}
