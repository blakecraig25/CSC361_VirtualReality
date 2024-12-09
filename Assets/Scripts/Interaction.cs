using UnityEngine;

public class Interaction : MonoBehaviour
{
    public GameObject promptCanvas; // Interaction prompt canvas
    public GameObject infoCanvas;  // Information canvas
    private bool isActiveCharacterNearby = false; // Is the active character in range?
    private bool isOtherCharacterNearby = false;  // Is the inactive character in range?

    private CharacterSwap characterSwap;
    private GameObject activeCharacter;

    void Start()
    {
        // Hide both canvases initially
        promptCanvas.SetActive(false);
        infoCanvas.SetActive(false);

        // Find the CharacterSwap script
        characterSwap = FindObjectOfType<CharacterSwap>();
        if (characterSwap == null)
        {
            Debug.LogError("CharacterSwap script not found in the scene.");
        }

        // Initialize active character
        if (characterSwap != null)
        {
            activeCharacter = characterSwap.GetActiveCharacter();
        }
    }

    void Update()
    {
        // Dynamically update the active character
        if (characterSwap != null)
        {
            activeCharacter = characterSwap.GetActiveCharacter();
        }

        // Handle interaction display logic
        if (!isActiveCharacterNearby && !isOtherCharacterNearby)
        {
            HidePromptAndInfo(); // Hide everything if no one is nearby
        }

        // Allow interaction with the active character
        if (isActiveCharacterNearby && Input.GetKeyDown(KeyCode.E))
        {
            ToggleInfoCanvas();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Check if the active character is entering
            if (other.gameObject == activeCharacter)
            {
                isActiveCharacterNearby = true;
                TogglePromptCanvas();
            }
            else
            {
                // Handle the inactive character
                isOtherCharacterNearby = true;
                TogglePromptCanvas();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Check if the active character is exiting
            if (other.gameObject == activeCharacter)
            {
                isActiveCharacterNearby = false;
            }
            else
            {
                // Handle the inactive character exiting
                isOtherCharacterNearby = false;
            }

            // Hide the prompt if neither character is nearby
            if (!isActiveCharacterNearby && !isOtherCharacterNearby)
            {
                HidePromptAndInfo();
            }
        }
    }

    private void TogglePromptCanvas()
    {
        // Show the prompt canvas
        promptCanvas.SetActive(true);
    }

    private void ToggleInfoCanvas()
    {
        // Show the info canvas and hide the prompt
        promptCanvas.SetActive(false);
        infoCanvas.SetActive(true);
    }

    private void HidePromptAndInfo()
    {
        // Hide both canvases
        promptCanvas.SetActive(false);
        infoCanvas.SetActive(false);
    }
}
