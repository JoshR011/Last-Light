/*
 * Authors: Joshua Antonio-Rodriguez, Jacob Krinsky, Qingzhe Song
 * Summary: Connects FantasticUI weapon slots to the player's pickup and equip flow.
 */

using System.Collections.Generic;
using MyIndieGames.FantasticUI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum WeaponType
{
    Pistol,
    AssaultRifle,
    BurstSMG
}

[DefaultExecutionOrder(-10)]
public class InventoryManager : MonoBehaviour
{
    [SerializeField] private PlayerHud hudPrefab;
    [SerializeField] private PlayerHud sceneHud;
    [SerializeField] private Sprite pistolIcon;
    [SerializeField] private Sprite rifleIcon;
    [SerializeField] private Sprite smgIcon;

    private static InventoryManager instance;
    private readonly Dictionary<WeaponType, FantasticItem> weaponItems = new Dictionary<WeaponType, FantasticItem>();
    private readonly Dictionary<FantasticItem, GameObject> weaponModels = new Dictionary<FantasticItem, GameObject>();
    private PlayerHud hud;
    private GraphicRaycaster raycaster;
    private PlayerInput playerInput;
    private Life playerLife;
    private PlayerDash playerDash;
    private int displayedDashTenths;
    private GameObject equippedWeapon;
    private int selectedSlotIndex = -1;
    private bool hotbarChanged;
    private bool inventoryOpen;
    private bool restorePlayerInput;

    public static bool IsInventoryOpen => instance != null && instance.inventoryOpen;

    private void Awake()
    {
        instance = this;
        playerInput = GetComponentInParent<PlayerInput>();
        if (sceneHud == null && hudPrefab == null)
        {
            Debug.LogError("Assign a Player HUD to the Inventory Manager.", this);
            return;
        }

        foreach (Transform child in transform)
        {
            GameObject weapon = child.gameObject;
            if (weapon.TryGetComponent(out PistolWeapon _))
            {
                RegisterWeapon(WeaponType.Pistol, weapon, pistolIcon, "Pistol", "Semi-automatic sidearm");
            }
            else if (weapon.TryGetComponent(out AutomaticWeapon _))
            {
                RegisterWeapon(WeaponType.AssaultRifle, weapon, rifleIcon, "AR", "Automatic assault rifle");
            }
            else if (weapon.TryGetComponent(out BurstWeapon _))
            {
                RegisterWeapon(WeaponType.BurstSMG, weapon, smgIcon, "SMG", "Burst-fire submachine gun");
            }
        }

        InitializeHud();
        if (TryCollectWeapon(WeaponType.Pistol))
        {
            SelectInventorySlot(0);
        }
        else
        {
            Debug.LogError("The player's inventory needs a pistol child for the starting weapon.", this);
        }
    }

    private void RegisterWeapon(WeaponType type, GameObject weapon, Sprite icon, string displayName, string description)
    {
        weapon.SetActive(false);
        FantasticItem item = ScriptableObject.CreateInstance<FantasticItem>();
        item.name = displayName;
        item.DisplayName = displayName;
        item.Description = description;
        item.Icon = icon;
        item.Category = FantasticItemCategory.Weapon;
        weaponItems.Add(type, item);
        weaponModels.Add(item, weapon);
    }

    public bool TryCollectWeapon(WeaponType weaponType)
    {
        if (hud == null || !weaponItems.TryGetValue(weaponType, out FantasticItem item))
        {
            return false;
        }

        return hud.hotbar.Add(item) == 0 || hud.bag.Add(item) == 0;
    }

    private void OnSlot1() => SelectInventorySlot(0);
    private void OnSlot2() => SelectInventorySlot(1);
    private void OnSlot3() => SelectInventorySlot(2);
    private void OnSlot4() => SelectInventorySlot(3);
    private void OnSlot5() => SelectInventorySlot(4);
    private void OnSlot6() => SelectInventorySlot(5);

    public void SelectInventorySlot(int slotIndex)
    {
        if (hud == null || slotIndex < 0 || slotIndex >= hud.hotbar.Slots.Count || hud.hotbar.Slots[slotIndex].IsEmpty)
        {
            return;
        }

        selectedSlotIndex = slotIndex;
        hud.selectedFrame.transform.SetParent(hud.hotbar.Slots[slotIndex].transform, false);
        hud.selectedFrame.transform.SetAsLastSibling();
        hud.selectedFrame.enabled = true;
        UpdateEquippedWeapon();
    }

    private void LateUpdate()
    {
        UpdateDashCooldown();
        if (hud != null && playerLife != null)
        {
            float health = Mathf.Clamp(playerLife.currentHealth, 0f, Life.PlayerMaxHealth);
            if (hud.healthBar.Value != health)
            {
                hud.healthBar.SetValue(health);
            }
        }

        if (hotbarChanged && hud != null)
        {
            // FantasticSlot changes both slots during a swap. Equip once the swap is complete.
            hotbarChanged = false;
            UpdateEquippedWeapon();
        }
    }

    private void UpdateDashCooldown()
    {
        if (hud == null || playerDash == null)
        {
            return;
        }

        // Round up so the countdown stays positive until a dash is available.
        int tenths = Mathf.CeilToInt(playerDash.CooldownRemaining * 10f);
        if (tenths == displayedDashTenths)
        {
            return;
        }

        displayedDashTenths = tenths;
        hud.dashCooldown.gameObject.SetActive(tenths > 0);
        if (tenths > 0)
        {
            hud.dashCooldown.SetText("{0}.{1}", tenths / 10, tenths % 10);
        }
    }

    private void UpdateEquippedWeapon()
    {
        FantasticItem item = selectedSlotIndex >= 0 ? hud.hotbar.Slots[selectedSlotIndex].Item : null;
        GameObject weapon = null;
        if (item != null)
        {
            weaponModels.TryGetValue(item, out weapon);
        }
        if (weapon == equippedWeapon)
        {
            return;
        }

        if (equippedWeapon != null)
        {
            equippedWeapon.SetActive(false);
        }
        equippedWeapon = weapon;
        if (equippedWeapon != null)
        {
            equippedWeapon.SetActive(true);
        }
    }

    private void Update()
    {
        if (hud != null && Keyboard.current != null && Keyboard.current.tabKey.wasPressedThisFrame &&
            (inventoryOpen || playerInput == null || playerInput.inputIsActive))
        {
            hud.inventoryWindow.Toggle();
        }
    }

    private void OnInventoryVisibilityChanged(FantasticWindow window, bool open)
    {
        inventoryOpen = open;
        if (open)
        {
            restorePlayerInput = playerInput != null && playerInput.inputIsActive;
            if (restorePlayerInput)
            {
                playerInput.DeactivateInput();
            }
        }
        else
        {
            if (FantasticSlot.Dragging != null && FantasticSlot.Dragging.transform.IsChildOf(hud.transform))
            {
                FantasticSlot.Dragging.CancelDrag();
            }
            FantasticTooltip.Hide();
            if (restorePlayerInput && playerInput != null && playerInput.isActiveAndEnabled)
            {
                playerInput.ActivateInput();
            }
            restorePlayerInput = false;
        }

        hud.backdrop.gameObject.SetActive(open);
        raycaster.enabled = open;
        Cursor.lockState = open ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = open;
    }

    private void OnEnable()
    {
        if (hud != null)
        {
            hud.gameObject.SetActive(true);
        }
    }

    private void OnDisable()
    {
        if (hud != null)
        {
            if (inventoryOpen)
            {
                hud.inventoryWindow.SetOpen(false, true);
            }
            hud.gameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
        if (hud != null)
        {
            Destroy(hud.gameObject);
        }
        foreach (FantasticItem item in weaponItems.Values)
        {
            Destroy(item);
        }
    }

    private void InitializeHud()
    {
        // PlayerInput broadcasts OnMove down its hierarchy. UI buttons handle a different OnMove.
        hud = sceneHud != null ? sceneHud : Instantiate(hudPrefab);
        if (hud.transform.parent != null)
        {
            hud.transform.SetParent(null, false);
        }
        if (hud.gameObject.scene != gameObject.scene)
        {
            SceneManager.MoveGameObjectToScene(hud.gameObject, gameObject.scene);
        }

        raycaster = hud.GetComponent<GraphicRaycaster>();
        raycaster.enabled = false;
        hud.backdrop.gameObject.SetActive(false);
        hud.selectedFrame.enabled = false;
        hud.dashCooldown.gameObject.SetActive(false);
        hud.inventoryWindow.SetOpen(false, true);
        hud.inventoryWindow.VisibilityChanged += OnInventoryVisibilityChanged;

        if (FindFirstObjectByType<EventSystem>() == null)
        {
            var events = new GameObject("Inventory Event System", typeof(FantasticEventSystem));
            events.transform.SetParent(transform, false);
        }

        playerLife = GetComponentInParent<Life>();
        playerDash = GetComponentInParent<PlayerDash>();
        if (playerLife != null)
        {
            hud.healthBar.SetValues(playerLife.currentHealth, Life.PlayerMaxHealth, true);
        }

        var hotbarSlots = hud.hotbar.Slots;
        for (int index = 0; index < hotbarSlots.Count; index++)
        {
            int slotIndex = index;
            FantasticSlot slot = hotbarSlots[index];
            slot.GetComponent<Button>().onClick.AddListener(() => SelectInventorySlot(slotIndex));
            slot.OnUse.AddListener(_ => SelectInventorySlot(slotIndex));
        }
        hud.hotbar.OnChanged.AddListener(() => hotbarChanged = true);
    }
}
