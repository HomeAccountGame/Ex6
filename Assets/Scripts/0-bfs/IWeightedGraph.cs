using System.Collections.Generic;


/**
 * An abstract weighted graph.
 * It extends the IGraph interface and adds the functionality of edge weights.
 * T = type of node in graph.
 * @author Erel Segal-Halevi
 * @since 2020-12
 */
public interface IWeightedGraph<T> : IGraph<T>
{
    public float GetW(T node1);
}
