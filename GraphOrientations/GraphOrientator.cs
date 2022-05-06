using System.Collections.Generic;
using System.Linq;

namespace GraphOrientations
{
    internal class GraphOrientator
    {
        public IEnumerable<(int[] graph, int groupSize)> Orient(int[] graph)
        {
            var codes = new HashSet<long>();
            var substitutions = Utils.EnumerateAllSubstitutions(graph.Length).ToArray();

            foreach (var orientedGraph in OrientInternal(graph))
            {
                var code = Utils.GetGraphCode(orientedGraph);

                if (codes.Contains(code))
                {
                    continue;
                }

                var maxCode = -1L;
                var groupSize = 1;
                foreach (var substitution in substitutions)
                {
                    var currentGraph = Utils.UseSubstitution(orientedGraph, substitution);
                    var currentCode = Utils.GetGraphCode(currentGraph);

                    if (currentCode > maxCode)
                    {
                        maxCode = currentCode;
                        groupSize = 1;
                    }
                    else if (currentCode == maxCode)
                    {
                        groupSize++;
                    }

                    codes.Add(currentCode);
                }

                yield return (orientedGraph, groupSize);
            }
        }

        private IEnumerable<int[]> OrientInternal(int[] graph, int from = 0, int to = 1)
        {        
            if (from >= graph.Length)
            {
                yield return graph;
            }
            else
            {
                var toMask = 1 << to;
                int nextFrom = to >= graph.Length - 1 ? from + 1 : from;
                int nextTo = to == graph.Length - 1 ? nextFrom + 1 : to + 1;

                if ((graph[from] & toMask) != 0)
                {
                    var fromMask = 1 << from;
                    graph[to] ^= fromMask;
                    foreach (var val in OrientInternal(graph, nextFrom, nextTo))
                    {
                        yield return val;
                    }
                    graph[to] ^= fromMask;

                    graph[from] ^= toMask;
                    foreach (var val in OrientInternal(graph, nextFrom, nextTo))
                    {
                        yield return val;
                    }
                    graph[from] ^= toMask;
                }
                else
                {
                    foreach (var val in OrientInternal(graph, nextFrom, nextTo))
                    {
                        yield return val;
                    }
                }
            }
        }
    }
}
