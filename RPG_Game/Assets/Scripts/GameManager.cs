using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager instance = null;
    [SerializeField]
    private EnemyCatalog enemyCatalog;
    [SerializeField]
    private SkillCatalog skillCatalog;
    [SerializeField]
    private ItemCatalog itemCatalog;
    private List<Item> currentShopItems;
    private Player player;
    public List<Item> pruebaObjetos;
    public List<Skill> pruebaHabilidades;
    private ServerConnection serverConnection;
    private User user;
    private bool updated_player;
    private string currentGuild;
    [SerializeField]
    private List<string> defeatedEnemies;
    private bool isBossFight;
    private string bossName;
    [SerializeField]
    private float distanceTraveled;
    // Variables para combate online
    private bool onlineBattle;
    [SerializeField]
    private Enemy onlinePlayer;
    private int onlinePlayerId;
    private Job onlinePlayerJob;
    private int onlineBattleId;
    private string onlinePlayerName;

    void Awake() {
        if(instance == null) {
            instance = this;
        }
        else if(instance != this) {
            Destroy(gameObject);
        }

        serverConnection = new ServerConnection();
        currentShopItems = new List<Item>();
        player = new Player();
        defeatedEnemies = new List<string>();
        isBossFight = false;
        distanceTraveled = 0;
        onlineBattle = false;

        DontDestroyOnLoad(gameObject);
    }

    public string getCurrentGuild() {
        return currentGuild;
    }

    public List<string> getDefeatedEnemies() {
        return defeatedEnemies;
    }

    public void setDefeatedEnemies(List<string> value) {
        defeatedEnemies = value;
    }

    public void setCurrentGuild(string value) {
        currentGuild = value;
    }
    
    public EnemyCatalog getEnemyCatalog() {
        return enemyCatalog;
    }

    public SkillCatalog getSkillCatalog() {
        return skillCatalog;
    }

    public ItemCatalog getItemCatalog() {
        return itemCatalog;
    }

    // Crea un nuevo personaje
    public void setPlayer() {      
        updatePlayer();
    }

    public Player getPlayer() {
        return player;
    }

    // Peticion al servidor para obtener los datos del gremio
    public void loadGuild(string value) {
        currentGuild = value;
        //StartCoroutine(getServerConnection().getRequest("shop/" + value.ToString(), getShopResponse));
        SceneManager.LoadScene("GuildScene");
    }

    // Peticion al servidor de los objetos de la tienda
    public void loadShop(int value) {
        currentShopItems.Clear();
        StartCoroutine(getServerConnection().getRequest("shop/" + value.ToString(), getShopResponse));   
    }

    // Obtiene la respuesta con los objetos de la tienda
    public void getShopResponse(JSONObject json) {		
			JSONObject array = json.GetField("items_names");
			foreach(JSONObject j in array.list) {
				currentShopItems.Add(itemCatalog.getItemByName(j.GetField("item_name").str));				
			}
            SceneManager.LoadScene("ShopScene");
	}

    public List<Item> getCurrentShopItems() {
        return currentShopItems;
    }

    public void changeScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }

    public ServerConnection getServerConnection() {
        return serverConnection;
    }

    // Guarda el usuario
    public void setUser(string email, string password) {
        if(user != null) {
            user.setEmail(email);
            user.setPassword(password);
        }

        else {
            user = new User(email, password);
        }
    }

    public User getUser() {
        return user;
    }

    // Se obtienen los datos del jugador del servidor
    public void updatePlayer() {    
        updated_player = false;
        StartCoroutine(serverConnection.getRequest("user_player/" + user.getEmail(), getPlayerResponse));                
    }    

    public void getPlayerResponse(JSONObject json) {
        
        List<Item> inventory = new List<Item>();
        // Se actualiza el inventario
        JSONObject array_items = json.GetField("inventory");
        inventory.Clear();
        if(array_items != null) {
            foreach(JSONObject j in array_items.list) {                      
                inventory.Add(itemCatalog.getItemByName(j.GetField("item_name").str));				
            }
        }
        player.setInventory(inventory);

        // Se actualizan las habilidades
        List<Skill> skills = new List<Skill>();
        JSONObject array_skills = json.GetField("skills");
        skills.Clear();
        if(array_skills != null) {
            foreach(JSONObject j in array_skills.list) {
                skills.Add(skillCatalog.getSkillByName(j.GetField("skill_name").str));				
            }            
        }
        player.setSkills(skills);

        if(json.GetField("weapon").str != "none") {
            player.setWeapon(itemCatalog.getItemByName(json.GetField("weapon").str));
        }
        if(json.GetField("accessory").str != "none") {
            player.setAccessory(itemCatalog.getItemByName(json.GetField("accessory").str));
        }
        player.setName(json.GetField("name").str);

        // Se actualiza la clase
        if(json.GetField("class").str == "wizard") {
            player.setJob(Job.Wizard);
        }
        else if (json.GetField("class").str == "warrior") {
            player.setJob(Job.Warrior);
        }
        else {
            player.setJob(Job.Swordsman);
        }

        // Se actualiza la faccion
        if(json.GetField("faction").str == "science") {
            player.setFaction(Faction.Science);
        }
        else if (json.GetField("faction").str == "engineering") {
            player.setFaction(Faction.Engineering);
        }
        else {
            player.setFaction(Faction.ArtsAndHumanities);
        }

        player.setLevel(int.Parse(json.GetField("player_level").str));
        player.setExperience(int.Parse(json.GetField("experience").str));
        player.setCurrentExperience(int.Parse(json.GetField("current_experience").str));
        player.setMoney(int.Parse(json.GetField("money").str));

        // Se actualizan las estadisticas principales
        player.setStat(Stat.Strength, int.Parse(json.GetField("strength").str));
        player.setStat(Stat.BaseDamage, int.Parse(json.GetField("base_damage").str));
        player.setStat(Stat.MaxHealth, int.Parse(json.GetField("max_health").str));
        player.setStat(Stat.CurrentHealth, int.Parse(json.GetField("current_health").str));
        player.setStat(Stat.ManaPoints, int.Parse(json.GetField("mana").str));
        player.setStat(Stat.CurrentManaPoints, int.Parse(json.GetField("current_mana").str));
        player.setStat(Stat.Dextery, int.Parse(json.GetField("dextery").str));
        player.setStat(Stat.Intelligence, int.Parse(json.GetField("intelligence").str));
        updated_player = true;
    }

    public IEnumerator waitPlayerUpdate(string value, System.Action<string> callBack) {
        yield return new WaitUntil(() => updated_player);
        callBack(value);        
        yield return new WaitForSeconds(0);
    }

    public void deleteItem(Item value) {
        JSONObject json = new JSONObject(JSONObject.Type.OBJECT);
        json.AddField("email", user.getEmail());
        json.AddField("user_password", user.getPassword());
        json.AddField("item_name", value.getName());
        StartCoroutine(serverConnection.postRequest(json, "delete_item", null));
    }

    public void sendPlayerAttributes() {
        JSONObject json = new JSONObject(JSONObject.Type.OBJECT);
        json.AddField("email", user.getEmail());
        json.AddField("user_password", user.getPassword());
        json.AddField("strength", player.getStat(Stat.Strength));
        json.AddField("dextery", player.getStat(Stat.Dextery));
        json.AddField("intelligence", player.getStat(Stat.Intelligence));
        json.AddField("max_health", player.getStat(Stat.MaxHealth));
        json.AddField("current_health", player.getStat(Stat.CurrentHealth));
        json.AddField("mana", player.getStat(Stat.ManaPoints));
        json.AddField("current_mana", player.getStat(Stat.CurrentManaPoints));
        json.AddField("money", player.getMoney());
        json.AddField("base_damage", player.getStat(Stat.BaseDamage));
        StartCoroutine(serverConnection.postRequest(json, "update_attributes"));        
    }

    public void sendItem(Item value) {
        JSONObject json = new JSONObject(JSONObject.Type.OBJECT);
        json.AddField("email", user.getEmail());
        json.AddField("user_password", user.getPassword());
        json.AddField("item_name", value.getName());
        StartCoroutine(serverConnection.postRequest(json, "add_item", null));
    }

    public void sendEquipItems() {
        JSONObject json = new JSONObject(JSONObject.Type.OBJECT);
        json.AddField("email", user.getEmail());
        json.AddField("user_password", user.getPassword());
        json.AddField("weapon_name", player.getWeapon().getName());
        json.AddField("accessory_name", player.getAccessory().getName());
        StartCoroutine(serverConnection.postRequest(json, "equip_items", null));
    }

    public void loadBossBattle(int value, int braveryPoints) {        
        JSONObject json = new JSONObject(JSONObject.Type.OBJECT);
        json.AddField("email", user.getEmail());
        json.AddField("user_password", user.getPassword());
        json.AddField("bravery_points", braveryPoints);
        json.AddField("boss_id", value);
        StartCoroutine(serverConnection.postRequest(json, "use_bravery_points", getBossBattleResponse));
    }

    public void getBossBattleResponse(JSONObject json) {
        isBossFight = true;
        Debug.Log(json.GetField("boss_name").str);
        bossName = json.GetField("boss_name").str;
        SceneManager.LoadScene("BattleScene"); 
    }
    
    public bool checkBossFight() {
        return isBossFight;
    }

    public void changeBossFight() {
        isBossFight = false;
    }

    public string getBossName() {
        return bossName;
    }

    public void addDistanceTraveled(float value) {
        distanceTraveled += value;
        if(distanceTraveled >= 100.0) {
            distanceTraveled = 0;
            JSONObject json = new JSONObject(JSONObject.Type.OBJECT);
            json.AddField("email", user.getEmail());
            json.AddField("user_password", user.getPassword());
            StartCoroutine(serverConnection.postRequest(json, "add_bravery_point", null));
        }
    }

    public float getDistanceTraveled() {
        return distanceTraveled;
    }

    public void playersInteraction(int id, Faction value, string name) {

        onlinePlayerId = id;
        onlinePlayerName = name;
        // Abre la ventana de chat si es de la misma faccion
        if(player.getFaction() == value) {
            SceneManager.LoadScene("ChatScene");
        }
        // Combate si son de diferente faccion
        else {            
            JSONObject json = new JSONObject(JSONObject.Type.OBJECT);
            json.AddField("email", user.getEmail());
            json.AddField("user_password", user.getPassword());
            json.AddField("second_player_id", id);            
            StartCoroutine(serverConnection.postRequest(json, "start_online_battle", getOnlinePlayer));
        }
    }

    public void getOnlinePlayer(JSONObject json) {    
        onlineBattle = true;
        onlinePlayer.setStat(Stat.Strength, int.Parse(json.GetField("strength").str));
        onlinePlayer.setStat(Stat.BaseDamage, int.Parse(json.GetField("base_damage").str));
        onlinePlayer.setStat(Stat.MaxHealth, int.Parse(json.GetField("max_health").str));
        onlinePlayer.setStat(Stat.CurrentHealth, int.Parse(json.GetField("current_health").str));
        onlinePlayer.setStat(Stat.ManaPoints, int.Parse(json.GetField("mana").str));
        onlinePlayer.setStat(Stat.CurrentManaPoints, int.Parse(json.GetField("current_mana").str));
        onlinePlayer.setStat(Stat.Dextery, int.Parse(json.GetField("dextery").str));
        onlinePlayer.setStat(Stat.Intelligence, int.Parse(json.GetField("intelligence").str));
        onlinePlayer.setName(json.GetField("name").str);
        onlinePlayerId = int.Parse(json.GetField("online_player_id").str);
        onlineBattleId = int.Parse(json.GetField("online_battle_id").str);
        player.setId(int.Parse(json.GetField("local_player_id").str));
        if(json.GetField("class").str == "wizard") {
            onlinePlayerJob = Job.Wizard;
        }
        else if (json.GetField("class").str == "warrior") {
            onlinePlayerJob = Job.Warrior;
        }
        else {
            onlinePlayerJob = Job.Swordsman;
        }
        
        SceneManager.LoadScene("BattleScene"); 
    }

    public Enemy getOnlinePlayer() {
        return onlinePlayer;
    }

    public bool isOnlineBattle() {
        return onlineBattle;
    }

    public void changeOnlineBattle() {
        onlineBattle = false;
    }

    public int getOnlineBattleId() {
        return onlineBattleId;
    }

    public int getOnlinePlayerId() {
        return onlinePlayerId;
    }

    public Job getOnlinePlayerJob() {
        return onlinePlayerJob;
    }

    public string getOnlinePlayerName() {
        return onlinePlayerName;
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
