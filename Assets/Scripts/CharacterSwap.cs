using UnityEngine;

public class CharacterSwap : MonoBehaviour
{
    public GameObject character1;      // Reference to the first character
    public GameObject character2;      // Reference to the second character
    public Camera camera1;             // Camera attached to character1
    public Camera camera2;             // Camera attached to character2

    private GameObject activeCharacter;  // The currently active character
    private Camera activeCamera;         // The currently active camera
    
    private IPlayerMovement character1Movement;  // Reference to character1's movement script
    private IPlayerMovement character2Movement;  // Reference to character2's movement script

<<<<<<< Updated upstream
    void Start()
    {
        character1Movement = character1.GetComponent<IPlayerMovement>();
        character2Movement = character2.GetComponent<IPlayerMovement>();

        character1Movement.canMove = true;
        character2Movement.canMove = false;

        // Set initial active character and camera
=======

    void Start()
    {
        if (character1 == null || character2 == null)
        {
            Debug.LogError("Character references are not assigned!");
            return;
        }

        if (camera1 == null || camera2 == null)
        {
            Debug.LogError("Camera references are not assigned!");
            return;
        }

        character1Movement = character1.GetComponent<IPlayerMovement>();
        character2Movement = character2.GetComponent<IPlayerMovement>();

        if (character1Movement == null || character2Movement == null)
        {
            Debug.LogError("IPlayerMovement component not found on one or both characters!");
            return;
        }

        character1Movement.canMove = true;
        character2Movement.canMove = false;

>>>>>>> Stashed changes
        activeCharacter = character1;
        activeCamera = camera1;

        camera1.enabled = true;
        camera2.enabled = false;
    }

<<<<<<< Updated upstream
=======

>>>>>>> Stashed changes
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SwapCharacter();
        }
    }

    void SwapCharacter()
    {
<<<<<<< Updated upstream
=======
        if (character1Movement == null || character2Movement == null)
        {
            Debug.LogError("Movement components are not assigned!");
            return;
        }
        
>>>>>>> Stashed changes
        if (character1Movement.canMove)
        {
            activeCharacter = character2;
            activeCamera = camera2;

            camera1.enabled = false;
            camera2.enabled = true;

            character1Movement.canMove = false;
            character2Movement.canMove = true;
        }
        else
        {
            activeCharacter = character1;
            activeCamera = camera1;

            camera1.enabled = true;
            camera2.enabled = false;

            character1Movement.canMove = true;
            character2Movement.canMove = false;
        }

        Debug.Log("Switched to: " + activeCharacter.name);
    }

    public GameObject GetActiveCharacter()
    {
        return activeCharacter;  // Return the currently active character
    }
}
