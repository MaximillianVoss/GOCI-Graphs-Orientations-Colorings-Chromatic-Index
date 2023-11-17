using GraphBase.�����;
using GraphBase.���������;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace GraphBaseTests.����
{
    [TestClass]
    public class GraphTests
    {
        [TestMethod]
        public void ConstructorTest()
        {
            // ��������� ����������� � �������� ���������
            int[,] matrix = { { 0, 1 }, { 1, 0 } };
            GraphCustom graph = new GraphCustom(matrix);

            Assert.AreEqual(2, graph.VerticesCount);
            CollectionAssert.AreEqual(matrix, graph.AdjacencyMatrix);
        }

        [TestMethod]
        public void Constructor_FromG6String_CreatesExpectedGraph()
        {
            // ��������� ����������� �� ������� G6
            var g6String = new G6String("A_");
            int[,] expectedAdjacencyMatrix = { { 0, 1 }, { 1, 0 } };

            GraphCustom graph = new GraphCustom(g6String);

            Assert.AreEqual(expectedAdjacencyMatrix.GetLength(0), graph.VerticesCount);
            for (int i = 0; i < graph.VerticesCount; i++)
            {
                for (int j = 0; j < graph.VerticesCount; j++)
                {
                    Assert.AreEqual(expectedAdjacencyMatrix[i, j], graph.AdjacencyMatrix[i, j]);
                }
            }
        }

        [TestMethod]
        public void Constructor_DegreeVector_CreatesExpectedGraph()
        {
            // ��������� ����������� � �������� ��������
            int[] degreeVector = new int[] { 2, 2, 0 };
            var edges = new List<Tuple<int, int>> { Tuple.Create(0, 1), Tuple.Create(1, 0) }; // ���� ��������� ������ ����
            int[,] expectedAdjacencyMatrix = {
                { 0, 1, 0 },
                { 1, 0, 0 },
                { 0, 0, 0 }
            };

            GraphCustom graph = new GraphCustom(new DegreeVector(degreeVector, edges)); // ���������� ����������� � �������������� �����������

            Assert.AreEqual(degreeVector.Length, graph.VerticesCount);
            for (int i = 0; i < graph.VerticesCount; i++)
            {
                for (int j = 0; j < graph.VerticesCount; j++)
                {
                    Assert.AreEqual(expectedAdjacencyMatrix[i, j], graph.AdjacencyMatrix[i, j]);
                }
            }
        }

        [TestMethod]
        public void Constructor_CanonicalGraphCode_CreatesExpectedGraph()
        {
            // Arrange
            // ������� ������� ��������� � ��������������� ������ ����
            int[,] adjacencyMatrix = {
                { 0, 1, 0 },
                { 1, 0, 1 },
                { 0, 1, 0 }
            };


            // ������� ������������ ��� �� ������ ���� �������
            var canonicalCode = new CanonicalGraphCode(new AdjacencyMatrix(adjacencyMatrix));

            // Act
            // ������� ���� �� ������������� ����
            GraphCustom graph = new GraphCustom(canonicalCode);

            // Assert
            // ���������, ��� ������� ��������� ����� ������������� ���������
            Assert.AreEqual(adjacencyMatrix.GetLength(0), graph.VerticesCount, "Number of vertices should match.");
            for (int i = 0; i < graph.VerticesCount; i++)
            {
                for (int j = 0; j < graph.VerticesCount; j++)
                {
                    Assert.AreEqual(adjacencyMatrix[i, j], graph.AdjacencyMatrix[i, j],
                                    $"Adjacency matrix does not match expected value at position [{i}, {j}].");
                }
            }
        }

        [TestMethod]
        public void GetChromaticNumber_SimpleGraph_ReturnsExpectedNumber()
        {
            // �������� �������� �����
            // ����: A - B - C (�������� ����)
            int[,] adjacencyMatrix = new int[,]
            {
        { 0, 1, 0 },
        { 1, 0, 1 },
        { 0, 1, 0 }
            };
            GraphCustom graph = new GraphCustom(adjacencyMatrix);

            // ��������� ������������� ����� ��� ��������� ����� � ����� ��������� - 2
            int expectedChromaticNumber = 2;

            // ����������
            int actualChromaticNumber = graph.GetChromaticNumber();

            // ��������
            Assert.AreEqual(expectedChromaticNumber, actualChromaticNumber);
        }

        [TestMethod]
        public void GetChromaticIndex_SimpleGraph_ReturnsExpectedIndex()
        {
            // �������� �������� �����
            // ����: A - B - C (�������� ����)
            int[,] adjacencyMatrix = new int[,]
            {
        { 0, 1, 0 },
        { 1, 0, 1 },
        { 0, 1, 0 }
            };
            GraphCustom graph = new GraphCustom(adjacencyMatrix);

            // ��������� ������������� ������ ��� ��������� ����� � ����� ��������� - 2
            int expectedChromaticIndex = 2;

            // ����������
            int actualChromaticIndex = graph.GetChromaticIndex();

            // ��������
            Assert.AreEqual(expectedChromaticIndex, actualChromaticIndex);
        }

        [TestMethod]
        public void GetDistinguishingNumber_CompleteGraph_ReturnsVertexCount()
        {
            //// ������� ������ ���� � 3 ���������
            //int[,] adjacencyMatrix = new int[,]
            //{
            //    { 0, 1, 1 },
            //    { 1, 0, 1 },
            //    { 1, 1, 0 }
            //};
            //GraphCustom graph = new GraphCustom(adjacencyMatrix);

            //// ��� ������� ����� � 3 ��������� �������������� ����� ������ ���� 3
            //int expectedDistinguishingNumber = 3;
            //// �������� ����� GetDistinguishingNumber
            //int actualDistinguishingNumber = graph.GetDistinguishingNumber(); // ������������ ���������� ������

            //// ���������, ��� ���������� �������������� ����� ������������� ����������
            //Assert.AreEqual(expectedDistinguishingNumber, actualDistinguishingNumber,
            //                "The distinguishing number of a complete graph should equal the number of its vertices.");
        }

    }
}
