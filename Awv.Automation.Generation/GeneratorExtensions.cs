using Awv.Automation.Generation.Interface;
using System.Collections.Generic;

namespace Awv.Automation.Generation
{
    public static class GeneratorExtensions
    {
        public static IEnumerable<TGenerationType> GenerateMany<TGenerationType>(this IGenerator<TGenerationType> generator, IRNG random, int count)
        {
            var list = new List<TGenerationType>();

            for (var i = 0; i < count; i++)
                list.Add(generator.Generate(random));

            return list;
        }

        public static IEnumerable<TGenerationType> GenerateManyUnique<TGenerationType>(this IGenerator<TGenerationType> generator, IRNG random, int count, int retryMax = 10)
        {
            var list = new List<TGenerationType>();
            var retries = 0;
            for (var i = 0; i < count; i++)
            {
                var instance = generator.Generate(random);
                if (instance == null)
                    break;
                if (!list.Contains(instance))
                {
                    list.Add(instance);
                }
                else if (retries < retryMax)
                {
                    retries++;
                    --i;
                }
                else
                {
                    break;
                }
            }

            return list;
        }
    }
}
