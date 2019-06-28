using UnityEngine;
using System.Collections.Generic;

public class Computer : MonoBehaviour
{

    private List<Vector2Int> MovesBlack = new List<Vector2Int>();
    private List<Vector2Int> MovesWhite = new List<Vector2Int>();
    public PieceManager mPieceManager;
    public Cell[,] mAllCells = new Cell[8,8];

    public void Setup(PieceManager newPieceManager) 
    {
        mPieceManager = newPieceManager;
    }

    public void StartComputerTurn() 
    {
        Debug.Log("Computer Turn Started.");
        GetMoves();

    }

    public void GetMoves()
    {
        foreach(BasePiece piece in mPieceManager.mBlackPieces)
        {
            piece.CheckPathing(true);

            foreach(Cell cell in piece.mHighlightedCells)  //move is located in piece.mHighlightedCells[i].mBoardPosition
            {
                MovesBlack.Add(cell.mBoardPosition);
            }
            // piece.ShowCells(); //testing to see if there are bugs


            // piece.ClearCells();  
        }
    }




    
}

