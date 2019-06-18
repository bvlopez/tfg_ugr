﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ArtsFactionButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void selectFaction() {
        GameObject createCharacterScreen = GameObject.Find("CreateCharacterScreen");
        CreateCharacterScreenManager screenManager = createCharacterScreen.GetComponent<CreateCharacterScreenManager>();

        // Si la clase no esta seleccionada ya
        if(screenManager.getActiveFaction() != 1) {
            screenManager.setActiveFaction(1);
            GetComponent<Image>().color = Color.grey;
        }
    }

    public void deselectFaction() {
        GetComponent<Image>().color = Color.clear;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
