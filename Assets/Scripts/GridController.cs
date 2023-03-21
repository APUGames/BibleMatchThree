using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    // This script will manage the grid of pieces

    [SerializeField]
    private GameObject piecePrefab;

    [SerializeField]
    private Vector3 originPosition;

    private Piece[,] grid = new Piece[8, 8];

    // [[0, 1, 2, 3, 4], [], [], []]

    // Start is called before the first frame update
    void Start()
    {
        // Debug.Log(grid);

        for (int row = 0; row < grid.GetLength(0); row++)
        {
            for (int column = 0; column < grid.GetLength(1); column++)
            {
                // 4-0=4, 4-1=3, 4-2=2, 4-3=1, 4-4=0, 4-5=-1, 4-6=-2, 4-7=-3
                Vector3 newWorldPosition = new Vector3(originPosition.x + row, originPosition.y, originPosition.z - column);
                grid[row, column] = new Piece(newWorldPosition, new Vector2(row, column));
                // Debug.Log(grid[row, column]);

                Instantiate(piecePrefab, grid[row, column].GetPosition(), Quaternion.identity);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
