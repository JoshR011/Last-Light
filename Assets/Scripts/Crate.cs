using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Reveals the winning crate or creates a temporary hint pointing toward it when broken.
public class Crate : MonoBehaviour
{
    // Prefab used to display the directional hint from an incorrect crate.
    public GameObject hintLine;

    void Awake()
    {
        // Generate a hint when this crate's Life component raises its break event.
        var life = GetComponent<Life>();
        life.onBreak.AddListener(GenerateHint);
    }

    void GenerateHint()
    {
        // Breaking the selected crate completes the search and skips creating a hint.
        if (CrateManager.instance.realCrate == this) {
            print("You found the right crate!");
            Destroy(gameObject);
            return;
        };

        // Calculate a direction between the centers of this crate and the winning crate.
        Vector3 start = GetComponent<Collider>().bounds.center;
        Vector3 target = CrateManager.instance.realCrate.GetComponent<Collider>().bounds.center;
        Vector3 direction = target - start;

        // Place the hint at this crate and rotate its forward direction toward the target.
        GameObject line = Instantiate(hintLine, start, Quaternion.identity);
        line.transform.rotation = Quaternion.LookRotation(direction, line.transform.up);
        
        // Remove the broken crate immediately and let its hint remain for four seconds.
        Destroy(gameObject);
        Destroy(line, 4);
    }
}
