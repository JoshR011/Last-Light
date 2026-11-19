using MyIndieGames.FantasticUI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas), typeof(GraphicRaycaster))]
public class PlayerHud : MonoBehaviour
{
    [SerializeField] internal Image backdrop;
    [SerializeField] internal FantasticInventory hotbar;
    [SerializeField] internal FantasticInventory bag;
    [SerializeField] internal FantasticWindow inventoryWindow;
    [SerializeField] internal Image selectedFrame;
    [SerializeField] internal FantasticBar healthBar;
    [SerializeField] internal TMP_Text dashCooldown;
}
