using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCamera : MonoBehaviour
{

    Quaternion fixedRotation;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake() {
        fixedRotation = transform.rotation;
    }

    void LateUpdate() {
        transform.rotation = fixedRotation;
    }

    // Update is called once per frame
    void Update()
    {
                
    }
}
