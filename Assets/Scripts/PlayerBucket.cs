using UnityEngine;
using System.Collections;

public class PlayerBucket : MonoBehaviour
{
    public Transform compostBin1; // Drop-off point
    public Transform compostBin2;
    public int wasteCollected = 0; // Waste collected by this character
    private GameObject currentWaste = null; // Holds the waste being carried
    private GameObject nearbyWaste = null; // Tracks waste near the player
    private bool isNearTrash = false;
    public Camera playerCamera; // Reference to the player’s camera
    private GameObject successCanvas; // Reference to success canvas (shared between players)

    private static int totalWasteCollected = 0;
    private const int totalWasteGoal = 9;

    public static event System.Action<PlayerBucket> OnPlayerBucketCreated;

    private CharacterSwap characterSwap;
    private bool isActiveCharacter = false;

    void Awake()
    {
        OnPlayerBucketCreated?.Invoke(this);
    }

    void Start()
    {
        characterSwap = FindObjectOfType<CharacterSwap>();
        if (characterSwap == null)
        {
            Debug.LogError("CharacterSwap script not found in the scene.");
        }

        compostBin1 = GameObject.Find("CompostBin1Location")?.transform;
        compostBin2 = GameObject.Find("CompostBin2Location")?.transform;

        if (compostBin1 == null || compostBin2 == null)
        {
            Debug.LogError("Compost bins not found in the scene. Please ensure the objects are named correctly.");
        }

        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }

        successCanvas = GameObject.Find("SuccessCanvas");
        if (successCanvas != null)
        {
            successCanvas.SetActive(false);
        }
        else
        {
            Debug.LogError("SuccessCanvas not found in the scene.");
        }
    }

    void Update()
    {
        // Check if this is the active character
        if (characterSwap != null)
        {
            isActiveCharacter = characterSwap.GetActiveCharacter() == gameObject;
        }

        if (!isActiveCharacter)
        {
            // Clear variables for inactive character
            nearbyWaste = null;
            return;
        }

        if (totalWasteCollected >= totalWasteGoal)
        {
            if (successCanvas != null && !successCanvas.activeSelf)
            {
                successCanvas.SetActive(true);
                StartCoroutine(HideSuccessMessageAfterTime());
            }
            return;
        }

        // Pick up waste
        if (nearbyWaste != null && Input.GetKeyDown(KeyCode.E) && currentWaste == null)
        {
            PickUpWaste(nearbyWaste);
        }

        // Drop waste
        if (isNearTrash && currentWaste != null && Input.GetKeyDown(KeyCode.E))
        {
            DropWaste();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isActiveCharacter) return; // Ignore triggers for inactive characters

        if (other.CompareTag("Waste"))
        {
            nearbyWaste = other.gameObject; // Track the nearby waste
        }
        if (other.CompareTag("TrashBin"))
        {
            isNearTrash = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!isActiveCharacter) return; // Ignore triggers for inactive characters

        if (other.CompareTag("Waste") && nearbyWaste == other.gameObject)
        {
            nearbyWaste = null; // Clear the reference to waste
        }
        if (other.CompareTag("TrashBin"))
        {
            isNearTrash = false;
        }
    }

    void PickUpWaste(GameObject waste)
    {
        currentWaste = waste; // Assign the waste being picked up
        currentWaste.transform.SetParent(transform);
        currentWaste.GetComponent<Rigidbody>().isKinematic = true;

        Vector3 forwardPosition = playerCamera.transform.position + playerCamera.transform.forward * 1.0f + Vector3.up * 0.5f;
        currentWaste.transform.position = forwardPosition;
        currentWaste.transform.rotation = Quaternion.LookRotation(playerCamera.transform.forward);

        Debug.Log($"Picked up waste: {currentWaste.name}");
    }

    void DropWaste()
    {
        currentWaste.SetActive(false);
        currentWaste.transform.SetParent(null);
        currentWaste = null; // Clear the reference after dropping

        wasteCollected++;
        totalWasteCollected++;
        Debug.Log($"Waste dropped! Total Waste Collected: {totalWasteCollected}");
    }

    IEnumerator HideSuccessMessageAfterTime()
    {
        yield return new WaitForSeconds(5f);
        if (successCanvas != null)
        {
            successCanvas.SetActive(false);
        }
    }
}
