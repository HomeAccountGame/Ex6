using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;
using System.Collections.Generic;

/**
 * This component just keeps a list of allowed tiles.
 * Such a list is used both for pathfinding and for movement.
 */
public class AllowedTiles : MonoBehaviour
{
    [SerializeField] TileBase[] allowedTiles = null;
    [SerializeField] float[] weights;
    Dictionary<TileBase, float> weights_dic = new Dictionary<TileBase, float>();
    private void Awake()
    {
        if (weights.Length<1)
        {
            for (int i = 0; i < allowedTiles.Length; i++)
            {
                weights_dic.Add(allowedTiles[i], Random.Range(0f, 5f));
            }
        }
        else
        {
            for (int i = 0; i < allowedTiles.Length; i++)
            {
                weights_dic.Add(allowedTiles[i], weights[i]);
            }
        }
    }
    public bool Contain(TileBase tile) {
        return allowedTiles.Contains(tile);
    }

    public TileBase[] Get() { return allowedTiles;}
    public float GetW(TileBase tile) { return weights_dic[tile]; }
}
