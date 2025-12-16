using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public class NavigationGrid : MonoBehaviour
{
    Node[,] grid;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    public LayerMask unwalkableMask;
    float nodeDiameter;
    int gridSizeX, gridSizeY;


    private void Start()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y/ nodeDiameter);
        CreateGrid();
    }

    
    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];

        // get the bottom left point of the grid object
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x/2 - Vector3.forward * gridWorldSize.y/2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                // get each point that a node will occupy in the world
                Vector3 worldPoint = worldBottomLeft + 
                    Vector3.right * 
                    (x * nodeDiameter + nodeRadius) + 
                    Vector3.forward * 
                    (y * nodeDiameter + nodeRadius);

                // for each of these points, do a collision check
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));
                
                // populate the grid object
                grid[x, y] = new Node(walkable, worldPoint);
            }
        }
            
    }


    private void OnDrawGizmos()
    {
        // draw a gizmo for the size of the grid (debugging)
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

        // outline each space on the grid
        if (grid != null)
        {
            foreach (Node n in grid)
            {
                if (n.walkable)
                {
                    Gizmos.color = Color.white;
                }
                else
                {
                    Gizmos.color = Color.red;
                }
                Gizmos.DrawCube(n.worldPos, Vector3.one * (nodeDiameter - 0.1f));
            }
        }
    }
}
