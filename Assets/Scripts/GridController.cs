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
    [SerializeField]
    private Material pieceThirdMaterial;
    [SerializeField]
    private Material pieceFourMaterial;

    public bool pressedDown;
    public Vector2 pressedDownPosition;
    public Vector2 pressedUpPosition;
    public GameObject pressedDownGameObject;
    public GameObject pressedUpGameObject;

    private Vector2 startMovementPiecePosition;
    private Vector2 endMovementPiecePosition;

    private bool validMoveInProcess = false;

    // Section for tuning

    private Piece[,] grid = new Piece[8, 8];

    // [[0, 1, 2, 3, 4], [], [], []]

    // Start is called before the first frame update
    void Start()
    {
        // Debug.Log(grid);

        pressedDown = false;

        System.Random rand = new System.Random();

        for (int row = 0; row < grid.GetLength(0); row++)
        {
            for (int column = 0; column < grid.GetLength(1); column++)
            {
                // 4-0=4, 4-1=3, 4-2=2, 4-3=1, 4-4=0, 4-5=-1, 4-6=-2, 4-7=-3
                Vector3 newWorldPosition = new Vector3(originPosition.x + row, originPosition.y, originPosition.z - column);
                Piece newPiece = new Piece(newWorldPosition, new Vector2(row, column));
                
                // Debug.Log(grid[row, column]);

                GameObject gameObject = Instantiate(piecePrefab, newPiece.GetPosition(), Quaternion.identity);
                int theNumber = rand.Next(13, 101);
                // Debug.Log(theNumber);
                if (theNumber > 30 && theNumber < 45)
                {
                    // Debug.Log("changing color");
                    // Get the Renderer component from the new game object
                    var gameObjectRenderer = gameObject.GetComponent<Renderer>();

                    // Call SetColor using the shader property name "_Color" and setting the color to red
                    gameObjectRenderer.material = pieceOneMaterial;

                    newPiece.SetPieceType(PieceTypes.Elisha);
                }
                else if (theNumber >= 45 && theNumber < 60)
                {
                    // Debug.Log("changing color");
                    // Get the Renderer component from the new game object
                    var gameObjectRenderer = gameObject.GetComponent<Renderer>();

                    // Call SetColor using the shader property name "_Color" and setting the color to red
                    gameObjectRenderer.material = pieceFourMaterial;

                    newPiece.SetPieceType(PieceTypes.Lamb);
                }
                else if (theNumber >= 60 && theNumber < 85)
                {
                    // Debug.Log("changing color");
                    // Get the Renderer component from the new game object
                    var gameObjectRenderer = gameObject.GetComponent<Renderer>();

                    // Call SetColor using the shader property name "_Color" and setting the color to red
                    gameObjectRenderer.material = pieceSecondMaterial;

                    newPiece.SetPieceType(PieceTypes.Andrew);
                }
                else if (theNumber >= 85 && theNumber < 101)
                {
                    // Debug.Log("changing color");
                    // Get the Renderer component from the new game object
                    var gameObjectRenderer = gameObject.GetComponent<Renderer>();

                    // Call SetColor using the shader property name "_Color" and setting the color to red
                    gameObjectRenderer.material = pieceThirdMaterial;

                    newPiece.SetPieceType(PieceTypes.Hannah);
                }

                // Set new piece to game object's piece controller
                PieceController controller = gameObject.GetComponent<PieceController>();
                controller.SetPiece(newPiece);

                grid[row, column] = newPiece;
            }
        }
    }

    private void Update()
    {
        if (validMoveInProcess)
        {
            Debug.Log("Switching");

            Vector3 placeHolderPosition = pressedDownGameObject.transform.position;
            pressedDownGameObject.transform.position = pressedUpGameObject.transform.position;
            pressedUpGameObject.transform.position = placeHolderPosition;

            validMoveInProcess = false;
        }
    }

    public void ValidMove(Vector2 start, Vector2 end)
    {
        Debug.Log("validating start (" + start.x + ", " + start.y + ") | end (" + end.x + ", " + end.y + ")");
        startMovementPiecePosition = start;
        endMovementPiecePosition = end;

        // Get type of piece based on start position
        // and check for neighboring pieces of the
        // same type below and above the end position
        Piece topPiece = grid[(int)end.x, (int)end.y-1];
        Piece bottomPiece = grid[(int)end.x, (int)end.y+1];
        Piece midPiece = grid[(int)end.x, (int)end.y];
        if (topPiece.GetPieceType() == bottomPiece.GetPieceType())
        {
            if (topPiece.GetPieceType() == midPiece.GetPieceType())
            {
                validMoveInProcess = true;
            }
        }

        Piece leftPiece = grid[(int)end.x-1, (int)end.y];
        Piece leftLeftPiece = grid[(int)end.x-2, (int)end.y];
        Piece endPiece = grid[(int)end.x, (int)end.y];
        if (leftPiece.GetPieceType() == leftLeftPiece.GetPieceType())
        {
            if (leftPiece.GetPieceType() == endPiece.GetPieceType())
            {
                validMoveInProcess = true;
            }
        }

        Debug.Log("not valid move");
    }
}
