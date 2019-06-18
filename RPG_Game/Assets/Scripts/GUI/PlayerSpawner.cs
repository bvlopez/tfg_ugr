using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerSpawner : MonoBehaviour
{

    private GameManager gameManager;
    private int id;
    private Faction faction;
    private string name;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.instance;
    }

    public void setPlayer(int value, string factionName, string playerName) {
        id = value;
        if(factionName == "science") {
            faction = Faction.Science;
        }
        else if(factionName == "engineering") {
            faction = Faction.Engineering;
        }
        else {
            faction = Faction.ArtsAndHumanities;
        }
        name = playerName;
    }

    public string getName() {
        return name;
    }

    public Faction getFaction() {
        return faction;
    }

    public int getId() {
        return id;
    }

    public void playersInteraction() {
        gameManager.playersInteraction(id, faction, name);
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}