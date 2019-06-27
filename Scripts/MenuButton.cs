using UnityEngine;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{
    //Make sure to attach these Buttons in the Inspector
    public GameObject Panel;

    public void TaskOnClick()
    {
        Debug.Log("You have clicked the button!");
        if (Panel != null)
        {
            Panel.SetActive(!Panel.activeSelf);
        }
        
        
    }

}