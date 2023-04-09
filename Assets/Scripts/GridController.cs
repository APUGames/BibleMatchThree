using System;
using UnityEngine;
using UnityEngine.UI;

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

    [Header("UI")]
    [SerializeField]
    private GameObject matchesFoundText;

    private Vector2 startMovementPiecePosition;
    private Vector2 endMovementPiecePosition;

    private bool validMoveInProcess = false;

    private int matchesFound;

    // Section for tuning

    private Piece[,] grid = new Piece[8, 8];

    // [[0, 1, 2, 3, 4], [], [], []]

    // Start is called before the first frame update
    void Start()
    {
        // Debug.Log(grid);

        matchesFound = 0;

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

            // Update the visual layer
            Vector3 placeHolderPosition = pressedDownGameObject.transform.position;
            pressedDownGameObject.transform.position = pressedUpGameObject.transform.position;
            pressedUpGameObject.transform.position = placeHolderPosition;

            // Update the data layer to match the visual layer
            Piece placeHolderPiece = grid[(int)endMovementPiecePosition.x, (int)endMovementPiecePosition.y];
            grid[(int)endMovementPiecePosition.x, (int)endMovementPiecePosition.y] = grid[(int)startMovementPiecePosition.x, (int)startMovementPiecePosition.y];
            grid[(int)startMovementPiecePosition.x, (int)startMovementPiecePosition.y] = placeHolderPiece;

            validMoveInProcess = false;

            matchesFound += 1;
        }

        matchesFoundText.GetComponent<Text>().text = matchesFound.ToString();
    }

    private Piece GetGridPiece(int row, int column)
    {
        Piece foundPiece;
        try
        {
            foundPiece = grid[row, column];
            if (foundPiece == null || foundPiece.GetDestruction())
            {
                return null;
            }

            return foundPiece;
        }
        catch (IndexOutOfRangeException)  // CS0168
        {
            // Catch IndexOutOfRangeException when the grid is asked to retrieve a
            // piece from an unknown location.
        }

        return null;
    }

    private Piece GetGridPiece(int row, int column, bool isDestroyed)
    {
        Piece foundPiece;
        try
        {
            foundPiece = grid[row, column];
            if (foundPiece == null)
            {
                return null;
            }

            if (!isDestroyed)
            {
                return null;
            }

            return foundPiece;
        }
        catch (IndexOutOfRangeException)  // CS0168
        {
            // Catch IndexOutOfRangeException when the grid is asked to retrieve a
            // piece from an unknown location.
        }

        return null;
    }

    public void ValidMove(Vector2 start, Vector2 end)
    {
        Debug.Log("validating start (" + start.x + ", " + start.y + ") | end (" + end.x + ", " + end.y + ")");
        startMovementPiecePosition = start;
        endMovementPiecePosition = end;

        // Using this boolean value to not do subsequent matches
        bool matchFound = false;

        if (!matchFound)
        {
            // Get type of piece based on start position
            // and check for neighboring pieces of the
            // same type below and above the end position
            try
            {
                Piece topPiece1 = GetGridPiece((int)end.x, (int)end.y - 1);
                Piece bottomPiece1 = GetGridPiece((int)end.x, (int)end.y + 1);
                Debug.Log("Top piece type: " + topPiece1.GetPieceType());
                Debug.Log("Bottom piece type: " + bottomPiece1.GetPieceType());
                Piece midPiece1 = GetGridPiece((int)start.x, (int)start.y);
                Piece toDestroy1 = GetGridPiece((int)end.x, (int)end.y);
                Debug.Log("Mid piece type: " + midPiece1.GetPieceType());
                if (topPiece1.GetPieceType() == bottomPiece1.GetPieceType())
                {
                    if (topPiece1.GetPieceType() == midPiece1.GetPieceType())
                    {
                        matchFound = true;
                        validMoveInProcess = true;
                        topPiece1.SetForDestruction();
                        bottomPiece1.SetForDestruction();
                        toDestroy1.SetForDestruction();
                        Debug.Log("======= MATCHED =======");
                    }
                }
            }
            catch (NullReferenceException)
            {
                // Object reference not set to an instance of an object
            }
        }

        if (!matchFound)
        {
            // Checking for pattern of moving up or down and having
            // two matching types on the left
            try
            {
                Piece leftPiece = GetGridPiece((int)end.x - 1, (int)end.y);
                Piece leftLeftPiece = GetGridPiece((int)end.x - 2, (int)end.y);
                Piece checkPiece1 = GetGridPiece((int)start.x, (int)start.y);
                if (leftPiece.GetPieceType() == leftLeftPiece.GetPieceType())
                {
                    if (leftPiece.GetPieceType() == checkPiece1.GetPieceType())
                    {
                        matchFound = true;
                        validMoveInProcess = true;
                        Piece toDestroy2 = grid[(int)end.x, (int)end.y];

                        leftPiece.SetForDestruction();
                        leftLeftPiece.SetForDestruction();
                        toDestroy2.SetForDestruction();
                        Debug.Log("======= MATCHED =======");
                    }
                }
            }
            catch (NullReferenceException)
            {
                // Object reference not set to an instance of an object
            }
        }

        if (!matchFound)
        {
            // Checking for pattern of moving up or down and having
            // two matching types on the right
            try
            {
                Piece rightPiece = GetGridPiece((int)end.x + 1, (int)end.y);
                Piece rightRightPiece = GetGridPiece((int)end.x + 2, (int)end.y);
                Piece checkPiece2 = GetGridPiece((int)start.x, (int)start.y);
                if (rightPiece.GetPieceType() == rightRightPiece.GetPieceType())
                {
                    if (rightPiece.GetPieceType() == checkPiece2.GetPieceType())
                    {
                        matchFound = true;
                        validMoveInProcess = true;
                        Piece toDestroy2 = GetGridPiece((int)end.x, (int)end.y);

                        rightPiece.SetForDestruction();
                        rightRightPiece.SetForDestruction();
                        toDestroy2.SetForDestruction();
                        Debug.Log("======= MATCHED =======");
                    }
                }
            }
            catch (NullReferenceException)
            {
                // Object reference not set to an instance of an object
            }
        }

        if (!matchFound)
        {
            // Checking for pattern of moving up or down and having
            // two matching types on either side
            try
            {
                Piece rightPiece = GetGridPiece((int)end.x + 1, (int)end.y);
                Piece leftPiece = GetGridPiece((int)end.x - 1, (int)end.y);
                Piece checkPiece3 = GetGridPiece((int)start.x, (int)start.y);
                if (rightPiece.GetPieceType() == leftPiece.GetPieceType())
                {
                    if (rightPiece.GetPieceType() == checkPiece3.GetPieceType())
                    {
                        matchFound = true;
                        validMoveInProcess = true;
                        Piece toDestroy2 = GetGridPiece((int)end.x, (int)end.y);

                        rightPiece.SetForDestruction();
                        leftPiece.SetForDestruction();
                        toDestroy2.SetForDestruction();
                        Debug.Log("======= MATCHED =======");
                    }
                }
            }
            catch (NullReferenceException)
            {
                // Object reference not set to an instance of an object
            }
        }

        if (!matchFound)
        {
            // Checking for pattern of moving up, down, left, or right and having
            // two matching types above
            try
            {
                Piece abovePiece = GetGridPiece((int)end.x, (int)end.y + 1);
                Piece aboveAbovePiece = GetGridPiece((int)end.x, (int)end.y + 2);
                Piece checkPiece4 = GetGridPiece((int)start.x, (int)start.y);
                if (abovePiece.GetPieceType() == aboveAbovePiece.GetPieceType())
                {
                    if (abovePiece.GetPieceType() == checkPiece4.GetPieceType())
                    {
                        matchFound = true;
                        validMoveInProcess = true;
                        Piece toDestroy2 = GetGridPiece((int)end.x, (int)end.y);

                        abovePiece.SetForDestruction();
                        aboveAbovePiece.SetForDestruction();
                        toDestroy2.SetForDestruction();

                        Debug.Log("======= MATCHED =======");
                    }
                }
            }
            catch (NullReferenceException)
            {
                // Object reference not set to an instance of an object
            }
        }

        Debug.Log("not valid move");
    }

    public bool IsDestroyed(Vector2 gridPosition)
    {
        Piece piece = GetGridPiece((int)gridPosition.x, (int)gridPosition.y, true);
        if (piece != null)
        {
            return piece.GetDestruction();
        }
        return false;
    }
}
