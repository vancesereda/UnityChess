using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public Board mBoard;

    public PieceManager mPieceManager;


    void Start()
    {
        // Create the board
        mBoard.Create();

        // Create pieces
        mPieceManager.Setup(mBoard);
      
    }
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
                if (Application.platform == RuntimePlatform.Android)
                {
                    AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
                    activity.Call<bool>("moveTaskToBack", true);
                }
                else
                {
                    Application.Quit();
                }
        }
    }
    
 
}
