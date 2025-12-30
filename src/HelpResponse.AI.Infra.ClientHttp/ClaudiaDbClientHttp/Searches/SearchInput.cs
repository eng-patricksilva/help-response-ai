using System.Collections.Generic;

namespace HelpResponse.AI.Infra.ClientHttp.ClaudiaDbClientHttp.Searches
{
    public class SearchInput(bool count,
                             string select,
                             int top,
                             string filter,
                             float[] vector,
                             string fields,
                             string kind)
    {
        public bool Count { get; set; } = count;
        public string Select { get; set; } = select;
        public int Top { get; set; } = top;
        public string Filter { get; set; } = filter;
        public IEnumerable<Vectorquery> VectorQueries { get; set; } = [new(vector, top, fields, kind)];
    }

    public class Vectorquery(float[] vector,
                             int k,
                             string fields,
                             string kind)
    {
        public float[] Vector { get; set; } = vector;
        public int K { get; set; } = k;
        public string Fields { get; set; } = fields;
        public string Kind { get; set; } = kind;
    }
}