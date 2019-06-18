using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateAccountButton : MonoBehaviour
{

    public void changeScene() {
        GameObject panel = GameObject.Find("Panel");
        MainPanelManager panelManager = panel.GetComponent<MainPanelManager>();     
        panelManager.setScreen("accountCreation");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
