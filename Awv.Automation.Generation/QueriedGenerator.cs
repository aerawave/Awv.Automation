using Awv.Automation.Generation.Interface;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Awv.Automation.Generation
{
    public class QueriedGenerator<TQueryableResource, TQueryResult> : IGenerator<TQueryResult>, IEnumerable<TQueryResult>
    {
        public TQueryableResource Resource { get; set; }
        public Func<TQueryableResource, IEnumerable<TQueryResult>> Query { get; set; }

        private PredefinedGenerator<TQueryResult> Generator { get; set; } = new PredefinedGenerator<TQueryResult>(new TQueryResult[0]);

        public QueriedGenerator(TQueryableResource resource)
        {
            Resource = resource;
        }

        public QueriedGenerator(TQueryableResource resource, Func<TQueryableResource, IEnumerable<TQueryResult>> query)
            : this(resource)
        {
            Query = query;
        }

        public QueriedGenerator(IEnumerable<TQueryResult> values)
        {
            Query = res => values;
        }

        public IEnumerable<TQueryResult> QueryDefault() => Query(Resource);

        public TQueryResult Generate(IRNG random)
        {
            if (Query == null)
                throw new ArgumentNullException(nameof(Query));

            Generator.Values = QueryDefault();

            return Generator.Generate(random);
        }

        public IEnumerator<TQueryResult> GetEnumerator() => QueryDefault().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => QueryDefault().GetEnumerator();
    }

    public static class QueriedGeneratorExt
    {
        public static QueriedGenerator<IEnumerable<TQueryResult>, TQueryResult> AsGenerator<TQueryResult>(this IEnumerable<TQueryResult> enumerable)
            => new QueriedGenerator<IEnumerable<TQueryResult>, TQueryResult>(enumerable, enumer => enumer);
    }
}
