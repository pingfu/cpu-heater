using System;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace cpu_heater
{
    public class Program
    {
        public static Stopwatch Sw = new Stopwatch();

        public static void Main(string[] args)
        {
            const int work = 20000000;

            var cores = Enumerable.Range(0, Environment.ProcessorCount).ToArray();

            var perCoreWork = work / Environment.ProcessorCount;

            Console.WriteLine($"Scheduling {work:n0} million SHA512 computations across {Environment.ProcessorCount} core{(Environment.ProcessorCount > 1 ? "s" : "")} ({perCoreWork:n0} computations per core) ... ");

            Sw.Start();

            Parallel.ForEach(cores, core => LongCalculation(core, perCoreWork));

            Sw.Stop();

            Console.WriteLine($"\nAll cores completed in {Sw.ElapsedMilliseconds}ms");
            Console.ReadLine();
        }

        private static void LongCalculation(int core, int work)
        {
            var bytes = new ASCIIEncoding().GetBytes("hello world");

            using (var sha = new SHA512Managed())
            {
                for (var n = 0; n < work; n++)
                {
                    sha.ComputeHash(bytes);
                }
            }

            Console.WriteLine($" Core #{core}: Work complete in {Sw.ElapsedMilliseconds:n0}ms");
        }
    }
}
