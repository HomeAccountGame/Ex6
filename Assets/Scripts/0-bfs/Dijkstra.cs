using System;
using System.Collections.Generic;
using System.Linq;

using System.Collections;

public class Dijkstra
{
    public static Dictionary<NodeType, NodeType> FindPath<NodeType>(IWeightedGraph<NodeType> graph, NodeType startNode)
    {
        Dictionary<NodeType, float> distances = new Dictionary<NodeType, float>();
        Dictionary<NodeType, NodeType> previous = new Dictionary<NodeType, NodeType>();
        HashSet<NodeType> unvisited = new HashSet<NodeType>();
        Queue<NodeType> queue = new Queue<NodeType>();

        queue.Enqueue(startNode);
        distances[startNode] = 0f;
        while (queue.Count > 0)
        {
            NodeType n = queue.Peek();
            if (!unvisited.Contains(n))
            {
                unvisited.Add(n);
                queue.Dequeue();

                foreach (NodeType node in graph.Neighbors(n))
                {
                    if (!unvisited.Contains(node))
                    {
                        queue.Enqueue(node);
                    }
                    if (distances.ContainsKey(node))
                    {
                        if (distances[node] > distances[n] + graph.GetW(node))
                        {
                            distances[node] = distances[n] + graph.GetW(node);
                            previous[node] = n;
                        }
                    }
                    else
                    {
                        distances[node] = distances[n] + +graph.GetW(node);
                        previous[node] = n;
                    }
                }
            }
            else
            {
                queue.Dequeue();
            }
        }

            

        return previous;
    }

    public static List<NodeType> GetPath<NodeType>(IWeightedGraph<NodeType> graph, NodeType startNode, NodeType endNode)
    {
        Dictionary<NodeType, NodeType> previous = FindPath(graph, startNode);
        List<NodeType> path = new List<NodeType>();

        if (!previous.ContainsKey(endNode))
        {
            // There is no path from startNode to endNode
            return path;
        }

        path.Add(endNode);
        NodeType current = endNode;

        while (!current.Equals(startNode))
        {
            current = previous[current];
            path.Insert(0, current);
        }

        return path;
    }
}



    