using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyCatalog")]
public class EnemyCatalog : ScriptableObject
{
    [SerializeField]
    private List<Enemy> enemies;

    public Enemy getEnemyByName(string name) {
        Enemy value = enemies.Find(enemy => enemy.getName().ToUpper() == name.ToUpper());
        return value;
    }

    public Enemy getEnemyById(int id) {
        Enemy value = enemies.Find(enemy => enemy.getId() == id);
        return value;
    }

    public Enemy getRandomEnemy() {
        Enemy value = enemies[0];
        bool found = false;
        while(!found) {
            int i = UnityEngine.Random.Range(0, enemies.Count);
            if(!enemies[i].isEnemyBoss()) {
                found = true;
                value = enemies[i];
            }
        }
        return value;
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
