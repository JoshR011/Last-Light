/*
 * Authors: Joshua Antonio-Rodriguez, Jacob Krinsky, Qingzhe Song
 * Last updated: 2026-11-06
 * Summary: Lets the player enter or exit a house by holding E near its door.
 */

using UnityEngine;
using UnityEngine.InputSystem;

public class HoldDoorInteraction : MonoBehaviour
{
    [Header("Door Interaction")]
    [Min(0.1f)]
    public float interactionRadius = 2f;
    [Min(0.1f)]
    public float holdSeconds = 2f;
    [Tooltip("Local position of the interaction point on this door's mesh.")]
    public Vector3 interactionOffset = new Vector3(0f, 1.2f, 0f);

    [Header("Scene Destination")]
    public bool exitsInterior;
    public string interiorScenePath = "Assets/Scenes/HouseOne.unity";
    public string entranceSpawnName = "EntranceSpawn";

    private GameObject player;
    private float heldSeconds;
    private bool needsRelease;

    private void OnEnable()
    {
        heldSeconds = 0f;
        // Carrying E through a load must not immediately start the return trip.
        needsRelease = Keyboard.current != null && Keyboard.current.eKey.isPressed;
    }

    private void OnDisable()
    {
        heldSeconds = 0f;
    }

    private void Update()
    {
        Keyboard keyboard = Keyboard.current;
        if (keyboard == null || !keyboard.eKey.isPressed)
        {
            heldSeconds = 0f;
            needsRelease = false;
            return;
        }

        if (needsRelease || Time.timeScale <= 0f || !Application.isFocused)
        {
            heldSeconds = 0f;
            return;
        }

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        if (player == null)
        {
            heldSeconds = 0f;
            return;
        }

        Life life = player.GetComponent<Life>();
        DockHouseTravel travel = player.GetComponent<DockHouseTravel>();
        // Measure from torso height because this player's root is at their feet.
        Vector3 playerPoint = player.transform.position + Vector3.up * 1.2f;
        Vector3 doorPoint = transform.TransformPoint(interactionOffset);
        if ((life != null && life.currentHealth <= 0f)
            || (travel != null && travel.IsTransitioning)
            || Vector3.Distance(playerPoint, doorPoint) > interactionRadius)
        {
            heldSeconds = 0f;
            return;
        }

        heldSeconds += Time.deltaTime;
        if (heldSeconds < holdSeconds)
        {
            return;
        }

        heldSeconds = 0f;
        needsRelease = true;
        if (travel == null)
        {
            travel = player.AddComponent<DockHouseTravel>();
        }
        if (exitsInterior)
        {
            travel.TryExit();
        }
        else
        {
            travel.TryEnter(interiorScenePath, entranceSpawnName);
        }
    }
}
