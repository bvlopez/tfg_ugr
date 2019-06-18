using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ShopSpawner : MonoBehaviour
{

    private GameManager gameManager;
    private int id;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.instance;
    }

    public void setShop(int value) {
        id = value;   
    }

    public void loadShop() {
        gameManager.loadShop(id);        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
