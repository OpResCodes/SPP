using DataGenerator.Dijkstra;
using ShortestPaths.Dijkstra;
using System.Threading.Tasks;

namespace PerformanceTests
{
    public class TestClass
    {

        public TestClass()
        {
            var t1 = Task.Factory.StartNew(() =>
            {
                var gen = new GraphGenerator();
                SmallInstance = gen.GenerateRandomGraph(200, 0.3);
            });
            var t2 = Task.Factory.StartNew(() =>
            {
                var gen = new GraphGenerator();
                MediumInstance = gen.GenerateRandomGraph(2000, 0.3);
            });
            var t3 = Task.Factory.StartNew(() =>
            {
                var gen = new GraphGenerator();
                BigInstance = gen.GenerateRandomGraph(4000, 0.3);
            });
            Task.WaitAll(t1, t2, t3);
        }

        public Graph SmallInstance;
        public Graph MediumInstance;
        public Graph BigInstance;

        public void TestInstance(Graph Instance)
        {
            var calculator = new Calculator(Instance);
            calculator.CalculateShortestPathTree(0);
        }

    }
}
