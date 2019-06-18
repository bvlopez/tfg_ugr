using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BossSpawner : MonoBehaviour
{

    private GameManager gameManager;
    private int id;
    private int braveryPoints;
    private bool canFight;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.instance;        
    }

    public void setBoss(int value) {
        id = value;   
    }

    public void setBraveryPoints(int value) {
        braveryPoints = value;   
    }

    public int getBraveryPoints() {
        return braveryPoints;
    }

    public bool checkPoints(int value) {
        if(value >= braveryPoints) {
            canFight = true;
        }
        return canFight;
    }

    public void loadBattle() {
        if(canFight) {           
            gameManager.loadBossBattle(id, braveryPoints);    
        }    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
