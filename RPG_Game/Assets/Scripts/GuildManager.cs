using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GuildManager : MonoBehaviour
{

    private GameObject activeButton;
    [SerializeField]
    private GameObject descriptionView;
    [SerializeField]
    private GameObject acceptButton;
    [SerializeField]
    private GameObject completeButton;
    [SerializeField]
    private GameObject scienceFaction;
    [SerializeField]
    private GameObject engineeringFaction;
    [SerializeField]
    private GameObject humanitiesFaction;
    [SerializeField]
    private GameObject guildName;
    private GameManager gameManager;
    private List<GameObject> questsPrefabs;
    [SerializeField]
    private GameObject questButtonPrefab;


    public void showDescription(GameObject button, Quest quest) { 
        if(activeButton != null) {            
            activeButton.GetComponent<GuildQuestButton>().deselectQuest();
        }
        activeButton = button;
        Text descriptionText = descriptionView.GetComponent<Text>();
        if(quest.getStatus() == "No aceptada") {
            acceptButton.SetActive(true);
            completeButton.SetActive(false);
        }
        else if(quest.getStatus() == "Lista para entregar") {
            acceptButton.SetActive(false);
            completeButton.SetActive(true);
        }
        else {
            acceptButton.SetActive(false);
            completeButton.SetActive(false);
        }
        descriptionText.text = quest.getDescription();    
    }

    public void resetView() {
        Text descriptionText = descriptionView.GetComponent<Text>();
        descriptionText.text = "";
        //questsPrefabs.Clear();
    }   

    public void acceptQuest() {
        JSONObject json = new JSONObject(JSONObject.Type.OBJECT);
        json.AddField("email", gameManager.getUser().getEmail());
        json.AddField("user_password", gameManager.getUser().getPassword());
        json.AddField("id", activeButton.GetComponent<GuildQuestButton>().getQuest().getId());
        StartCoroutine(gameManager.getServerConnection().postRequest(json, "accept_quest", acceptQuestResponse));
    }

    public void acceptQuestResponse(JSONObject json) {
        getQuests();
    }

    public void completeQuestResponse(JSONObject json) {
        getQuests();
    }

    public void completeQuest() {
        JSONObject json = new JSONObject(JSONObject.Type.OBJECT);
        json.AddField("email", gameManager.getUser().getEmail());
        json.AddField("user_password", gameManager.getUser().getPassword());
        json.AddField("id", activeButton.GetComponent<GuildQuestButton>().getQuest().getId()); 
        StartCoroutine(gameManager.getServerConnection().postRequest(json, "complete_quest", completeQuestResponse));
    }

    // Obtiene las misiones
    public void getQuests() {
        JSONObject json = new JSONObject(JSONObject.Type.OBJECT);
        json.AddField("email", gameManager.getUser().getEmail());
        json.AddField("user_password", gameManager.getUser().getPassword());
        StartCoroutine(gameManager.getServerConnection().postRequest(json, "quests", getQuestsResponse));
    }

    public void getQuestsResponse(JSONObject json) {
        for(int j = 0; j < questsPrefabs.Count; j++) {
            questsPrefabs[j].SetActive(false);
            Destroy(questsPrefabs[j]);
        }
        questsPrefabs.Clear();
        GameObject scrollView = GameObject.Find("Content");

        JSONObject array_quests = json.GetField("quests");
        int i = 0;
        foreach(JSONObject j in array_quests.list) {
            int id = int.Parse(j.GetField("id").str);
            string title = j.GetField("title").str;              
            string description = j.GetField("description").str;
            int number = int.Parse(j.GetField("number").str);
            int experience = int.Parse(j.GetField("experience").str);
            int money = int.Parse(j.GetField("money").str);
            int progress = int.Parse(j.GetField("progress").str);
            string status = ""; 
            if(progress < 0) {
                status = "No aceptada";
                progress = 0;
            }
            else if(progress >= 0 && progress < number) {
                status = "Aceptada";
            }
            else if(progress >= number) {
                status = "Lista para entregar";
            }     
            questsPrefabs.Add((GameObject)Instantiate(questButtonPrefab, new Vector3(0, 358 - i*84, 0), Quaternion.identity));
            questsPrefabs[i].transform.SetParent(scrollView.transform, false);
            questsPrefabs[i].GetComponent<GuildQuestButton>().setQuest(new Quest(id, title, description, experience, money, progress, number, "", status));
            i++;   
        }  
        acceptButton.SetActive(false);
        completeButton.SetActive(false);
    }

    public void closeGuild() {
        SceneManager.LoadScene("WorldScene");
    }

    public void startReset(string value) {
        resetView();
    }

    public void getFactionsPoints() {
        StartCoroutine(gameManager.getServerConnection().getRequest("factions_points", getFactionsPointsResponse));
    }

    public void getFactionsPointsResponse(JSONObject json) {
            JSONObject array = json.GetField("factions_names");
            JSONObject points_array = json.GetField("factions_points");
            int i = 0;
			foreach(JSONObject j in array.list) {
				if(j.GetField("name").str == "Science") {
                    scienceFaction.GetComponent<Text>().text = points_array[i].GetField("points").str;
                }
                else if(j.GetField("name").str == "Engineering") {
                    engineeringFaction.GetComponent<Text>().text = points_array[i].GetField("points").str;
                }
                else {
                    humanitiesFaction.GetComponent<Text>().text = points_array[i].GetField("points").str;
                }
                i++;
			}
    }

    public void restorePlayer() {
        gameManager.getPlayer().setStat(Stat.CurrentHealth, gameManager.getPlayer().getStat(Stat.MaxHealth));
        gameManager.getPlayer().setStat(Stat.CurrentManaPoints, gameManager.getPlayer().getStat(Stat.ManaPoints));
        gameManager.sendPlayerAttributes();
    }

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.instance;
        GameObject scrollView = GameObject.Find("Content");
        acceptButton.SetActive(false);
        completeButton.SetActive(false);
        questsPrefabs = new List<GameObject>();
        getFactionsPoints();
        getQuests();

        guildName.GetComponent<Text>().text = gameManager.getCurrentGuild();        
        gameManager = GameManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
