using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChatManager : MonoBehaviour
{
    [SerializeField]
    private GameObject chatLinePrefab;
    [SerializeField]
    private GameObject chatLineInput;
    [SerializeField]
    private GameObject scrollView;
    private List<GameObject> chatLinesPrefabs;
    private GameManager gameManager;
    private bool autoLoad;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.instance;
        chatLinesPrefabs = new List<GameObject>();
        loadLines();
    }

    void Awake() {
        autoLoad = true;
    }

    public void sendLine() {
        JSONObject json = new JSONObject(JSONObject.Type.OBJECT);
        json.AddField("email", gameManager.getUser().getEmail());
        json.AddField("user_password", gameManager.getUser().getPassword());
        json.AddField("second_player_id", gameManager.getOnlinePlayerId());
        json.AddField("line_text", chatLineInput.GetComponent<InputField>().text);
        StartCoroutine(gameManager.getServerConnection().postRequest(json, "set_chat_line", setChatLineResponse));
    }

    public void setChatLineResponse(JSONObject json) {   
        autoLoad = false;                 
        loadLines();
        chatLineInput.GetComponent<InputField>().text = "";
    }


    public void loadLines() {
        JSONObject json = new JSONObject(JSONObject.Type.OBJECT);
        json.AddField("email", gameManager.getUser().getEmail());
        json.AddField("user_password", gameManager.getUser().getPassword());
        json.AddField("second_player_id", gameManager.getOnlinePlayerId());
        StartCoroutine(gameManager.getServerConnection().postRequest(json, "get_chat", getChatResponse));
    }

    public void getChatResponse(JSONObject json) {    
                
        for(int i = 0; i < chatLinesPrefabs.Count; i++) {
            chatLinesPrefabs[i].SetActive(false);
            Destroy(chatLinesPrefabs[i]);
        }
        chatLinesPrefabs = new List<GameObject>();

        List<int> owners_id = new List<int>();
		JSONObject owners_array = json.GetField("lines_owners");	
		foreach(JSONObject j in owners_array.list) {
			owners_id.Add(int.Parse(j.GetField("owner_id").str));				
		}

        List<string> lines_text = new List<string>();
		JSONObject lines_array = json.GetField("lines_text");	
		foreach(JSONObject j in lines_array.list) {
			lines_text.Add(j.GetField("text").str);				
        }

        List<string> dates = new List<string>();
		JSONObject dates_array = json.GetField("dates");	
		foreach(JSONObject j in dates_array.list) {
			dates.Add(j.GetField("date").str);				
		}	

        for(int i = 0; i < lines_text.Count; i++) {     
            chatLinesPrefabs.Add((GameObject)Instantiate(chatLinePrefab, new Vector3(0, 358 - i*84, 0), Quaternion.identity));         
            chatLinesPrefabs[i].transform.SetParent(scrollView.transform, false);
            if(owners_id[i] != gameManager.getOnlinePlayerId()) {
                chatLinesPrefabs[i].transform.GetChild(0).GetComponent<Text>().text = "[" + dates[i] + "] " + "Yo:";
            }
            else {
                chatLinesPrefabs[i].transform.GetChild(0).GetComponent<Text>().text = "[" + dates[i] + "] " + gameManager.getOnlinePlayerName() + ":";
            }    
            chatLinesPrefabs[i].transform.GetChild(1).GetComponent<Text>().text = lines_text[i];
        }

        if(autoLoad) {
            JSONObject j = new JSONObject(JSONObject.Type.OBJECT);
            j.AddField("email", gameManager.getUser().getEmail());
            j.AddField("user_password", gameManager.getUser().getPassword());
            j.AddField("second_player_id", gameManager.getOnlinePlayerId());
            StartCoroutine(gameManager.getServerConnection().postRequest(j, "get_chat", getChatResponse));
        }
        autoLoad = true;
    }

    public void closeChat() {
        SceneManager.LoadScene("WorldScene");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
