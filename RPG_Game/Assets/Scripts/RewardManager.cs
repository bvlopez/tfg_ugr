using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RewardManager : MonoBehaviour
{

    [SerializeField]
    private GameObject money;
    [SerializeField]
    private GameObject experience;
    [SerializeField]
    private List<GameObject> enemies;
    private GameManager gameManager;
    private List<string> enemies_names;

    void Awake()
    {
        gameManager = GameManager.instance;
        enemies_names = gameManager.getDefeatedEnemies();
        for(int i = 0; i < enemies_names.Count; i++) {
            enemies[i].GetComponent<Text>().text = enemies_names[i];
        }
        JSONObject json = new JSONObject(JSONObject.Type.OBJECT);
        json.AddField("email", gameManager.getUser().getEmail());
        json.AddField("user_password", gameManager.getUser().getPassword());
        json.AddField("enemies", enemies_names.Count);    
        StartCoroutine(gameManager.getServerConnection().postRequest(json, "reward", getResponse));
        StartCoroutine(gameManager.getServerConnection().postRequest(json, "accepted_quests", getAcceptedQuestsResponse));

        // Se ganan puntos de faccion si era un jefe
        if(gameManager.getEnemyCatalog().getEnemyByName(enemies_names[0]).isEnemyBoss()) {
            JSONObject secondJson = new JSONObject(JSONObject.Type.OBJECT);
            secondJson.AddField("email", gameManager.getUser().getEmail());
            secondJson.AddField("user_password", gameManager.getUser().getPassword());
            StartCoroutine(gameManager.getServerConnection().postRequest(secondJson, "add_faction_points", null));
        }
    }

    public void getResponse(JSONObject json) {		
		experience.GetComponent<Text>().text = ((int) json.GetField("experience").n).ToString();
		money.GetComponent<Text>().text = ((int) json.GetField("money").n).ToString();           
	}

    public void getAcceptedQuestsResponse(JSONObject json) {			
        JSONObject array_quests = json.GetField("quests");
        foreach(JSONObject j in array_quests.list) {
            int id = int.Parse(j.GetField("id").str);
            string title = j.GetField("title").str;              
            string description = j.GetField("description").str;
            string enemy_name = j.GetField("enemy_name").str;
            int number = int.Parse(j.GetField("number").str);
            int experience = int.Parse(j.GetField("experience").str);
            int money = int.Parse(j.GetField("money").str);
            int progress = int.Parse(j.GetField("progress").str);         
            
            int numberDefeated = 0;
            for(int i = 0; i < enemies_names.Count; i++) {
                if(enemies_names[i] == enemy_name) {
                    numberDefeated++;
                }
            }
            if((numberDefeated > 0) && (progress < number)) {
                if((numberDefeated + progress) > number) {
                    numberDefeated = number - progress;
                }
                JSONObject j2 = new JSONObject(JSONObject.Type.OBJECT);
                j2.AddField("email", gameManager.getUser().getEmail());
                j2.AddField("user_password", gameManager.getUser().getPassword());
                j2.AddField("id", id);
                j2.AddField("progress", numberDefeated);  
                StartCoroutine(gameManager.getServerConnection().postRequest(j2, "progress_quest", null));
            }
        }           
	}

    public void changeScene() {
        SceneManager.LoadScene("WorldScene");
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
