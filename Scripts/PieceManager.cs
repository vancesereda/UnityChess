using System;
using System.Collections.Generic;
using UnityEngine;

public enum CheckState 
{
    Check,
    Checkmate,
    None
}
public class PieceManager : MonoBehaviour
{
    [HideInInspector]
    public bool mIsKingAlive = true;

    public GameObject mPiecePrefab;
    public ResetButton ResetButton;

    public List<BasePiece> mWhitePieces = null;
    public List<BasePiece> mBlackPieces = null;
    private List<BasePiece> mPromotedPieces = new List<BasePiece>();

    private string[] mPieceOrder = new string[16]
    {
        "P", "P", "P", "P", "P", "P", "P", "P",
        "R", "KN", "B", "K", "Q", "B", "KN", "R"
    };

    private Dictionary<string, Type> mPieceLibrary = new Dictionary<string, Type>()
    {
        {"P",  typeof(Pawn)},
        {"R",  typeof(Rook)},
        {"KN", typeof(Knight)},
        {"B",  typeof(Bishop)},
        {"K",  typeof(King)},
        {"Q",  typeof(Queen)}
    };
    public void Setup(Board board)
    {
        // Create white pieces
        mWhitePieces = CreatePieces(Color.white, new Color32(80, 124, 159, 255), board);

        // Create place pieces
        mBlackPieces = CreatePieces(Color.black, new Color32(210, 95, 64, 255), board);

        // Place pieces
        PlacePieces(1, 0, mWhitePieces, board);
        PlacePieces(6, 7, mBlackPieces, board);


        // Added
        // White goes first
        SwitchSides(Color.black);
    }

    private List<BasePiece> CreatePieces(Color teamColor, Color32 spriteColor, Board board)
    {
        List<BasePiece> newPieces = new List<BasePiece>();
        ResetButton.Setup(this);

        for (int i = 0; i < mPieceOrder.Length; i++)
        {
            // Get the type, apply to new object
            string key = mPieceOrder[i];
            Type pieceType = mPieceLibrary[key];

            // Store new piece
            BasePiece newPiece = CreatePiece(pieceType);
            newPieces.Add(newPiece);

            // Setup piece
            newPiece.Setup(teamColor, spriteColor, this);
        }

        return newPieces;
    }

    private BasePiece CreatePiece(Type pieceType)
    {
        // Create new object
        GameObject newPieceObject = Instantiate(mPiecePrefab);
        newPieceObject.transform.SetParent(transform);

        // Set scale and position
        newPieceObject.transform.localScale = new Vector3(1, 1, 1);
        newPieceObject.transform.localRotation = Quaternion.identity;

        BasePiece newPiece = (BasePiece) newPieceObject.AddComponent(pieceType);

        return newPiece;
    }

    private void PlacePieces(int pawnRow, int royaltyRow, List<BasePiece> pieces, Board board)
    {
        for (int i = 0; i < 8; i++)
        {
            // Place pawns    
            pieces[i].Place(board.mAllCells[i, pawnRow]);
            //Add location of pawns
            pieces[i].location.Set(i, pawnRow-1);
            // Debug.Log(pieces[i].location.ToString());

            // Place royalty
            pieces[i + 8].Place(board.mAllCells[i, royaltyRow]);
            //Add location of royalty
            pieces[i].location.Set(i, royaltyRow-1);
            // Debug.Log(pieces[i].location.ToString());
        }
    }

    private void SetInteractive(List<BasePiece> allPieces, bool value)
    {
        foreach (BasePiece piece in allPieces)
            piece.enabled = value;
    }

    public bool SwitchSides(Color color)
    {
        if (!mIsKingAlive)
        {
            // Reset pieces
            ResetPieces();

            // King has risen from the dead
            mIsKingAlive = true;

            // Change color to black, so white can go first again
            color = Color.black;
        }

        bool isBlackTurn = color == Color.white ? true : false;
        // Set interactivity
        SetInteractive(mWhitePieces, !isBlackTurn);
        SetInteractive(mBlackPieces, isBlackTurn);
        

        // Set promoted interactivity
        foreach(BasePiece piece in mPromotedPieces)
        {
            bool isBlackPiece = piece.mColor != Color.white ? true : false;
            bool isPartOfTeam = isBlackPiece == true ? isBlackTurn : !isBlackTurn;

            piece.enabled = isPartOfTeam;
        }
        return isBlackTurn;
    }

    public void ResetPieces()
    {
        foreach(BasePiece piece in mPromotedPieces)
        {
            piece.Kill();

            Destroy(piece.gameObject);
        }

        // Reset white
        foreach (BasePiece piece in mWhitePieces)
            piece.Reset();

        // Reset black
        foreach (BasePiece piece in mBlackPieces)
            piece.Reset();
    }

    public void PromotePiece(Pawn pawn, Cell cell, Color teamColor, Color spriteColor)
    {
        // Kill pawn
        pawn.Kill();

        // Create
        BasePiece promotedPiece = CreatePiece(typeof(Queen));
        promotedPiece.Setup(teamColor, spriteColor, this);

        // Place
        promotedPiece.Place(cell);

        // Add
        mPromotedPieces.Add(promotedPiece);
    }


    // public bool GetCheck(/*paths from mHighlightedCells, CellState, ... */)  
    // {
    //     mTestPieces = !isBlackTurn ? mBlackPieces : mWhitePieces;

    //     foreach(BasePiece piece in mTestPieces)
    //     {
    //         piece.CheckPathing();

    //     }
    // }
    // public CheckState TestCheck(List<Cell> mKingCells) 
    // {
    //     //test check state
    //     // testPieces = mColor == Color.white ? mPieceManager.mWhitePieces : mPieceManager.mBlackPieces;
    //     // foreach(BasePiece piece in testPieces)
    //     // {
    //     //     CheckPathing();

    //     // }
    //     if (!mKingCells)
    //         return CheckState.Check;
        



    //     //if check, test for checkmate (check all other possible moves of other opponent)

    // }
}
