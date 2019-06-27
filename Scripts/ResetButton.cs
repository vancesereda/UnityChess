using UnityEngine;

public class ResetButton : MonoBehaviour 
{
    public PieceManager mPieceManager;
    public void Setup(PieceManager newPieceManager)
    {
        mPieceManager = newPieceManager;
    }
    public void Reset() 
    {
        Debug.Log("Clicked");
        mPieceManager.ResetPieces();
    }

}