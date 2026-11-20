/*
 * Authors: Joshua Antonio-Rodriguez, Jacob Krinsky, Qingzhe Song
 * Date Created: 2026-11-19
 * Date Last Updated: 2026-11-20
 * Summary: Updates the HUD compass, flashlight charge meter, and firing crosshair.
 */

using MyIndieGames.FantasticUI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas), typeof(GraphicRaycaster))]
public class PlayerHud : MonoBehaviour
{
    [Header("Inventory and Hotbar")]
    [SerializeField] internal Image backdrop;
    [SerializeField] internal FantasticInventory hotbar;
    [SerializeField] internal FantasticInventory bag;
    [SerializeField] internal FantasticWindow inventoryWindow;
    [SerializeField] internal Image selectedFrame;

    [Header("Health and Dash")]
    [SerializeField] internal FantasticBar healthBar;
    [SerializeField] internal TMP_Text dashCooldown;

    [Header("Compass")]
    [SerializeField] private RectTransform compassDial;

    [Header("Flashlight Charge")]
    [SerializeField] private GameObject flashlightMeter;
    [SerializeField] private RectTransform flashlightChargeFill;
    [SerializeField] private TMP_Text flashlightChargePercent;

    [Header("Firing Crosshair")]
    [SerializeField] private CanvasGroup crosshair;
    [SerializeField] private RectTransform crosshairTop;
    [SerializeField] private RectTransform crosshairBottom;
    [SerializeField] private RectTransform crosshairLeft;
    [SerializeField] private RectTransform crosshairRight;
    [SerializeField] private float crosshairGap = 10f;
    [SerializeField] private float spreadPerShot = 6f;
    [SerializeField] private float maxSpread = 24f;
    [SerializeField] private float spreadRecoverySpeed = 60f;

    private float crosshairSpread;

    private Transform compassHeading;
    private FlashlightController flashlight;
    private int displayedChargePercent = -1;

    internal void BindCompass(Transform heading)
    {
        compassHeading = heading;
        UpdateCompass();
    }

    internal void BindFlashlight(FlashlightController controller)
    {
        flashlight = controller;
        displayedChargePercent = -1;
        if (flashlightMeter != null) flashlightMeter.SetActive(flashlight != null);
        UpdateFlashlightCharge();
    }

    private void OnEnable()
    {
        WeaponAmmo.Fired += OnWeaponFired;
    }

    private void OnDisable()
    {
        // The static event can outlive this HUD when the player dies or changes scenes.
        WeaponAmmo.Fired -= OnWeaponFired;
    }

    private void OnWeaponFired()
    {
        crosshairSpread = Mathf.Min(crosshairSpread + spreadPerShot, maxSpread);
    }

    private void LateUpdate()
    {
        UpdateCompass();
        UpdateFlashlightCharge();
        UpdateCrosshair();
    }

    private void UpdateFlashlightCharge()
    {
        if (flashlight == null || flashlightChargeFill == null || flashlightChargePercent == null) return;

        // Round up so the meter reaches 0% only when the battery is depleted.
        int percent = Mathf.CeilToInt(flashlight.Charge * 100f);
        if (percent == displayedChargePercent) return;

        displayedChargePercent = percent;
        flashlightChargeFill.anchorMax = new Vector2(percent / 100f, 1f);
        flashlightChargePercent.SetText("{0:0}%", percent);
    }

    private void UpdateCompass()
    {
        if (compassDial == null || compassHeading == null)
        {
            return;
        }

        // World +Z is north. Turning east rotates the E on the dial up to the fixed marker.
        Vector3 forward = compassHeading.forward;
        float heading = Mathf.Atan2(forward.x, forward.z) * Mathf.Rad2Deg;
        compassDial.localRotation = Quaternion.Euler(0f, 0f, heading);
    }

    private void UpdateCrosshair()
    {
        if (crosshair == null)
        {
            return;
        }

        crosshair.alpha = InventoryManager.IsInventoryOpen ? 0f : 1f;

        if (crosshairTop == null || crosshairBottom == null || crosshairLeft == null || crosshairRight == null)
        {
            return;
        }

        crosshairSpread = Mathf.MoveTowards(crosshairSpread, 0f, spreadRecoverySpeed * Time.deltaTime);

        float distance = crosshairGap + crosshairSpread;
        crosshairTop.anchoredPosition = new Vector2(0f, distance);
        crosshairBottom.anchoredPosition = new Vector2(0f, -distance);
        crosshairLeft.anchoredPosition = new Vector2(-distance, 0f);
        crosshairRight.anchoredPosition = new Vector2(distance, 0f);
    }

}
