global using TastyIO;
using IOBenchmarks.Benchmarks;
using System.Reflection;

public static class Program
{
    public static readonly ConsoleColor FOREGROND_COLOR = ConsoleColor.Green;
    public static readonly ConsoleColor BACKGROUND_COLOR = ConsoleColor.Black;
    static void Main()
    {
        var benchmarkResults =  new List<BenchmarkResult>();

        var entryAssembly = Assembly.GetEntryAssembly();
        if (entryAssembly == null)
            throw new NullReferenceException($"{nameof(entryAssembly)} cannot be null.");

        foreach(var type in GetTypesWithBenchmarkAttribute(entryAssembly))
        {
            var a = (IBenchmark?)Activator.CreateInstance(type);
            if(a != null)
            {
                a.Benchmark();
            }
        }

        Console.ReadLine();
    }

    //Yoinked from https://stackoverflow.com/questions/607178/how-enumerate-all-classes-with-custom-class-attribute
    static IEnumerable<Type> GetTypesWithBenchmarkAttribute(Assembly assembly)
    {
        foreach (Type type in assembly.GetTypes())
        {
            if (type.GetCustomAttributes(typeof(BenchmarkAttribute), true).Length > 0)
            {
                yield return type;
            }
        }
    }
}
