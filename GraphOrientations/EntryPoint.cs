using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommandLine;
using GraphOrientations.Writers;

namespace GraphOrientations
{
    internal class EntryPoint
    {
        static void Main(string[] args)
        {
            var consoleArguments = new Options();
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(o =>
                {
                    var startTime = DateTime.Now;
                    var reader = new GraphsReader();
                    var mapper = new GraphsMapper();
                    var orientator = new GraphOrientator();
                    IWriter writer = o.WriteGraphsToFile ? new FileWriterCustom(o.FileName) : new ConsoleWriter();
                    var n = o.VertexCount;

                    var totalCount = 0;

                    var automorphismReader = new AutomorphismGroupRepository();

                    var groupSizeToCount = new Dictionary<int, (int graphsCount, int OrientationsTotalCount)>();

                    if (o.CalculateOnly)
                    {
                        foreach (var g6Graph in reader.ReadGraphs(n))
                        {       
                            var graph = mapper.FromG6(g6Graph);
                            var groupSize = automorphismReader.GetNextAutomorphismGroupSize(g6Graph);
                            var currentOrientationsCount = orientator.Orient(graph).Count();
                            totalCount += currentOrientationsCount;

                            if (groupSizeToCount.TryGetValue(groupSize, out var value))
                            { 
                                groupSizeToCount[groupSize] = (value.graphsCount + 1, value.OrientationsTotalCount + currentOrientationsCount);
                            }
                            else
                            {
                                groupSizeToCount.Add(groupSize, (1, currentOrientationsCount));
                            }

                            Console.WriteLine($"GroupSizeOf {g6Graph} = {groupSize}; Orientations count = {currentOrientationsCount}");
                        }

                        Console.WriteLine();
                        Console.WriteLine($"Total orientations count: {totalCount}.");
                        Console.WriteLine($"Total calculating time in seconds: {(DateTime.Now - startTime).TotalSeconds}.");
                        Console.WriteLine();
                        Console.WriteLine("Average number of orientations depending on the size of the group:");

                        List<string> graphsCountByGroupSize = new List<string>();
                        List<string> orientationsAverageByGroupSize = new List<string>();
                        foreach (var kvp in groupSizeToCount.OrderBy(x => x.Key))
                        {
                            Console.WriteLine($"Group size: {kvp.Key}; Orientations count: {(kvp.Value.OrientationsTotalCount + .0) / kvp.Value.graphsCount:#.##}");
                            graphsCountByGroupSize.Add($"({kvp.Key};{kvp.Value.OrientationsTotalCount})");
                            orientationsAverageByGroupSize.Add($"({kvp.Key};{(kvp.Value.OrientationsTotalCount + .0) / kvp.Value.graphsCount})");
                        }

                        File.WriteAllText($"GraphsCountByGroupSize_{n}.txt", string.Join(" ", graphsCountByGroupSize));
                        File.WriteAllText($"OrientationsAverageByGroupSize_{n}.txt", string.Join(" ", orientationsAverageByGroupSize));

                        var d = groupSizeToCount.OrderBy(x => x.Key).ToList();

                        return;
                    }

                    // don't use it, too mutch data in console will do slow
                    foreach (var g6Graph in reader.ReadGraphs(n))
                    {
                        var graph = mapper.FromG6(g6Graph);

                        var groupSize = automorphismReader.GetNextAutomorphismGroupSize(g6Graph);
                        writer.WriteLine($"Current graph: {g6Graph}; Group size: {groupSize}");
                        writer.WriteLine();

                        foreach (var (oriented, _) in orientator.Orient(graph))
                        {
                            for (int i = 0; i < n; i++)
                            {
                                for (int j = 0, jMask = 1; j < n; j++, jMask <<= 1)
                                {
                                    writer.Write((oriented[i] & jMask) == 0 ? 0 : 1);
                                }
                                writer.WriteLine();
                            }
                            writer.WriteLine();
                            totalCount++;
                        }
                        writer.WriteLine();                
                    }

                    Console.WriteLine($"Total graphs count: {totalCount}.");
                    Console.WriteLine($"Total calculating time in seconds: {(DateTime.Now - startTime).TotalSeconds}.");
                });
        }
    }
}
