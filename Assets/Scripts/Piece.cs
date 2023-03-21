using UnityEngine;

public enum PieceTypes
{
    Deborah = 0,
    Elisha = 1,
    Lamb = 2,
    Andrew = 3
}

public class Piece
{
    // This script will manage the state of a single
    // piece in the grid.

    private Vector3 position;
    private Vector2 gridPosition;
    private PieceTypes pieceType;

    public Piece()
    {
        position = Vector3.zero;
        gridPosition = Vector2.zero;
        pieceType = PieceTypes.Andrew;
    }

    public Piece(Vector3 position, Vector2 gridPosition)
    {
        this.position = position;
        this.gridPosition = gridPosition;
        this.pieceType = PieceTypes.Andrew;
    }

    public Piece(Vector3 position, Vector2 gridPosition, PieceTypes pieceType)
    {
        this.position = position;
        this.gridPosition = gridPosition;
        this.pieceType = pieceType;
    }

    public Vector3 GetPosition()
    {
        return position;
    }

    public PieceTypes GetPieceType()
    {
        return pieceType;
    }
}
