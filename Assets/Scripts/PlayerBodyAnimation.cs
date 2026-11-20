/*
 * Authors: Joshua Antonio-Rodriguez, Jacob Krinsky, Qingzhe Song
 * Date Created: 2026-11-20
 * Date Last Updated: 2026-11-20
 * Summary: Drives locomotion, crouching, and weapon pose animation for the player model.
 */

using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Animator), typeof(Rigidbody), typeof(PlayerCrouchSlide))]
public class PlayerBodyAnimation : MonoBehaviour
{
    private static readonly int MoveX = Animator.StringToHash("MoveX");
    private static readonly int MoveZ = Animator.StringToHash("MoveZ");
    private static readonly int WeaponPose = Animator.StringToHash("WeaponPose");
    private static readonly int WeaponAim = Animator.StringToHash("WeaponAim");
    private const float MoveDamping = 0.1f;
    private const float CrouchBlendSpeed = 8f;
    private const float WalkBlendSpeed = 2f;
    private const float AimBlendSpeed = 6f;
    private const float ElbowGuideWeight = 0.5f;

    [Header("Animation References")]
    [SerializeField] private InventoryManager inventory;
    [SerializeField] private Transform view;
    [SerializeField, FormerlySerializedAs("shoulderFlashlight")] private FlashlightController neckFlashlight;
    [SerializeField, FormerlySerializedAs("bodyMesh")] private SkinnedMeshRenderer armsMesh;

    [Header("Crouch Pose")]
    [SerializeField, Min(0f)] private float crouchDrop = 1.05f;

    private Animator animator;
    private Rigidbody body;
    private PlayerCrouchSlide stance;
    private float crouchWeight;
    private float aimWeight;
    private int gripShapeIndex = -1;
    private Vector3 standingFlashlightPosition;

    internal FlashlightController MountedFlashlight => neckFlashlight;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody>();
        stance = GetComponent<PlayerCrouchSlide>();
        if (armsMesh != null) gripShapeIndex = armsMesh.sharedMesh.GetBlendShapeIndex("Weapon Grip");
        if (neckFlashlight != null)
        {
            standingFlashlightPosition = neckFlashlight.transform.localPosition;
        }
    }

    private void Update()
    {
        Vector3 velocity = transform.InverseTransformDirection(body.velocity);
        Vector2 movement = stance.IsSliding ? Vector2.zero : new Vector2(velocity.x, velocity.z);
        Vector2 stride = Vector2.ClampMagnitude(movement, WalkBlendSpeed);

        animator.SetFloat(MoveX, stride.x, MoveDamping, Time.deltaTime);
        animator.SetFloat(MoveZ, stride.y, MoveDamping, Time.deltaTime);
        FirstPersonWeapon weapon = inventory.EquippedFirstPersonWeapon;
        animator.SetInteger(WeaponPose, weapon != null ? (int)weapon.Pose : 0);
        bool firing = weapon != null && weapon.WantsFiringPose;
        aimWeight = firing ? 1f : Mathf.MoveTowards(aimWeight, 0f, AimBlendSpeed * Time.deltaTime);
        animator.SetFloat(WeaponAim, aimWeight);
        if (gripShapeIndex >= 0) armsMesh.SetBlendShapeWeight(gripShapeIndex, weapon != null ? 100f : 0f);
        crouchWeight = Mathf.MoveTowards(crouchWeight, stance.IsCrouching ? 1f : 0f, CrouchBlendSpeed * Time.deltaTime);
    }

    private void OnAnimatorIK(int layerIndex)
    {
        // Lower the hips while the feet keep the placement and lift from the locomotion clips.
        Vector3 leftFoot = animator.GetIKPosition(AvatarIKGoal.LeftFoot);
        Vector3 rightFoot = animator.GetIKPosition(AvatarIKGoal.RightFoot);
        Quaternion leftFootRotation = animator.GetIKRotation(AvatarIKGoal.LeftFoot);
        Quaternion rightFootRotation = animator.GetIKRotation(AvatarIKGoal.RightFoot);
        animator.bodyPosition -= transform.up * (crouchDrop * crouchWeight);
        animator.SetIKPosition(AvatarIKGoal.LeftFoot, leftFoot);
        animator.SetIKPosition(AvatarIKGoal.RightFoot, rightFoot);
        animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, crouchWeight);
        animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, crouchWeight);
        animator.SetIKRotation(AvatarIKGoal.LeftFoot, leftFootRotation);
        animator.SetIKRotation(AvatarIKGoal.RightFoot, rightFootRotation);
        animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, crouchWeight);
        animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, crouchWeight);

        if (neckFlashlight != null)
        {
            // Follow the stance height from the player root, away from the animated clavicle.
            neckFlashlight.transform.localPosition = standingFlashlightPosition - Vector3.up * (crouchDrop * crouchWeight);
        }

        // Bring the weapon closer in crouch and at steep angles so both wrists can stay on its grips.
        float lookUp = Mathf.InverseLerp(0.25f, 1f, view.forward.y);
        float lookDown = Mathf.InverseLerp(0.45f, 1f, -view.forward.y);
        Vector3 offset = new Vector3(-0.035f * crouchWeight - 0.012f * lookUp,
            0.10f * crouchWeight + 0.14f * lookUp - 0.01f * lookDown,
            -0.17f * crouchWeight - 0.13f * lookUp + 0.08f * lookDown);
        FirstPersonWeapon weapon = inventory.EquippedFirstPersonWeapon;
        if (weapon != null)
        {
            weapon.HoldOffset = offset;
            weapon.ApplyPose(aimWeight);
        }
        PlaceHand(AvatarIKGoal.RightHand, AvatarIKHint.RightElbow, weapon != null ? weapon.Grip : null);
        PlaceHand(AvatarIKGoal.LeftHand, AvatarIKHint.LeftElbow, weapon != null ? weapon.SupportGrip : null);
    }

    private void PlaceHand(AvatarIKGoal goal, AvatarIKHint elbow, Transform grip)
    {
        float weight = grip != null ? 1f : 0f;
        animator.SetIKPositionWeight(goal, weight);
        animator.SetIKRotationWeight(goal, weight);
        animator.SetIKHintPositionWeight(elbow, ElbowGuideWeight * weight);
        if (grip != null)
        {
            animator.SetIKPosition(goal, grip.position);
            animator.SetIKRotation(goal, grip.rotation);

            // Keep a slight outward bend without pulling the elbows across the chest.
            bool right = goal == AvatarIKGoal.RightHand;
            Vector3 elbowOffset = transform.right * (right ? 0.16f : -0.14f)
                - transform.up * (right ? 0.105f : 0.18f) - transform.forward * (right ? 0.40f : 0.33f);
            animator.SetIKHintPosition(elbow, grip.position + elbowOffset);
        }
    }
}
