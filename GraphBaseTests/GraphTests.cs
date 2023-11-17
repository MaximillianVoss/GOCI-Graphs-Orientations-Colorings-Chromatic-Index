using GraphBase;

namespace GraphBaseTests
{
    [TestClass]
    public class GraphTests
    {
        [TestMethod]
        public void ConstructorTest()
        {
            int[,] matrix = { { 0, 1 }, { 1, 0 } };
            Graph graph = new Graph(matrix);

            Assert.AreEqual(2, graph.VerticesCount);
            CollectionAssert.AreEqual(matrix, graph.AdjacencyMatrix);
        }

        [TestMethod]
        public void ToG6Test()
        {
            int[,] matrix = { { 0, 1 }, { 1, 0 } };
            Graph graph = new Graph(matrix);

            string g6String = graph.ToG6();
            Assert.AreEqual("A_", g6String);
        }

        [TestMethod]
        public void FromG6Test()
        {
            // ��� ������ ������������ ���� � ����� ��������� � ����� ������ ����� ����
            string g6String = "A_"; // ASCII for '_' is 95, which gives us 32 in our encoding, representing an edge
            Graph graph = Graph.FromG6(g6String);

            int[,] expectedMatrix = { { 0, 1 }, { 1, 0 } }; // ��������� ������� ���������
            for (int i = 0; i < graph.VerticesCount; i++)
            {
                for (int j = 0; j < graph.VerticesCount; j++)
                {
                    Assert.AreEqual(expectedMatrix[i, j], graph.AdjacencyMatrix[i, j],
                                    "The adjacency matrix does not match the expected matrix at position [" + i + ", " + j + "]");
                }
            }
        }

        [TestMethod]
        public void Constructor_FromG6String_CreatesExpectedGraph()
        {
            // Arrange
            // ��� ������ ������������ ���� �� ���� ������ � ����� ������ ����� ����
            string g6String = "A_"; // ������ ���� �������� ������������� ������ ����� � ������� G6
            int[,] expectedAdjacencyMatrix = { { 0, 1 }, { 1, 0 } };

            // Act
            Graph graph = new Graph(g6String);

            // Assert
            Assert.AreEqual(expectedAdjacencyMatrix.GetLength(0), graph.VerticesCount, "The number of vertices should match.");
            for (int i = 0; i < graph.VerticesCount; i++)
            {
                for (int j = 0; j < graph.VerticesCount; j++)
                {
                    Assert.AreEqual(expectedAdjacencyMatrix[i, j], graph.AdjacencyMatrix[i, j],
                                    $"The adjacency matrix does not match the expected value at position [{i}, {j}].");
                }
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_EmptyG6String_ThrowsArgumentException()
        {
            // Arrange
            string g6String = "";

            // Act
            Graph graph = new Graph(g6String);

            // Assert is handled by the ExpectedException attribute
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_TooLargeG6String_ThrowsArgumentException()
        {
            // Arrange
            string g6String = "~~..."; // ������������ �������� ��� ������������ �����

            // Act
            Graph graph = new Graph(g6String);

            // Assert is handled by the ExpectedException attribute
        }

        [TestMethod]
        public void Constructor_DegreeVector_CreatesExpectedGraph()
        {
            // Arrange
            int[] degreeVector = new int[] { 2, 2, 0 }; // ��� ������� �� �������� 2 � ���� �� �������� 0
            int[,] expectedAdjacencyMatrix = new int[,]
            {
                { 0, 1, 0 }, // ������� 1 ��������� � �������� 2
                { 1, 0, 0 }, // ������� 2 ��������� � �������� 1
                { 0, 0, 0 }  // ������� 3 �� ��������� �� � ����� ��������
            };

            // Act
            var graph = new Graph(degreeVector);

            // Assert
            Assert.AreEqual(degreeVector.Length, graph.VerticesCount, "The number of vertices should match.");
            for (int i = 0; i < graph.VerticesCount; i++)
            {
                for (int j = 0; j < graph.VerticesCount; j++)
                {
                    Assert.AreEqual(expectedAdjacencyMatrix[i, j], graph.AdjacencyMatrix[i, j],
                                    $"The adjacency matrix does not match the expected value at position [{i}, {j}].");
                }
            }
        }

        [TestMethod]
        public void GenerateDegreeVector_ValidGraph_ReturnsCorrectDegreeVector()
        {
            // Arrange
            // ������� ���� � ��������� �������� ��������
            // ��������, ���� � ����� ��������� � ����� �������: A-B, A-C
            int[,] adjacencyMatrix = new int[,] { { 0, 1, 1 }, { 1, 0, 0 }, { 1, 0, 0 } };
            var graph = new Graph(adjacencyMatrix);
            int[] expectedVector = new int[] { 2, 1, 1 }; // ������� ������: A=2, B=1, C=1

            // Act
            int[] actualVector = graph.GenerateDegreeVector();

            // Assert
            CollectionAssert.AreEqual(expectedVector, actualVector, "The degree vector does not match the expected vector.");
        }

        [TestMethod]
        public void GenerateDegreeVector_EmptyGraph_ReturnsEmptyVector()
        {
            // Arrange
            var graph = new Graph(new int[0, 0]);

            // Act
            int[] actualVector = graph.GenerateDegreeVector();

            // Assert
            Assert.AreEqual(0, actualVector.Length, "The degree vector for an empty graph should be empty.");
        }
    }
}