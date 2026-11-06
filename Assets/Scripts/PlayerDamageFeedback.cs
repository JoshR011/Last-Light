/*
 * Authors: Joshua Antonio-Rodriguez, Jacob Krinsky, Qingzhe Song
 * Last updated: 2026-11-06
 * Summary: Flashes soft red screen edges on damage or green on healing, then fades them away.
 */

using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Life))]
public class PlayerDamageFeedback : MonoBehaviour
{
    [Header("Damage Flash Color")]
    public Color flashColor = new Color(0.9f, 0.02f, 0.02f, 1f);
    [Header("Healing Flash Color")]
    public Color healFlashColor = new Color(0.02f, 0.9f, 0.02f, 1f);

    [Header("Shared Flash Strength")]
    [Range(0f, 1f)]
    public float flashOpacity = 0.8f;

    [Header("Damage Flash Timing")]
    [Min(0.01f)]
    public float fadeDuration = 0.7f;

    [Header("Screen Edges")]
    [Tooltip("Fraction of each screen dimension covered by the soft border; the center stays clear.")]
    [Range(0.05f, 0.45f)]
    public float edgeWidth = 0.22f;

    private Life playerLife;
    private RawImage overlay;
    private Texture2D edgeTexture;
    private float flashTimeRemaining;
    private Color activeFlashColor;

    private void Awake()
    {
        playerLife = GetComponent<Life>();
        CreateOverlay();
    }

    private void OnEnable()
    {
        // Subscribe only while active so disabling this feedback cannot leave stale hit listeners.
        playerLife.Damaged += ShowDamageFlash;
    }

    private void CreateOverlay()
    {
        // A display-sized overlay keeps the flash independent of fog, world lighting, and camera movement.
        GameObject screen = new GameObject("Player Damage Flash", typeof(RectTransform), typeof(Canvas), typeof(RawImage));
        screen.layer = LayerMask.NameToLayer("UI");
        Canvas canvas = screen.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 100;
        overlay = screen.GetComponent<RawImage>();
        // This is visual feedback only; it must never intercept mouse clicks or other UI interaction.
        overlay.raycastTarget = false;

        const int textureSize = 256;
        edgeTexture = new Texture2D(textureSize, textureSize, TextureFormat.RGBA32, false);
        edgeTexture.name = "Soft Damage Edges";
        edgeTexture.wrapMode = TextureWrapMode.Clamp;
        edgeTexture.filterMode = FilterMode.Bilinear;
        Color32[] pixels = new Color32[textureSize * textureSize];
        float width = Mathf.Clamp(edgeWidth, 0.05f, 0.45f);
        for (int y = 0; y < textureSize; y++)
        {
            for (int x = 0; x < textureSize; x++)
            {
                float u = (float)x / (textureSize - 1);
                float v = (float)y / (textureSize - 1);
                float distanceToEdge = Mathf.Min(Mathf.Min(u, 1f - u), Mathf.Min(v, 1f - v));
                // Smooth alpha creates a feathered red border with a fully transparent central view.
                float alpha = 1f - Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(distanceToEdge / width));
                pixels[y * textureSize + x] = new Color32(255, 255, 255, (byte)Mathf.RoundToInt(alpha * 255f));
            }
        }
        edgeTexture.SetPixels32(pixels);
        edgeTexture.Apply(false, true);
        overlay.texture = edgeTexture;
        overlay.enabled = false;
    }

    private void ShowDamageFlash()
    {
        ShowFlash(flashColor);
    }

    public void ShowHealFlash()
    {
        if (!isActiveAndEnabled) return;
        ShowFlash(healFlashColor);
    }

    private void ShowFlash(Color color)
    {
        // A fresh hit restarts the flash instead of stacking overlays or allowing an old fade to hide it.
        activeFlashColor = color;
        flashTimeRemaining = Mathf.Max(0.01f, fadeDuration);
        overlay.color = new Color(activeFlashColor.r, activeFlashColor.g, activeFlashColor.b, activeFlashColor.a * flashOpacity);
        overlay.enabled = true;
    }

    private void Update()
    {
        if (flashTimeRemaining <= 0f) return;
        // Use real elapsed time so pausing or slowing gameplay cannot leave a red overlay stuck on screen.
        flashTimeRemaining = Mathf.Max(0f, flashTimeRemaining - Time.unscaledDeltaTime);
        float fade = Mathf.SmoothStep(0f, 1f, flashTimeRemaining / Mathf.Max(0.01f, fadeDuration));
        overlay.color = new Color(activeFlashColor.r, activeFlashColor.g, activeFlashColor.b, activeFlashColor.a * flashOpacity * fade);
        overlay.enabled = flashTimeRemaining > 0f;
    }

    private void OnDisable()
    {
        playerLife.Damaged -= ShowDamageFlash;
        flashTimeRemaining = 0f;
        if (overlay != null) overlay.enabled = false;
    }

    private void OnDestroy()
    {
        // The canvas lives at screen level, so remove it explicitly when the player dies or the scene unloads.
        if (overlay != null) Destroy(overlay.gameObject);
        if (edgeTexture != null) Destroy(edgeTexture);
    }
}
