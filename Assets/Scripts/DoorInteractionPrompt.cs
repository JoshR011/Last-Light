/*
 * Authors: Joshua Antonio-Rodriguez, Jacob Krinsky, Qingzhe Song
 * Date Created: 2026-11-19
 * Date Last Updated: 2026-11-20
 * Summary: Shows the nearest available door interaction and its hold progress.
 */

using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class DoorInteractionPrompt : MonoBehaviour
{
    private VisualElement prompt;
    private VisualElement holdProgressFill;
    private Label promptLabel;

    private void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        prompt = root?.Q<VisualElement>("door-prompt");
        holdProgressFill = root?.Q<VisualElement>("door-fill");
        promptLabel = root?.Q<Label>("door-label");
        if (root != null)
        {
            root.pickingMode = PickingMode.Ignore;
        }
    }

    private void OnDisable()
    {
        if (prompt != null)
        {
            prompt.style.display = DisplayStyle.None;
        }
    }

    private void LateUpdate()
    {
        if (prompt == null || holdProgressFill == null || promptLabel == null)
        {
            return;
        }

        // Read after the door has processed this frame's E input.
        HoldDoorInteraction door = HoldDoorInteraction.GetClosestAvailable(gameObject);
        if (door == null)
        {
            prompt.style.display = DisplayStyle.None;
            return;
        }

        promptLabel.text = door.exitsInterior ? "Hold E to exit" : "Hold E to open";
        holdProgressFill.style.width = Length.Percent(door.HoldProgress * 100f);
        prompt.style.display = DisplayStyle.Flex;
    }
}
