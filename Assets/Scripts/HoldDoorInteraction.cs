/*
 * Authors: Joshua Antonio-Rodriguez, Jacob Krinsky, Qingzhe Song
 * Date Created: 2026-11-06
 * Date Last Updated: 2026-11-20
 * Summary: Lets the player enter or exit a house by holding E near its door.
 */

using System.Collections.Generic;
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

    // The prompt and hold logic share this list so they pick the same door when ranges overlap.
    private static readonly List<HoldDoorInteraction> activeDoors = new List<HoldDoorInteraction>();
    private GameObject player;
    private float heldSeconds;
    private bool needsRelease;

    internal float HoldProgress => Mathf.Clamp01(heldSeconds / Mathf.Max(0.01f, holdSeconds));

    private void OnEnable()
    {
        activeDoors.Add(this);
        heldSeconds = 0f;
        // Carrying E through a load must not immediately start the return trip.
        needsRelease = Keyboard.current != null && Keyboard.current.eKey.isPressed;
    }

    private void OnDisable()
    {
        activeDoors.Remove(this);
        heldSeconds = 0f;
    }

    internal static HoldDoorInteraction GetClosestAvailable(GameObject player)
    {
        if (player == null || Time.timeScale <= 0f || !Application.isFocused ||
            InventoryManager.IsInventoryOpen)
        {
            return null;
        }

        Life life = player.GetComponent<Life>();
        DockHouseTravel travel = player.GetComponent<DockHouseTravel>();
        if ((life != null && life.currentHealth <= 0f) ||
            (travel != null && travel.IsTransitioning))
        {
            return null;
        }

        bool isInside = travel != null && travel.IsInside;
        // Measure from torso height because this player's root is at their feet.
        Vector3 playerPoint = player.transform.position + Vector3.up * 1.2f;
        HoldDoorInteraction closest = null;
        float closestDistance = float.PositiveInfinity;

        foreach (HoldDoorInteraction door in activeDoors)
        {
            if (door.exitsInterior != isInside)
            {
                continue;
            }

            Vector3 doorPoint = door.transform.TransformPoint(door.interactionOffset);
            float distance = Vector3.Distance(playerPoint, doorPoint);
            if (distance <= door.interactionRadius && distance < closestDistance)
            {
                closest = door;
                closestDistance = distance;
            }
        }

        return closest;
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

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        // Restart the hold if the player moves away or another door becomes the closer target.
        if (needsRelease || GetClosestAvailable(player) != this)
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
        DockHouseTravel travel = player.GetComponent<DockHouseTravel>();
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
