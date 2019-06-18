using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConnectButton : MonoBehaviour
{

    public void login() {
        GameManager gameManager = GameManager.instance;
        JSONObject json = new JSONObject(JSONObject.Type.OBJECT);
        GameObject email = GameObject.Find("Email");
        GameObject password = GameObject.Find("Password");
        //GameObject ip = GameObject.Find("ip");
        //if((password.GetComponent<InputField>().text != null) && (password.GetComponent<InputField>().text != "")) {
        //    gameManager.getServerConnection().setUrl(ip.GetComponent<InputField>().text);
       // }       
        json.AddField("user_password", password.GetComponent<InputField>().text);
        json.AddField("email", email.GetComponent<InputField>().text);
        StartCoroutine(gameManager.getServerConnection().postRequest(json, "login", getResponse));
    }

    public void getResponse(JSONObject json) {
        if(json.GetField("correct_data").n == 1) {            
            GameManager gameManager = GameManager.instance;
            gameManager.setUser(json.GetField("email").str, json.GetField("user_password").str);
            gameManager.setPlayer();
            StartCoroutine(gameManager.waitPlayerUpdate("WorldScene", gameManager.changeScene));
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
