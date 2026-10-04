using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour
{
    public GameObject hintLine;

    void Awake()
    {
        var life = GetComponent<Life>();
        life.onBreak.AddListener(GenerateHint);
    }

    void GenerateHint()
    {
        if (CrateManager.instance.realCrate == this) {
            print("You found the right crate!");
            Destroy(gameObject);
            return;
        };

        Vector3 start = GetComponent<Collider>().bounds.center;
        Vector3 target = CrateManager.instance.realCrate.GetComponent<Collider>().bounds.center;
        Vector3 direction = target - start;

        GameObject line = Instantiate(hintLine, start, Quaternion.identity);
        line.transform.rotation = Quaternion.LookRotation(direction, line.transform.up);
        
        Destroy(gameObject);
        Destroy(line, 4);
    }
}
