using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPanelManager : MonoBehaviour
{
    private GameObject loginScreen;
    private GameObject createAccountScreen;
    private GameObject createCharacterScreen;

    // Start is called before the first frame update
    void Start()
    {
        loginScreen = GameObject.Find("LoginScreen");
        createAccountScreen = GameObject.Find("CreateAccountScreen");
        createCharacterScreen = GameObject.Find("CreateCharacterScreen");

        loginScreen.SetActive(true);
        createAccountScreen.SetActive(false);
        createCharacterScreen.SetActive(false);
        
        //GameObject prueba1 = GameObject.Find("LoginTitle");
        //GameManager gameManager = GameManager.instance;
        //prueba1.GetComponent<Text>().text = gameManager.getEnemyCatalog().getEnemyByName("Araña").getName();
    }
    
    public GameObject getLoginScreen() {
        return loginScreen;
    }

    public GameObject getCreateAccountScreen() {
        return createAccountScreen;
    }

    public GameObject getCreateCharacterScreen() {
        return createCharacterScreen;
    }
    
    public void setScreen(string screen) {
        if(string.Compare(screen, "login") == 0) {
            loginScreen.SetActive(true);
            createAccountScreen.SetActive(false);
            createCharacterScreen.SetActive(false);
        }
        else if(string.Compare(screen, "accountCreation") == 0) {
            loginScreen.SetActive(false);
            createAccountScreen.SetActive(true);
            createCharacterScreen.SetActive(false);
        }
        else if(string.Compare(screen, "characterCreation") == 0) {
            loginScreen.SetActive(false);
            createAccountScreen.SetActive(false);
            createCharacterScreen.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
