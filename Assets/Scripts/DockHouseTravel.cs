/*
 * Authors: Joshua Antonio-Rodriguez, Jacob Krinsky, Qingzhe Song
 * Last updated: 2026-11-06
 * Summary: Moves the player between a dock house and its interior while preserving the outdoor scene.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class DockHouseTravel : MonoBehaviour
{
    public bool IsTransitioning { get; private set; }
    public bool IsInside { get; private set; }

    private Scene exteriorScene;
    private Scene interiorScene;
    private Vector3 returnPosition;
    private Quaternion returnRotation;
    private readonly List<GameObject> suspendedRoots = new List<GameObject>();
    private Rigidbody body;
    private PlayerMovement movement;
    private PlayerDash dash;
    private PlayerInput input;
    private bool bodyWasKinematic;
    private bool movementWasEnabled;
    private bool dashWasEnabled;
    private bool inputWasEnabled;

    public void TryEnter(string scenePath, string spawnName)
    {
        if (IsTransitioning || IsInside)
        {
            return;
        }

        if (transform.parent != null || !Application.CanStreamedLevelBeLoaded(scenePath))
        {
            Debug.LogError("Door travel requires a root player and an enabled destination scene: " + scenePath, this);
            return;
        }

        if (SceneManager.GetSceneByPath(scenePath).isLoaded)
        {
            Debug.LogError("The house interior is already loaded: " + scenePath + ". Start from Map with that interior closed.", this);
            return;
        }

        StartCoroutine(Enter(scenePath, spawnName));
    }

    public void TryExit()
    {
        if (!IsTransitioning && IsInside && exteriorScene.IsValid() && exteriorScene.isLoaded)
        {
            StartCoroutine(Exit());
        }
    }

    private IEnumerator Enter(string scenePath, string spawnName)
    {
        IsTransitioning = true;
        exteriorScene = gameObject.scene;
        returnPosition = transform.position;
        returnRotation = transform.rotation;
        FreezePlayer();
        SuspendExterior();

        try
        {
            AsyncOperation load = SceneManager.LoadSceneAsync(scenePath, LoadSceneMode.Additive);
            if (load == null)
            {
                yield break;
            }
            yield return load;

            interiorScene = SceneManager.GetSceneByPath(scenePath);
            Transform spawn = null;
            foreach (GameObject root in interiorScene.GetRootGameObjects())
            {
                foreach (Transform candidate in root.GetComponentsInChildren<Transform>(true))
                {
                    if (candidate.name == spawnName)
                    {
                        spawn = candidate;
                    }
                }

                // The existing player's camera and audio listener travel with them.
                foreach (Camera camera in root.GetComponentsInChildren<Camera>(true))
                {
                    camera.enabled = false;
                }
                foreach (AudioListener listener in root.GetComponentsInChildren<AudioListener>(true))
                {
                    listener.enabled = false;
                }
            }

            if (spawn == null)
            {
                Debug.LogError("Interior is missing spawn: " + spawnName, this);
                yield return SceneManager.UnloadSceneAsync(interiorScene);
                yield break;
            }

            SceneManager.SetActiveScene(interiorScene);
            SceneManager.MoveGameObjectToScene(gameObject, interiorScene);
            Teleport(spawn.position, spawn.rotation);
            IsInside = true;
        }
        finally
        {
            if (!IsInside)
            {
                ResumeExterior();
            }
            UnfreezePlayer();
            IsTransitioning = false;
        }
    }

    private IEnumerator Exit()
    {
        IsTransitioning = true;
        FreezePlayer();

        try
        {
            SceneManager.SetActiveScene(exteriorScene);
            SceneManager.MoveGameObjectToScene(gameObject, exteriorScene);
            ResumeExterior();
            Teleport(returnPosition, returnRotation);
            IsInside = false;
            if (interiorScene.IsValid() && interiorScene.isLoaded)
            {
                yield return SceneManager.UnloadSceneAsync(interiorScene);
            }
        }
        finally
        {
            UnfreezePlayer();
            IsTransitioning = false;
        }
    }

    private void SuspendExterior()
    {
        suspendedRoots.Clear();
        foreach (GameObject root in exteriorScene.GetRootGameObjects())
        {
            // Player UI (including the existing damage overlay) stays available indoors.
            if (root == gameObject || !root.activeSelf || root.GetComponent<Canvas>() != null)
            {
                continue;
            }

            suspendedRoots.Add(root);
            root.SetActive(false);
        }
    }

    private void ResumeExterior()
    {
        foreach (GameObject root in suspendedRoots)
        {
            if (root != null)
            {
                root.SetActive(true);
            }
        }
        suspendedRoots.Clear();
    }

    private void FreezePlayer()
    {
        body = GetComponent<Rigidbody>();
        movement = GetComponent<PlayerMovement>();
        dash = GetComponent<PlayerDash>();
        input = GetComponent<PlayerInput>();
        movementWasEnabled = movement != null && movement.enabled;
        dashWasEnabled = dash != null && dash.enabled;
        inputWasEnabled = input != null && input.enabled;

        if (input != null)
        {
            input.enabled = false;
        }
        if (movement != null)
        {
            movement.enabled = false;
        }
        if (dash != null)
        {
            dash.enabled = false;
        }
        if (body != null)
        {
            bodyWasKinematic = body.isKinematic;
            if (!bodyWasKinematic)
            {
                body.velocity = Vector3.zero;
                body.angularVelocity = Vector3.zero;
            }
            body.isKinematic = true;
        }
    }

    private void Teleport(Vector3 position, Quaternion rotation)
    {
        transform.SetPositionAndRotation(position, rotation);
        if (body != null)
        {
            body.position = position;
            body.rotation = rotation;
        }
        Physics.SyncTransforms();
    }

    private void UnfreezePlayer()
    {
        if (body != null)
        {
            body.isKinematic = bodyWasKinematic;
        }
        if (movement != null)
        {
            movement.enabled = movementWasEnabled;
        }
        if (dash != null)
        {
            dash.enabled = dashWasEnabled;
        }
        if (input != null)
        {
            input.enabled = inputWasEnabled;
        }
    }
}

