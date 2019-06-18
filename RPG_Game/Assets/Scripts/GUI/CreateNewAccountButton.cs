using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateNewAccountButton : MonoBehaviour
{

    public void changeScene() {        
        GameManager gameManager = GameManager.instance;
        JSONObject json = new JSONObject(JSONObject.Type.OBJECT);
        GameObject email = GameObject.Find("NewEmail");
        GameObject password = GameObject.Find("NewPassword");
        json.AddField("user_password", password.GetComponent<InputField>().text);
        json.AddField("email", email.GetComponent<InputField>().text);
        StartCoroutine(gameManager.getServerConnection().postRequest(json, "create", getResponse));          
    }


    public void getResponse(JSONObject json) {
        if(json.GetField("email").str != "exits") {
            GameObject panel = GameObject.Find("Panel");  
            MainPanelManager panelManager = panel.GetComponent<MainPanelManager>();
            GameManager gameManager = GameManager.instance;
            gameManager.setUser(json.GetField("email").str, json.GetField("user_password").str);
            panelManager.setScreen("characterCreation");
        }
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
