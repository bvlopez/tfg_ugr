using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemCatalog")]
public class ItemCatalog : ScriptableObject
{

    [SerializeField]
    private List<Item> items;

    public Item getItemByName(string name) {
        Item value = items.Find(item => item.getName().ToUpper() == name.ToUpper());
        return value;
    }

    public Item getItemById(int id) {
        Item value = items.Find(item => item.getId() == id);
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
