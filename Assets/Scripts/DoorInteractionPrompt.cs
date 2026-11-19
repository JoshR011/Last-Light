using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class DoorInteractionPrompt : MonoBehaviour
{
    private VisualElement prompt;
    private VisualElement fill;
    private Label label;

    private void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        prompt = root?.Q<VisualElement>("door-prompt");
        fill = root?.Q<VisualElement>("door-fill");
        label = root?.Q<Label>("door-label");
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
        if (prompt == null || fill == null || label == null)
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

        label.text = door.exitsInterior ? "Hold E to exit" : "Hold E to open";
        fill.style.width = Length.Percent(door.HoldProgress * 100f);
        prompt.style.display = DisplayStyle.Flex;
    }
}
