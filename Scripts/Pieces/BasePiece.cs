using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;


public abstract class BasePiece : EventTrigger
{
    [HideInInspector]
    public Color mColor = Color.clear;

    protected Cell mOriginalCell = null;
    protected Cell mCurrentCell = null;

    protected RectTransform mRectTransform = null;
    protected PieceManager mPieceManager;

    protected Cell mTargetCell = null;
    public Vector2Int location = Vector2Int.zero;

    protected Vector3Int mMovement = Vector3Int.one;
    protected List<Cell> mHighlightedCells = new List<Cell>();
    protected List<Cell> mKingCells = new List<Cell>();

    public virtual void Setup(Color newTeamColor, Color32 newSpriteColor, PieceManager newPieceManager)
    {
        mPieceManager = newPieceManager;

        mColor = newTeamColor;
        GetComponent<Image>().color = newSpriteColor;
        mRectTransform = GetComponent<RectTransform>();
    }

    public void Place(Cell newCell)
    {
        // Cell stuff
        mCurrentCell = newCell;
        mOriginalCell = newCell;
        mCurrentCell.mCurrentPiece = this;

        // Object stuff
        transform.position = newCell.transform.position;
        gameObject.SetActive(true);
    }

    public virtual void Reset()
    {
        Kill();

        Place(mOriginalCell);
    }

    public virtual void Kill()
    {
        // Clear current cell
        mCurrentCell.mCurrentPiece = null;

        // Remove piece
        gameObject.SetActive(false);
    }

    #region Movement
    protected void CreateCellPath(int xDirection, int yDirection, int movement)
    {
        // Target position
        int currentX = mCurrentCell.mBoardPosition.x;
        int currentY = mCurrentCell.mBoardPosition.y;

        // Check each cell
        for (int i = 1; i <= movement; i++)
        {
            currentX += xDirection;
            currentY += yDirection;

            // Get the state of the target cell
            CellState cellState = CellState.None;
            cellState = mCurrentCell.mBoard.ValidateCell(currentX, currentY, this);

            //If King, add 
            if (cellState == CellState.King)
            {
                mKingCells.Add(mCurrentCell.mBoard.mAllCells[currentX,currentY]);
                mHighlightedCells.Add(mCurrentCell.mBoard.mAllCells[currentX, currentY]);
                break;
            }
            // If enemy, add to list, break
            if (cellState == CellState.Enemy)
            {
                mHighlightedCells.Add(mCurrentCell.mBoard.mAllCells[currentX, currentY]);
                break;
            }

            // If the cell is not free, break
            if (cellState != CellState.Free)
                break;

            // Add to list
            mHighlightedCells.Add(mCurrentCell.mBoard.mAllCells[currentX, currentY]);
        }
    }

    protected virtual void CheckPathing()
    {
        // Horizontal
        CreateCellPath(1, 0, mMovement.x);
        CreateCellPath(-1, 0, mMovement.x);

        // Vertical 
        CreateCellPath(0, 1, mMovement.y);
        CreateCellPath(0, -1, mMovement.y);

        // Upper diagonal
        CreateCellPath(1, 1, mMovement.z);
        CreateCellPath(-1, 1, mMovement.z);

        // Lower diagonal
        CreateCellPath(-1, -1, mMovement.z);
        CreateCellPath(1, -1, mMovement.z);
    }

    protected void ShowCells()
    {
        foreach (Cell cell in mHighlightedCells)
            cell.mOutlineImage.enabled = true;
    }

    protected void ClearCells()
    {
        foreach (Cell cell in mHighlightedCells)
            cell.mOutlineImage.enabled = false;

        mHighlightedCells.Clear();
    }

    protected virtual void Move()
    {
        // If there is an enemy piece, remove it
        mTargetCell.RemovePiece();

        // Clear current
        mCurrentCell.mCurrentPiece = null;

        // Switch cells
        mCurrentCell = mTargetCell;
        mCurrentCell.mCurrentPiece = this;
        location.Set(mCurrentCell.mBoardPosition.x, mCurrentCell.mBoardPosition.y);
        Debug.Log(location.ToString());


        // Move on board
        transform.position = mCurrentCell.transform.position;
        
        Debug.Log(mCurrentCell.mBoardPosition);
        mTargetCell = null;
    }
    #endregion

    #region Events
    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);

        // Test for cells
        CheckPathing();

        // Show valid cells
        ShowCells();
    }

    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);

        // Follow pointer
        transform.position += (Vector3)eventData.delta;

        // Check for overlapping available squares
        foreach (Cell cell in mHighlightedCells)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(cell.mRectTransform, Input.mousePosition))
            {
                // If the mouse is within a valid cell, get it, and break.
                mTargetCell = cell;
                break;
            }

            // If the mouse is not within any highlighted cell, we don't have a valid move.
            mTargetCell = null;
        }
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);

        // Hide
        ClearCells();

        // Return to original position
        if (!mTargetCell)
        {
            transform.position = mCurrentCell.gameObject.transform.position;
            return;
        }
       
        // Move to new cell
        Move();

        // GetCurrentTeamMoves();
        // mPieceManager.TestCheck(mKingCells);

        // End turn
        bool isBlackTurn = mPieceManager.SwitchSides(mColor);
        if (isBlackTurn) StartComputerTurn();
    }

    private void StartComputerTurn() 
    {
        // get all pieces (get the location of all pieces)
        // foreach (BasePiece piece in mPieceManager.mBlackPieces)
        // {
        //     Debug.Log('sdf');
        // };
        // BasePiece.GetType().GetProperties();
    }

    // private void GetCurrentTeamMoves() 
    // {
    //     testPieces = mColor == Color.white ? mPieceManager.mWhitePieces : mPieceManager.mBlackPieces;
    //     foreach(BasePiece piece in testPieces)
    //     {
    //         CheckPathing();
    //         mHighlightedCells.Clear();
    //     }
    // }

    // public List<Cell> GetOtherTeamMoves()
    // {
    //     testPieces = mColor == Color.white ? mPieceManager.mBlackPieces : mPieceManager.mWhitePieces;
    //     foreach(BasePiece piece in testPieces)
    //     {
    //         CheckPathing();
    //         mHighlightedCells.Clear();
    //     }
    // }





    
    #endregion
}
