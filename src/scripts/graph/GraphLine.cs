namespace Utils.Graph
{
    public class GraphLine<T>
    {
        public T Begin;
        public T End;

        public GraphLine(T Begin, T End)
        {
            this.Begin = Begin;
            this.End = End;
        }
    }
}

