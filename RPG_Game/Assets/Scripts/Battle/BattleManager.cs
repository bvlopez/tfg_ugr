using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    private int enemyCount;
    private List<CharacterController> characters;
    private int deadEnemies;
    // PRUEBAS BORRAR
    public GameObject[] participants;
    [SerializeField]
    private GameObject controlPanel;
    [SerializeField]
    private GameObject skillsPanel;
    [SerializeField]
    private GameObject itemsPanel;
    private GameObject activePanel;
    [SerializeField]
    private GameObject[] logLines;
    private int nextLine;
    private List<string> enemyNames;
    private bool actionFinish;

    void Awake() {
        GameManager gameManager = GameManager.instance;
        skillsPanel.SetActive(false);
        itemsPanel.SetActive(false);
        controlPanel.SetActive(true);
        activePanel = controlPanel;
        if(gameManager.checkBossFight() || gameManager.isOnlineBattle()) {
            enemyCount = 1;
            characters = new List<CharacterController>();   
            characters.Add(participants[0].GetComponent<CharacterController>());
            characters.Add(participants[1].GetComponent<CharacterController>());
            characters.Add(participants[2].GetComponent<CharacterController>());
            characters.Add(participants[3].GetComponent<CharacterController>());
            enemyNames = new List<string>();
            enemyNames.Add(characters[1].getName());
            participants[2].SetActive(false);
            participants[3].SetActive(false);
            gameManager.changeBossFight();
        }
        else {
            // Se decide el numero de enemigos
            enemyCount = Random.Range(0, 3) + 1;        
            // PRUEBAS BORRAR
            //enemyCount = 3;
            // Posicion 0 para el jugador
            characters = new List<CharacterController>();   
            characters.Add(participants[0].GetComponent<CharacterController>());
            characters.Add(participants[1].GetComponent<CharacterController>());
            characters.Add(participants[2].GetComponent<CharacterController>());
            characters.Add(participants[3].GetComponent<CharacterController>());

            enemyNames = new List<string>();
            enemyNames.Add(characters[1].getName());
            if(enemyCount < 2) {
                participants[2].SetActive(false);
            }
            else {
                enemyNames.Add(characters[2].getName());
            }
            if(enemyCount < 3) {
                participants[3].SetActive(false);
            }
            else {
                enemyNames.Add(characters[3].getName());
            }
        }

        deadEnemies = 0;
    }

    // Start is called before the first frame update
    void Start()
    {       
        
    }

    public IEnumerator checkParticipants() { 
        Debug.Log("llega1participants");        
        bool allSelected = true;
        // Si todos los participantes han seleccionado una accion se ejecutan
        for(int i = 0; i <= enemyCount; i++) {
            if((!characters[i].getBattleAction().isSelected()) && (!characters[i].isDead())) {
                allSelected = false;
            }
        }
        
        if(allSelected) {
            Debug.Log("llega2participants");
            for(int i = 0; i <= enemyCount; i++) {
                characters[i].getBattleAction().completeAction();
            }
            allSelected = false;
            StartCoroutine(executeActions());
        }               
        
        yield return new WaitForSeconds(0);
    }

    public IEnumerator executeActions() {             
        GameManager gameManager = GameManager.instance;
        cleanLog();
        nextLine = 0;

        // Se decide el orden        
        List<CharacterController> order = new List<CharacterController>(); 
        for(int i = 0; i <= enemyCount; i++) {
            order.Add(characters[i]);
            StartCoroutine(order[i].updateStats());               
        }
        if(gameManager.isOnlineBattle()) {
            yield return new WaitForSeconds(2);   
        }        
               
        CharacterController tmp;
        for(int i = 1; i <= enemyCount; i++) {
            for(int j = 0; j <= enemyCount - 1; j++) {
                if(order[j].getStat(Stat.Dextery) > order[j + 1].getStat(Stat.Dextery)) {
                    tmp = order[j];
                    order[j] = order[j + 1];
                    order[j + 1] = tmp;
                }
            }
        } 
        
        // Se ejecutan las acciones en el orden establecido    
        for(int i = 0; i <= enemyCount; i++) {
            if(!(order[i].isDead())) {
                if(!gameManager.isOnlineBattle() || (order[i].getCharacterType() == CharacterType.Player)) {
                    // Ataque basico
                    if(order[i].getBattleAction().getBattleActionType() == BattleActionType.Attack) {
                        attackAction(order[i]);
                    }

                    // Defender
                    else if(order[i].getBattleAction().getBattleActionType() == BattleActionType.Defend) {
                        defendAction(order[i]);
                    }

                    // Huir
                    else if(order[i].getBattleAction().getBattleActionType() == BattleActionType.Escape) {
                        escapeAction(order[i]);
                    }

                    // Habilidad
                    else if(order[i].getBattleAction().getBattleActionType() == BattleActionType.Skill) {
                        skillAction(order[i]);
                    }

                    // Objeto
                    else if(order[i].getBattleAction().getBattleActionType() == BattleActionType.Item) {
                        itemAction(order[i]);
                    }           

                    Debug.Log("llega1");
                    order[i].getBattleAction().completeAction();
                }               

                // comprobamos si se ha terminado la batalla            
                for(int j = 0; j <= enemyCount; j++) {
                    StartCoroutine(characters[j].sendStats());
                    if(gameManager.isOnlineBattle()) {
                        yield return new WaitForSeconds(1);   
                    }             
                    // Se actualizan las barras
                    characters[j].updateHealthBar();
                    characters[j].updateManaBar();
                    characters[j].setNextTurn();
                    JSONObject json = new JSONObject(JSONObject.Type.OBJECT);
                    json.AddField("player_id", gameManager.getPlayer().getId());
                    json.AddField("finish_action", "yes");
                    json.AddField("command", "none");
                    StartCoroutine(gameManager.getServerConnection().postRequest(json, "set_online_battle_command", null));
                    // Se comprueba la vida
                    if(characters[j].getStat(Stat.CurrentHealth) <= 0) { 
                        participants[j].SetActive(false);                 
                        if(j == 0) {
                            finishBattle();
                        }
                        else if(!(characters[j].isDead())) {
                            deadEnemies++;
                        }                
                        characters[j].setDead();
                        if(deadEnemies >= enemyCount) {
                            finishBattle();
                        }
                    }
                }
                yield return new WaitForSeconds(0);
            }

        }            
    }

    public void attackAction(CharacterController characterController) {
        int target = characterController.getBattleAction().getTarget();
        int value = characterController.getBattleAction().getValue();
        Stat stat = characterController.getBattleAction().getStat();

        bool counterAttack = false;    
        int random_value = 0;
        int max = 0;    
        value = (int)(value+(characterController.getStat(Stat.Strength)*0.4));

        // Si el objetivo se esta defendiendo
        if(characters[target].getBattleAction().getBattleActionType() == BattleActionType.Defend) {
            // Probabilidad de contraataque
            random_value = Random.Range(0, 100) + 1;
            max = 2 + (int)(characterController.getStat(Stat.Dextery)/4);
            if((random_value >= 1) && (random_value <= max)) {
                counterAttack = true;
            }           
        }       

        // Si se produce contraataque
        if(counterAttack) {            
            // Al ataque se le suma valor por la fuerza del que contraataca
            value = characters[target].getStat(Stat.BaseDamage);
            value = (int)(value+(characters[target].getStat(Stat.Strength)*0.4));
            characterController.setStat(stat, characterController.getStat(Stat.CurrentHealth) - value);            
            writeLogLine(characters[target].getName() + " ha contraatacado a " + characterController.getName() + " con " + value + " de daño");
        }

        // Si no se produce contraataque
        else {            
            // Probabilidad de critico
            random_value = Random.Range(0, 100) + 1;
            max = 10 + characterController.getStat(Stat.Dextery);
            if((random_value >= 1) && (random_value <= max)) {
                value = value*2;
            }
            // En caso de que se haya defendido y no haya conseguido un contraataque
            if(characters[target].getBattleAction().getBattleActionType() == BattleActionType.Defend) {
                value = value - ((int)(value*0.4));
            }       
            // Al ataque se le suma valor por la fuerza del que ataca
            characters[target].setStat(stat, characters[target].getStat(Stat.CurrentHealth) - value);            
            writeLogLine(characterController.getName() + " ha atacado a " + characters[target].getName() + " con " + value + " de daño");
        }

        actionFinish = true;
    }

    public void defendAction(CharacterController characterController) {
        // Se recupera vida y mana
        characterController.setStat(Stat.CurrentHealth, characterController.getStat(Stat.CurrentHealth) + (int)(characterController.getStat(Stat.Strength)*0.1));
        characterController.setStat(Stat.ManaPoints, characterController.getStat(Stat.ManaPoints) + (int)(characterController.getStat(Stat.Intelligence)*0.1));
        writeLogLine(characterController.getName() + " se esta defendiendo");
        actionFinish = true;
    }

    public void skillAction(CharacterController characterController) {
        Skill skill = characterController.getSelectedSkill();
        int value = characterController.getBattleAction().getValue();
        int target = characterController.getBattleAction().getTarget();
        Stat stat = characterController.getBattleAction().getStat();
        int random_value = 0;
        int max = 0;

        characterController.setStat(Stat.CurrentManaPoints, characterController.getStat(Stat.CurrentManaPoints) - skill.getManaCost());
        // Segun el tipo de habilidad se realizaran unas acciones u otras
        if(skill.getSkillType() == SkillType.DamageSkill) {           

            if(!skill.canMultiTarget()) {
                value = (int)(value+(characterController.getStat(Stat.Intelligence)*0.4));
                // Probabilidad de critico
                random_value = Random.Range(0, 100) + 1;
                max = 10 + characterController.getStat(Stat.Dextery);
                if((random_value >= 1) && (random_value <= max)) {
                    value = value*2;
                }

                // Si el objetivo se defiende
                if(characters[target].getBattleAction().getBattleActionType() == BattleActionType.Defend) {
                    value = value - ((int)(value*0.4));
                }  

                characters[target].setStat(stat, characters[target].getStat(Stat.CurrentHealth) - value);
                writeLogLine(characterController.getName() + " ha atacado con " + skill.getName() + " a "+ characters[target].getName() + " con " + value + " de daño");
            }

            // En caso de que afecte a todos
            else {
                value = (int)(value+(characterController.getStat(Stat.Intelligence)*0.4));
                for(int i = 1; i <= enemyCount; i++) {                    
                    int value_tmp = value;
                    // Probabilidad de critico
                    random_value = Random.Range(0, 100) + 1;
                    max = 10 + characterController.getStat(Stat.Dextery);
                    if((random_value >= 1) && (random_value <= max)) {
                        value_tmp = value_tmp*2;
                    }

                    // Si el objetivo se defiende
                    if(characters[i].getBattleAction().getBattleActionType() == BattleActionType.Defend) {
                        value_tmp = value_tmp - ((int)(value_tmp*0.4));
                    }  

                    characters[i].setStat(stat, characters[i].getStat(Stat.CurrentHealth) - value);
                }
                writeLogLine(characterController.getName() + " ha atacado con " + skill.getName() + " a todos los enemigos");
            }
        }

        // Mejora una estadistica
        else if(skill.getSkillType() == SkillType.BuffSkill) {
            characterController.setStatCounter(skill.getSkillType(), skill.getTargetStat(), skill.getValue(), skill.getActiveTurns());
            writeLogLine(characterController.getName() + " ha usado " + skill.getName() + " aumentado sus estadisticas durante " + skill.getActiveTurns() + " turnos");
        }

        // Empeora una estadistica del enemigo
        else {
            if(!skill.canMultiTarget()) {
                characters[target].setStatCounter(skill.getSkillType(), skill.getTargetStat(), skill.getValue(), skill.getActiveTurns());
                writeLogLine(characterController.getName() + " ha usado " + skill.getName() + " disminuyendo las estadisticas de  " + characters[target].getName() + " durante "+ skill.getActiveTurns() + " turnos");
            }
            else {
                for(int i = 1; i <= enemyCount; i++) {
                    characters[i].setStatCounter(skill.getSkillType(), skill.getTargetStat(), skill.getValue(), skill.getActiveTurns());
                    writeLogLine(characterController.getName() + " ha usado " + skill.getName() + " disminuyendo las estadisticas de todos los enemigos durante "+ skill.getActiveTurns() + " turnos");
                }
            }
        }
        actionFinish = true;
    }

    // Usa un objeto
    public void itemAction(CharacterController characterController) {
        characterController.getPlayer().useItem(characterController.getSelectedItem());        
        writeLogLine(characterController.getName() + " ha usado " + characterController.getSelectedItem().getName());
    }

    public void escapeAction(CharacterController characterController) {
        int random_value = Random.Range(0, 10) + 1;
        if((random_value >= 1) && (random_value <= 3)) {
            GameManager gameManager = GameManager.instance;
            gameManager.sendPlayerAttributes();
            SceneManager.LoadScene("WorldScene");
        }            
        writeLogLine(characterController.getName() + " ha intentado escapar");
        actionFinish = true;
    }

    public void finishBattle() {
        GameManager gameManager = GameManager.instance;      
        if(deadEnemies == enemyCount) {
            characters[0].clearStats();           
            gameManager.setDefeatedEnemies(enemyNames);
            gameManager.sendPlayerAttributes();
            JSONObject json = new JSONObject(JSONObject.Type.OBJECT);
            json.AddField("player_id", gameManager.getPlayer().getId());
            json.AddField("finish_action", "no");
            json.AddField("command", "finish");
            StartCoroutine(gameManager.getServerConnection().postRequest(json, "set_online_battle_command", null));
            SceneManager.LoadScene("RewardScene");
        }
        else {
            characters[0].clearStats();
            if(characters[0].getStat(Stat.CurrentHealth) < 0) {
                characters[0].setStat(Stat.CurrentHealth, 0);
            }
            gameManager.sendPlayerAttributes();
            JSONObject json = new JSONObject(JSONObject.Type.OBJECT);
            json.AddField("player_id", gameManager.getPlayer().getId());
            json.AddField("finish_action", "no");
            json.AddField("command", "finish");
            StartCoroutine(gameManager.getServerConnection().postRequest(json, "set_online_battle_command", null));
            SceneManager.LoadScene("WorldScene");
        }
    }

    public void addCharacter(CharacterController newCharacter) {
        characters[newCharacter.getPosition()] = newCharacter;
    }

    public void returnButton() {
        activePanel.SetActive(false);
        controlPanel.SetActive(true);
        activePanel = controlPanel;
    }

    public void setSkillsPanel() {
        controlPanel.SetActive(false);
        skillsPanel.SetActive(true);
        activePanel = skillsPanel;

        GameObject.Find("SkillsPanel").GetComponent<BattleSkillsManager>().setSkillsView(characters[0].getPlayer());
    }

    public void setItemsPanel() {
        controlPanel.SetActive(false);
        itemsPanel.SetActive(true);
        activePanel = itemsPanel;
    }

    // Escribe una linea en el log
    public void writeLogLine(string text_value) {
        logLines[nextLine].GetComponent<Text>().text = text_value;
        nextLine++;
    }

    // Borra el log
    public void cleanLog() {
        for(int i = 0; i < logLines.Length; i++) {
            logLines[i].GetComponent<Text>().text = ""; 
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
