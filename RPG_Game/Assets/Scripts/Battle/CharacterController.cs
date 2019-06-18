using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterController : MonoBehaviour
{       
    private BattleAction action;
    private BattleManager battleManager;
    private Player player;
    [SerializeField]
    private Enemy enemy;
    [SerializeField]
    private CharacterType characterType;
    private bool dead;
    private List<Slider> healthBars;
    [SerializeField]
    private List<GameObject> childs;
    [SerializeField]
    private int position;
    private bool nextTurn;
    private int target;
    private Skill selectedSkill;
    private Slider manaBar;
    [SerializeField]
    private GameObject manaBarChild;
    private List<Stat> modifiedStats;
    private List<int> modifiedStatsCounter;
    private List<int> modifiedStatsValue;
    private List<SkillType> modifiedStatsSkillType;
    private Item selectedItem;
    [SerializeField]
    private Image characterSprite;
    [SerializeField]
    private GameObject warriorSprite;

    [SerializeField]
    private GameObject wizardSprite;

    [SerializeField]
    private GameObject swordsmanSprite;
    private bool updatedStat;
    private bool requestedStat;

    void Awake() {
        action = new BattleAction();
        battleManager = GameObject.Find("BattleManager").GetComponent<BattleManager>();        
        GameManager gameManager = GameManager.instance;
        dead = false;
        nextTurn = true;
        if(characterType == CharacterType.Player) {
            player = gameManager.getPlayer();
            Debug.Log("id del jugador: " + player.getId());
            if(player.getJob() == Job.Warrior) {
                characterSprite.sprite = warriorSprite.GetComponent<SpriteRenderer>().sprite;
            }
            else if(player.getJob() == Job.Wizard) {
                characterSprite.sprite = wizardSprite.GetComponent<SpriteRenderer>().sprite;
            }
            else {
                characterSprite.sprite = swordsmanSprite.GetComponent<SpriteRenderer>().sprite;
            }
        }
        if(characterType == CharacterType.Enemy) {
            if(!gameManager.isOnlineBattle()) {
                if(gameManager.checkBossFight()) {
                    enemy = gameManager.getEnemyCatalog().getEnemyByName(gameManager.getBossName());                
                }
                else {
                    enemy = gameManager.getEnemyCatalog().getRandomEnemy();
                }
                enemy = Instantiate(enemy);
                enemy.adjustAttributes(gameManager.getPlayer().getLevel());
                characterSprite.sprite = enemy.getEnemySprite().GetComponent<SpriteRenderer>().sprite;
            }
            else {
                enemy = Instantiate(gameManager.getOnlinePlayer());
                if(gameManager.getOnlinePlayerJob() == Job.Warrior) {
                    characterSprite.sprite = warriorSprite.GetComponent<SpriteRenderer>().sprite;
                }
                else if(gameManager.getOnlinePlayerJob() == Job.Wizard) {
                    characterSprite.sprite = wizardSprite.GetComponent<SpriteRenderer>().sprite;
                }
                else {
                    characterSprite.sprite = swordsmanSprite.GetComponent<SpriteRenderer>().sprite;                    
                }
                transform.Rotate(0, 180, 0, Space.Self);
            }
          
            target = 0;
        }
        healthBars = new List<Slider>();

        for(int i = 0; i < childs.Count; i++) {
            healthBars.Add(childs[i].GetComponent<Slider>()); 
            healthBars[i].maxValue = getStat(Stat.MaxHealth);
            healthBars[i].value = getStat(Stat.CurrentHealth);
        }

        if(characterType == CharacterType.Player) {
            manaBar = manaBarChild.GetComponent<Slider>();
            manaBar.maxValue = player.getStat(Stat.ManaPoints);
            manaBar.value = player.getStat(Stat.CurrentManaPoints); 
        }

        modifiedStatsCounter = new List<int>();
        modifiedStatsValue = new List<int>();
        modifiedStats = new List<Stat>();
        modifiedStatsSkillType = new List<SkillType>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    public void setBattleAction(BattleActionType battleActionType, Stat stat, int value, int target) {
        GameManager gameManager = GameManager.instance;
        Debug.Log("se va a elegir el modo");   

        if(!dead && !action.isSelected() && nextTurn && (!gameManager.isOnlineBattle())) {
            action.setBattleAction(battleActionType, stat, value, target);
            nextTurn = false;  
           
            // Actualizamos las ventajas y desventajas
            for(int i = 0; i < modifiedStatsCounter.Count; i++) {
                Debug.Log("llega action offline");   
                // Si ya terminado el numero de turnos vuelve a la normalidad
                if(modifiedStatsCounter[i] <= 0) {
                    if(modifiedStatsSkillType[i] == SkillType.BuffSkill) {
                        setStat(modifiedStats[i], getStat(modifiedStats[i]) - modifiedStatsValue[i]);
                    }
                    else {
                        setStat(modifiedStats[i], getStat(modifiedStats[i]) + modifiedStatsValue[i]);
                    }

                }  
                else {
                    modifiedStatsCounter[i] = modifiedStatsCounter[i] - 1;
                }
                modifiedStats.RemoveAt(i);
                modifiedStatsCounter.RemoveAt(i);
                modifiedStatsValue.RemoveAt(i);
                modifiedStatsSkillType.RemoveAt(i);
                i--;
            }              
            StartCoroutine(battleManager.checkParticipants());       
        }

        else if(!dead && !action.isSelected() && nextTurn && gameManager.isOnlineBattle() && (characterType == CharacterType.Player)) {
            Debug.Log("llega action online");     
            action.setBattleAction(battleActionType, stat, value, target);
            nextTurn = false;  
           
            // Actualizamos las ventajas y desventajas
            for(int i = 0; i < modifiedStatsCounter.Count; i++) {
                // Si ya terminado el numero de turnos vuelve a la normalidad
                if(modifiedStatsCounter[i] <= 0) {
                    if(modifiedStatsSkillType[i] == SkillType.BuffSkill) {
                        setStat(modifiedStats[i], getStat(modifiedStats[i]) - modifiedStatsValue[i]);
                    }
                    else {
                        setStat(modifiedStats[i], getStat(modifiedStats[i]) + modifiedStatsValue[i]);
                    }

                }  
                else {
                    modifiedStatsCounter[i] = modifiedStatsCounter[i] - 1;
                }
                modifiedStats.RemoveAt(i);
                modifiedStatsCounter.RemoveAt(i);
                modifiedStatsValue.RemoveAt(i);
                modifiedStatsSkillType.RemoveAt(i);
                i--;
            }

            JSONObject json = new JSONObject(JSONObject.Type.OBJECT);
            json.AddField("player_id", gameManager.getPlayer().getId());
            json.AddField("finish_action", "no");
            if(battleActionType == BattleActionType.Attack) {
                json.AddField("command", "attack");
            }
            else if(battleActionType == BattleActionType.Skill) {
                json.AddField("command", "skill");
            }
            else if(battleActionType == BattleActionType.Item) {
                json.AddField("command", "item");
            }
            else if(battleActionType == BattleActionType.Defend) {
                json.AddField("command", "defend");
            }
            else {
                json.AddField("command", "escape");
            }  
            Debug.Log("se va a enviar battle action");          
            StartCoroutine(gameManager.getServerConnection().postRequest(json, "set_online_battle_command", null));          
            Debug.Log("envia battle action");  
            StartCoroutine(battleManager.checkParticipants()); 
            Debug.Log("comprueba participantes");
        }
        else if(!dead && !action.isSelected() && nextTurn && gameManager.isOnlineBattle() && (characterType == CharacterType.Enemy)) {
            nextTurn = false;
            JSONObject json = new JSONObject(JSONObject.Type.OBJECT);
            json.AddField("player_id", gameManager.getOnlinePlayerId());
            StartCoroutine(gameManager.getServerConnection().postRequest(json, "get_online_battle_command", getCommandResponse));
        }        
    }

    public void getCommandResponse (JSONObject json) {
        GameManager gameManager = GameManager.instance;
        Debug.Log("llegacommand");        
        if(json.GetField("finish_action").str == "yes") {
            JSONObject j = new JSONObject(JSONObject.Type.OBJECT);
            j.AddField("player_id", gameManager.getOnlinePlayerId());
            j.AddField("finish_action", "no");
            j.AddField("command", "none");
            StartCoroutine(gameManager.getServerConnection().postRequest(j, "set_online_battle_command", null));
            Debug.Log("llegacommand3");
            updateHealthBar();
            updateManaBar();
            setNextTurn();
            getBattleAction().completeAction();            
        }
        else if(json.GetField("command").str != "none") {
            if((json.GetField("command").str == "finish") || (json.GetField("command").str == "escape")) {
                battleManager.finishBattle();                
            }
            Debug.Log("llegacommand2");
            JSONObject j = new JSONObject(JSONObject.Type.OBJECT);
            j.AddField("player_id", gameManager.getOnlinePlayerId());
            j.AddField("finish_action", "no");
            j.AddField("command", "none");
            StartCoroutine(gameManager.getServerConnection().postRequest(j, "set_online_battle_command", null));
            action.setBattleAction(BattleActionType.Attack, Stat.CurrentHealth, 0, 0);
            StartCoroutine(battleManager.checkParticipants()); 
            StartCoroutine(waitForCommand());
        }
        else if(json.GetField("finish_action").str == "no" && json.GetField("command").str == "none") {        
            StartCoroutine(waitForCommand());
        }        
    }

    public IEnumerator waitForCommand() {
        GameManager gameManager = GameManager.instance;
        JSONObject j = new JSONObject(JSONObject.Type.OBJECT);
        j.AddField("player_id", gameManager.getOnlinePlayerId());   
        yield return new WaitForSeconds(10);
        StartCoroutine(gameManager.getServerConnection().postRequest(j, "get_online_battle_command", getCommandResponse));
    }

    // Comandos del jugador
    // Atacar
    public void attack() {
        Debug.Log("se ha elegido atacar");   
        if(!dead && !action.isSelected() && nextTurn) {
            if(player.getWeapon() != null) {
                setBattleAction(BattleActionType.Attack, Stat.CurrentHealth, player.getWeapon().getValue(0) + getStat(Stat.BaseDamage), target);
            }
            else {
                setBattleAction(BattleActionType.Attack, Stat.CurrentHealth, getStat(Stat.BaseDamage), target);
            }
        }        
    }

    // Escapar
    public void escape() {
        if(!dead && !action.isSelected() && nextTurn) {            
            setBattleAction(BattleActionType.Escape, Stat.CurrentHealth, getStat(Stat.BaseDamage), target);       
        }
    }

    // Defender
    public void defend() {
        if(!dead && !action.isSelected() && nextTurn) {            
            setBattleAction(BattleActionType.Defend, Stat.CurrentHealth, getStat(Stat.BaseDamage), target);       
        }
    }

    // Usar habilidad
    public void useSkill() {   
        if(!dead && !action.isSelected() && nextTurn && (characterType == CharacterType.Player)) {      
            BattleSkillsManager battleSkillsManager = GameObject.Find("SkillsPanel").GetComponent<BattleSkillsManager>();    
            if(battleSkillsManager.getSelectedSkill() != null) {
                selectedSkill = battleSkillsManager.getSelectedSkill();
                if(selectedSkill.getManaCost() <= player.getStat(Stat.CurrentManaPoints)) {
                    battleManager.returnButton();                
                    setBattleAction(BattleActionType.Skill, battleSkillsManager.getSelectedSkill().getTargetStat(), battleSkillsManager.getSelectedSkill().getValue(), target);
                }               
            }    
        }
    }

    // Usar objeto
    public void useItem() {   
        if(!dead && !action.isSelected() && nextTurn && (characterType == CharacterType.Player)) {      
            BattleItemManager battleItemManager = GameObject.Find("ItemsPanel").GetComponent<BattleItemManager>();    
            if(battleItemManager.getSelectedItem() != null) {
                selectedItem = battleItemManager.getSelectedItem();
                if((selectedItem.getTargetStat(0) == Stat.CurrentHealth) && (getStat(Stat.CurrentHealth) < getStat(Stat.MaxHealth))) {
                    battleItemManager.resetItemList();
                    battleManager.returnButton();                
                    setBattleAction(BattleActionType.Item, selectedItem.getTargetStat(0), selectedItem.getValue(0), target);
                }
                else if((selectedItem.getTargetStat(0) == Stat.CurrentManaPoints) && (getStat(Stat.CurrentManaPoints) < getStat(Stat.ManaPoints))) {
                    battleItemManager.resetItemList();
                    battleManager.returnButton();                
                    setBattleAction(BattleActionType.Item, selectedItem.getTargetStat(0), selectedItem.getValue(0), target);
                }                     
            }    
        }
    }

    public string getName() {
        if(characterType == CharacterType.Enemy) {
            return enemy.getName();
        }
        else {
            return player.getName();
        }
    }

    public void changeTarget(int value) {
        target = value;
    }

    public BattleAction getBattleAction() {
        return action;
    }

    public Item getSelectedItem() {
        return selectedItem;
    }

    public int getStat(Stat stat) {        
        if(characterType == CharacterType.Enemy) {
            return enemy.getStat(stat);
        }
        else {
            return player.getStat(stat);
        }
    }

    public IEnumerator sendStats() {
        GameManager gameManager = GameManager.instance;
        if(gameManager.isOnlineBattle()) {
            requestedStat = false;
            JSONObject json = new JSONObject(JSONObject.Type.OBJECT);
            if(characterType == CharacterType.Player) {
                json.AddField("player_id", player.getId());
                json.AddField("strength", player.getStat(Stat.Strength));
                json.AddField("dextery", player.getStat(Stat.Dextery));
                json.AddField("intelligence", player.getStat(Stat.Intelligence));
                json.AddField("current_health", player.getStat(Stat.CurrentHealth));
                json.AddField("current_mana", player.getStat(Stat.CurrentManaPoints));
                json.AddField("base_damage", player.getStat(Stat.BaseDamage));
            }
            else {
                json.AddField("player_id", gameManager.getOnlinePlayerId());
                json.AddField("strength", enemy.getStat(Stat.Strength));
                json.AddField("dextery", enemy.getStat(Stat.Dextery));
                json.AddField("intelligence", enemy.getStat(Stat.Intelligence));
                json.AddField("current_health", enemy.getStat(Stat.CurrentHealth));
                json.AddField("current_mana", enemy.getStat(Stat.CurrentManaPoints));
                json.AddField("base_damage", enemy.getStat(Stat.BaseDamage));
            }

            StartCoroutine(gameManager.getServerConnection().postRequest(json, "set_player_battle_stats", sendStatsResponse));
            yield return new WaitForSeconds(0);   
        }
        else {
            requestedStat = true;
            yield return new WaitForSeconds(0);
        }
    }      

    public void sendStatsResponse(JSONObject json) {
        requestedStat = true;
    }

    public IEnumerator updateStats() {
        GameManager gameManager = GameManager.instance;
        if(gameManager.isOnlineBattle()) {
            JSONObject json = new JSONObject(JSONObject.Type.OBJECT);
            updatedStat = false;
            if(characterType == CharacterType.Player) {
                json.AddField("player_id", player.getId());
            }
            else {
                json.AddField("player_id", gameManager.getOnlinePlayerId());
            }
            StartCoroutine(gameManager.getServerConnection().postRequest(json, "get_player_battle_stats", getStatsResponse));
            yield return new WaitForSeconds(0);
        }
        else {
            updatedStat = true;
            yield return new WaitForSeconds(0);
        }
    }

    public bool isRequestedStatReady() {
        return requestedStat;
    }

    public bool isStatUpdated() {
        return updatedStat;
    }

    public void getStatsResponse(JSONObject json) {       
        if(characterType == CharacterType.Player) {
            player.setStat(Stat.Strength, int.Parse(json.GetField("strength").str));
            player.setStat(Stat.BaseDamage, int.Parse(json.GetField("base_damage").str));
            player.setStat(Stat.CurrentHealth, int.Parse(json.GetField("current_health").str));
            player.setStat(Stat.CurrentManaPoints, int.Parse(json.GetField("current_mana").str));
            player.setStat(Stat.Dextery, int.Parse(json.GetField("dextery").str));
            player.setStat(Stat.Intelligence, int.Parse(json.GetField("intelligence").str));
        }
        else {
            enemy.setStat(Stat.Strength, int.Parse(json.GetField("strength").str));
            enemy.setStat(Stat.BaseDamage, int.Parse(json.GetField("base_damage").str));
            enemy.setStat(Stat.CurrentHealth, int.Parse(json.GetField("current_health").str));
            enemy.setStat(Stat.CurrentManaPoints, int.Parse(json.GetField("current_mana").str));
            enemy.setStat(Stat.Dextery, int.Parse(json.GetField("dextery").str));
            enemy.setStat(Stat.Intelligence, int.Parse(json.GetField("intelligence").str));
        }
        updatedStat = true;
    }

    public void setStat(Stat stat, int value) {    
        if(characterType == CharacterType.Enemy) {
            enemy.setStat(stat, value);
        }
        else {
            player.setStat(stat, value);
        }        
    }

    public Player getPlayer() {
        return player;
    }

    public Enemy getEnemy() {
        return enemy;
    }

    public void setDead() {
        dead = true;
    }

    public bool isDead() {
        return dead;
    }

    public int getPosition() {
        return position;
    }

    public void updateHealthBar() {
        for(int i = 0; i < childs.Count; i++) {
            healthBars[i].maxValue = getStat(Stat.MaxHealth);
            healthBars[i].value = getStat(Stat.CurrentHealth);         
        }
    }

    public void updateManaBar() {
        if(characterType == CharacterType.Player) {
            manaBar.maxValue = getStat(Stat.ManaPoints);
            manaBar.value = getStat(Stat.CurrentManaPoints);      
        }
    }

    public void setNextTurn() {
        nextTurn = true;
    }

    public Skill getSelectedSkill() {
        return selectedSkill;
    }

    public void setSelectedSkill(Skill newSkill) {
        selectedSkill = newSkill;
    }

    public CharacterType getCharacterType() {
        return characterType;
    }

    // Pone un contador para una modificacion en una estadistica
    public void setStatCounter(SkillType skillType, Stat stat, int value, int activeTurns) {
        modifiedStats.Add(stat);
        modifiedStatsCounter.Add(activeTurns);
        modifiedStatsValue.Add(value);
        modifiedStatsSkillType.Add(skillType);
    }

    // Devuelve a la normalidad todas las estadisticas
    public void clearStats() {
        for(int i = 0; i < modifiedStats.Count; i++) {
            if(modifiedStatsSkillType[i] == SkillType.BuffSkill) {
                        setStat(modifiedStats[i], getStat(modifiedStats[i]) - modifiedStatsValue[i]);
            }
            else {
                setStat(modifiedStats[i], getStat(modifiedStats[i]) + modifiedStatsValue[i]);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {      
        if(characterType == CharacterType.Enemy) {
            if(!dead && !action.isSelected() && nextTurn) {
                setBattleAction(BattleActionType.Attack, Stat.CurrentHealth, getStat(Stat.BaseDamage), target);
            }
        }
    }

}
