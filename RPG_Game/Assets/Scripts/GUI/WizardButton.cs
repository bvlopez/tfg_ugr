﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class WizardButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void selectJob() {
        GameObject createCharacterScreen = GameObject.Find("CreateCharacterScreen");
        CreateCharacterScreenManager screenManager = createCharacterScreen.GetComponent<CreateCharacterScreenManager>();

        // Si la clase no esta seleccionada ya
        if(screenManager.getActiveJob() != 2) {
            screenManager.setActiveJob(2);
            GetComponent<Image>().color = Color.grey;
        }
    }

    public void deselectJob() {
        GetComponent<Image>().color = Color.clear;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
