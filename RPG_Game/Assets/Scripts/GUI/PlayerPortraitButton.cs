using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerPortraitButton : MonoBehaviour
{
    [SerializeField]
    private List<Sprite> portraits;
    [SerializeField]
    private Image portrait;

    public void loadMenu() {
        SceneManager.LoadScene("MenuScene");
    }

    void Awake() {
        GameManager gameManager = GameManager.instance;
        Player player = gameManager.getPlayer();
        if(player.getJob() == Job.Wizard) {
            portrait.sprite = portraits[0];
        }
        else if(player.getJob() == Job.Warrior) {
            portrait.sprite = portraits[1];
        }
        else {
            portrait.sprite = portraits[2];
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
