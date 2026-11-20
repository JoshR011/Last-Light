using System;
using UnityEngine;
using UnityEngine.Rendering;

[DisallowMultipleComponent]
[RequireComponent(typeof(Animator))]
public sealed class FirstPersonArms : MonoBehaviour
{
    private static readonly Vector3 LeftForearm = new Vector3(0.34f, 0.32f, 0.66f).normalized;
    private static readonly Vector3 RightForearm = new Vector3(-0.46f, 0.32f, 0.66f).normalized;

    [SerializeField] private Camera view;
    [SerializeField] private SkinnedMeshRenderer worldArms;
    [SerializeField] private Mesh viewMesh;

    private SkinnedMeshRenderer viewArms;
    private InventoryManager inventory;
    private Transform leftElbow;
    private Transform rightElbow;
    private Transform leftHand;
    private Transform rightHand;
    private Transform viewLeftElbow;
    private Transform viewRightElbow;
    private bool showViewArms;
    private bool worldArmsWereHidden;
    private int gripShape;

    private void Awake()
    {
        var animator = GetComponent<Animator>();
        inventory = GetComponentInChildren<InventoryManager>(true);
        if (!animator.isHuman || view == null || worldArms == null || viewMesh == null || inventory == null)
        {
            enabled = false;
            return;
        }

        leftElbow = animator.GetBoneTransform(HumanBodyBones.LeftLowerArm);
        rightElbow = animator.GetBoneTransform(HumanBodyBones.RightLowerArm);
        leftHand = animator.GetBoneTransform(HumanBodyBones.LeftHand);
        rightHand = animator.GetBoneTransform(HumanBodyBones.RightHand);
        Transform[] bones = worldArms.bones;
        int leftIndex = Array.IndexOf(bones, leftElbow);
        int rightIndex = Array.IndexOf(bones, rightElbow);
        if (leftElbow == null || rightElbow == null || leftHand == null || rightHand == null ||
            leftIndex < 0 || rightIndex < 0)
        {
            enabled = false;
            return;
        }

        viewLeftElbow = new GameObject("View Left Forearm").transform;
        viewRightElbow = new GameObject("View Right Forearm").transform;
        viewLeftElbow.SetParent(leftElbow, false);
        viewRightElbow.SetParent(rightElbow, false);
        bones[leftIndex] = viewLeftElbow;
        bones[rightIndex] = viewRightElbow;

        var armsObject = new GameObject("First Person Arms");
        armsObject.layer = worldArms.gameObject.layer;
        armsObject.transform.SetParent(worldArms.transform, false);
        viewArms = armsObject.AddComponent<SkinnedMeshRenderer>();
        viewArms.forceRenderingOff = true;
        viewArms.sharedMesh = viewMesh;
        viewArms.sharedMaterials = worldArms.sharedMaterials;
        viewArms.rootBone = worldArms.rootBone;
        viewArms.bones = bones;
        viewArms.shadowCastingMode = ShadowCastingMode.Off;
        viewArms.updateWhenOffscreen = true;
        gripShape = viewMesh.GetBlendShapeIndex("Weapon Grip");
        worldArmsWereHidden = worldArms.forceRenderingOff;
    }

    private void OnEnable()
    {
        RenderPipelineManager.beginCameraRendering += ShowForCamera;
        RenderPipelineManager.endCameraRendering += RestoreWorldArms;
    }

    private void OnDisable()
    {
        RenderPipelineManager.beginCameraRendering -= ShowForCamera;
        RenderPipelineManager.endCameraRendering -= RestoreWorldArms;
        if (viewArms != null)
        {
            worldArms.forceRenderingOff = worldArmsWereHidden;
            viewArms.forceRenderingOff = true;
        }
    }

    private void LateUpdate()
    {
        showViewArms = inventory.EquippedFirstPersonWeapon != null;
        if (!showViewArms) return;

        PositionForearm(viewLeftElbow, leftElbow, leftHand, LeftForearm);
        PositionForearm(viewRightElbow, rightElbow, rightHand, RightForearm);
        if (gripShape >= 0) viewArms.SetBlendShapeWeight(gripShape, worldArms.GetBlendShapeWeight(gripShape));
    }

    private void PositionForearm(Transform viewElbow, Transform elbow, Transform hand, Vector3 viewDirection)
    {
        // Keep the hand on the grip while the sleeve approaches it from below the camera.
        Vector3 forearm = hand.position - elbow.position;
        Vector3 direction = view.transform.TransformDirection(viewDirection);
        var rotation = Quaternion.FromToRotation(forearm, direction) * elbow.rotation;
        viewElbow.SetPositionAndRotation(hand.position - direction * forearm.magnitude, rotation);
    }

    private void ShowForCamera(ScriptableRenderContext context, Camera camera)
    {
        bool useViewArms = showViewArms && camera == view;
        worldArms.forceRenderingOff = worldArmsWereHidden || useViewArms;
        viewArms.forceRenderingOff = !useViewArms;
    }

    private void RestoreWorldArms(ScriptableRenderContext context, Camera camera)
    {
        if (camera != view) return;
        worldArms.forceRenderingOff = worldArmsWereHidden;
        viewArms.forceRenderingOff = true;
    }
}
