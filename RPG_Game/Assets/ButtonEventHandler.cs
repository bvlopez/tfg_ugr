using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonEventHandler : MonoBehaviour
{
    public void closeGame() {
        GameObject closeButton = GameObject.Find("Button");
        closeButton.GetComponent<Image>().color = Color.red;
        Application.Quit();
    }
}
