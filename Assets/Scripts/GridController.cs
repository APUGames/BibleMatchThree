using System;
using UnityEngine;

public class GridController : MonoBehaviour
{
    // This script will manage the grid of pieces

    [SerializeField]
    private GameObject piecePrefab;
    [SerializeField]
    private Vector3 originPosition;

    [Header("Piece Colors")]
    [SerializeField]
    private Material pieceOneMaterial;
    [SerializeField]
    private Material pieceSecondMaterial;

    // Section for tuning

    private Piece[,] grid = new Piece[8, 8];

    // [[0, 1, 2, 3, 4], [], [], []]

    // Start is called before the first frame update
    void Start()
    {
        // Debug.Log(grid);

        System.Random rand = new System.Random();

        for (int row = 0; row < grid.GetLength(0); row++)
        {
            for (int column = 0; column < grid.GetLength(1); column++)
            {
                // 4-0=4, 4-1=3, 4-2=2, 4-3=1, 4-4=0, 4-5=-1, 4-6=-2, 4-7=-3
                Vector3 newWorldPosition = new Vector3(originPosition.x + row, originPosition.y, originPosition.z - column);
                grid[row, column] = new Piece(newWorldPosition, new Vector2(row, column));
                // Debug.Log(grid[row, column]);

                GameObject gameObject = Instantiate(piecePrefab, grid[row, column].GetPosition(), Quaternion.identity);
                int theNumber = rand.Next(13, 101);
                // Debug.Log(theNumber);
                if (theNumber > 23 && theNumber < 55)
                {
                    // Debug.Log("changing color");
                    // Get the Renderer component from the new game object
                    var gameObjectRenderer = gameObject.GetComponent<Renderer>();

                    // Call SetColor using the shader property name "_Color" and setting the color to red
                    gameObjectRenderer.material = pieceOneMaterial;
                }
                else if (theNumber >= 55 && theNumber < 85)
                {
                    // Debug.Log("changing color");
                    // Get the Renderer component from the new game object
                    var gameObjectRenderer = gameObject.GetComponent<Renderer>();

                    // Call SetColor using the shader property name "_Color" and setting the color to red
                    gameObjectRenderer.material = pieceSecondMaterial;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
