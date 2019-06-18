using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuildSpawner : MonoBehaviour
{

    private GameManager gameManager;
    private string name;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.instance;
    }

    public void setGuild(string value) {
        name = value;
    }

    public string getName() {
        return name;
    }

    public void loadGuild() {
        gameManager.loadGuild(name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
