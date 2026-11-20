using UnityEngine;
using UnityEngine.InputSystem;

[DisallowMultipleComponent]
public class FirstPersonWeapon : MonoBehaviour
{
    internal enum AimPose
    {
        Pistol = 1,
        LongGun = 2
    }

    private const float AimDistance = 100f;
    private const float MinimumConvergenceDistance = 0.35f;
    private const float AimAfterShotSeconds = 0.18f;
    private const float LowCarryPitch = 18f;
    private const float LowCarryYaw = -14f;
    private static readonly Vector3 PistolLowCarryOffset = new Vector3(-0.008f, -0.018f, 0.04f);
    private static readonly Vector3 LongGunLowCarryOffset = new Vector3(-0.02f, 0.02f, -0.055f);

    [SerializeField] private Transform muzzle;
    [SerializeField] private Transform grip;
    [SerializeField] private Transform supportGrip;
    [SerializeField] private AimPose aimPose = AimPose.Pistol;

    private Camera view;
    private Quaternion restRotation;
    private Vector3 restPosition;
    private int aimLayers;
    private WeaponAmmo ammo;
    private AutomaticWeapon automatic;
    private BurstWeapon burst;
    private float aimUntil;

    public Transform Grip => grip;
    internal Transform SupportGrip => supportGrip;
    internal AimPose Pose => aimPose;
    internal Vector3 HoldOffset { private get; set; }

    internal bool WantsFiringPose
    {
        get
        {
            if (InventoryManager.IsInventoryOpen) return false;
            if (Time.time < aimUntil) return true;
            if (Mouse.current == null || ammo == null || !ammo.HasRoundReady) return false;

            return automatic != null
                ? automatic.IsReadyToFire && Mouse.current.leftButton.isPressed
                : Mouse.current.leftButton.wasPressedThisFrame && (burst == null || burst.CanStartBurst);
        }
    }

    private void Awake()
    {
        view = GetComponentInParent<Camera>();
        restPosition = transform.localPosition;
        restRotation = transform.localRotation;
        aimLayers = Physics.DefaultRaycastLayers & ~LayerMask.GetMask("Player", "PlayerBullet", "Weapon");
        ammo = GetComponent<WeaponAmmo>();
        automatic = GetComponent<AutomaticWeapon>();
        burst = GetComponent<BurstWeapon>();
    }

    private void OnDisable()
    {
        aimUntil = 0f;
    }

    internal void ApplyPose(float aimWeight)
    {
        if (view == null) return;
        if (aimWeight > 0f) AlignToView();
        else ResetWeaponPose();

        float lowered = 1f - aimWeight;
        if (lowered > 0f)
        {
            Transform viewTransform = view.transform;
            Vector3 wrist = grip.position;
            Vector3 carryOffset = aimPose == AimPose.LongGun ? LongGunLowCarryOffset : PistolLowCarryOffset;
            Quaternion pitch = Quaternion.AngleAxis(LowCarryPitch * lowered, viewTransform.right);
            Quaternion yaw = Quaternion.AngleAxis(LowCarryYaw * lowered, viewTransform.up);
            transform.rotation = yaw * pitch * transform.rotation;
            transform.position += wrist - grip.position +
                viewTransform.TransformVector(carryOffset * lowered);
        }
    }

    public Ray AlignToView()
    {
        if (view == null)
        {
            return new Ray(muzzle.position, muzzle.forward);
        }

        Transform viewTransform = view.transform;
        ResetWeaponPose();
        Vector3 eye = viewTransform.position;
        Vector3 forward = viewTransform.forward;
        Vector3 barrel = muzzle.position;
        float distance = Physics.Raycast(eye, forward, out RaycastHit hit, AimDistance, aimLayers,
            QueryTriggerInteraction.Ignore) ? hit.distance : AimDistance;

        // Keep the barrel pointing forward when the crosshair is closer than the muzzle can converge.
        bool closeAim = distance < Vector3.Dot(barrel - eye, forward) + MinimumConvergenceDistance;
        Vector3 direction = closeAim ? forward : (eye + forward * distance - barrel).normalized;
        transform.rotation = Quaternion.FromToRotation(muzzle.forward, direction) * transform.rotation;
        transform.position += barrel - muzzle.position;

        // Start behind close cover if moving the camera against it put the muzzle past the surface.
        bool obstructedMuzzle = Physics.Linecast(eye, barrel, aimLayers, QueryTriggerInteraction.Ignore);
        return closeAim || obstructedMuzzle ? new Ray(eye, forward) : new Ray(barrel, direction);
    }

    public static Ray GetShot(FirstPersonWeapon weapon, Transform muzzle)
    {
        if (weapon == null) return new Ray(muzzle.position, muzzle.forward);
        weapon.aimUntil = Time.time + AimAfterShotSeconds;
        return weapon.AlignToView();
    }

    private void ResetWeaponPose()
    {
        Vector3 offset = transform.parent.InverseTransformVector(view.transform.TransformVector(HoldOffset));
        transform.SetLocalPositionAndRotation(restPosition + offset, restRotation);
    }
}
