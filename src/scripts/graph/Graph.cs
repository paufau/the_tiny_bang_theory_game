using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Utils.Graph
{
    public class Graph<T> where T : class
    {
        public List<T> Nodes = new();
        public List<GraphLine<T>> Lines = new();

        public void AddNode(T node)
        {
            Nodes.Add(node);
        }

        public List<GraphLine<T>> ConnectRange(T node1, List<T> nodes)
        {
            List<GraphLine<T>> lines = new();

            foreach (var node in nodes)
            {
                lines.Add(Connect(node1, node));
            }

            return lines;
        }

        public GraphLine<T> Connect(T node1, T node2)
        {
            GraphLine<T> connectionLine = GetConnectionLine(node1, node2);
            if (connectionLine == null)
            {
                connectionLine = new GraphLine<T>(node1, node2);
                Lines.Add(connectionLine);
            }

            if (!HasNode(node1))
            {
                AddNode(node1);
            }

            if (!HasNode(node2))
            {
                AddNode(node2);
            }

            return connectionLine;
        }

        public void RemoveNode(T node)
        {
            foreach(var connection in GetConnections(node))
            {
                Disconnect(node, connection);
            }
            Nodes.Remove(node);
        }

        public void Disconnect(T node1, T node2)
        {
            Lines.Remove(GetConnectionLine(node1, node2));
        }

        public bool HasNode(T node)
        {
            return Nodes.Contains(node);
        }

        public bool IsConnected(T node1, T node2)
        {
            return GetConnectionLine(node1, node2) != null;
        }

        public GraphLine<T>? GetConnectionLine(T node1, T node2)
        {
            foreach (var line in Lines)
            {
                if ((line.Begin.Equals(node1) && line.End.Equals(node2)) || (
                    line.Begin.Equals(node2) && line.End.Equals(node1)
                )) return line;
            }
            return null;
        }

        public List<T> GetConnections(T node)
        {
            return Nodes.Where(n => IsConnected(n, node)).ToList();
        }
    }
}

