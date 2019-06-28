using UnityEngine;
using System.Collections;

public class Computer : MonoBehaviour
{


    public PieceManager mPieceManager;

    public void Setup(PieceManager newPieceManager) 
    {
        mPieceManager = newPieceManager;
    }
    
    public void StartComputerTurn() 
    {
        foreach(BasePiece piece in mPieceManager.mBlackPieces)
        {
            piece.CheckPathing();
        }
        Debug.Log("Computer Turn Started.");
    }
}