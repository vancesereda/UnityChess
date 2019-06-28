using UnityEngine;
using System.Collections;

public class Computer : MonoBehaviour
{

    // private List<Move> Moves = new List<Move>();
    // private List<Scenario> Scenarios = new List<Scenario>(); 
    public PieceManager mPieceManager;

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
            piece.CheckPathing();
            // piece.ShowCells(); //testing to see if there are bugs

            
            // piece.ClearCells();
        }
        Debug.Log("Computer Turn Started.");
    }

    
}