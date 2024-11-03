using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PacStudentController : MonoBehaviour
{
    public float moveSpeed = 5f; 
    public AudioClip pelletAudio;
    public AudioClip movingAudio;
    public ParticleSystem dustEffect;
    public Tilemap wallMap; 
    public Tilemap pelletMap; 
    public Tilemap floorMap; 
    public Vector3 cellOffset = new Vector3(0.5f, 0.5f, 0);

    private Vector2Int currentGridPosition;  // Current grid position
    private Vector2Int targetGridPosition;   // Next target position to Lerp to
    private Vector2Int lastInput = Vector2Int.zero;  // Stores the last input direction
    private Vector2Int currentInput = Vector2Int.zero; // Stores current movement direction

    private bool isMoving = false; // Track if PacStudent is currently moving
    private Animator anim;
    private AudioSource audioSource;

    void Start()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        // Initialize PacStudent's position to align with the grid
        Vector3Int startPosition = floorMap.WorldToCell(transform.position);
        currentGridPosition = new Vector2Int(startPosition.x, startPosition.y);
        targetGridPosition = currentGridPosition;

        // Center PacStudent's initial position in the cell
        transform.position = floorMap.CellToWorld(startPosition) + cellOffset;
    }

    void Update()
    {
        HandleInput();

        if (!isMoving)
        {
            TryMove(lastInput); // Try moving with the last input

            if (!isMoving)
            {
                TryMove(currentInput); // If blocked, fallback to current direction
            }
        }

        // Update the Animator parameters for direction
        UpdateAnimator();
    }

    void HandleInput()
    {
        // Capture input for movement
        if (Input.GetKeyDown(KeyCode.W)) lastInput = Vector2Int.up;
        if (Input.GetKeyDown(KeyCode.S)) lastInput = Vector2Int.down;
        if (Input.GetKeyDown(KeyCode.A)) lastInput = Vector2Int.left;
        if (Input.GetKeyDown(KeyCode.D)) lastInput = Vector2Int.right;
    }

    void TryMove(Vector2Int direction)
    {
        Vector2Int nextPosition = currentGridPosition + direction;
        Vector3Int tilePosition = new Vector3Int(nextPosition.x, nextPosition.y, 0);

        // Check if the next position is walkable and does not contain a wall
        if (IsWalkable(tilePosition))
        {
            currentInput = direction;
            targetGridPosition = nextPosition;
            StartCoroutine(LerpMovement());
        }
    }

    bool IsWalkable(Vector3Int tilePosition)
    {
        // A position is walkable if it has a tile in the floor map and no tile in the wall map
        return floorMap.HasTile(tilePosition) && !wallMap.HasTile(tilePosition);
    }

    IEnumerator LerpMovement()
    {
        isMoving = true; // Set isMoving to true at the start of movement
        float elapsedTime = 0f;
        Vector3 startPos = transform.position;
        Vector3 endPos = floorMap.CellToWorld(new Vector3Int(targetGridPosition.x, targetGridPosition.y, 0)) + cellOffset;

        // Start animation and sound
        anim.SetBool("isMoving", true);
        PlayMovementAudio();

        dustEffect.Play(); // Play dust effect

        // Perform Lerp to move between positions
        while (elapsedTime < 1f / moveSpeed)
        {
            transform.position = Vector3.Lerp(startPos, endPos, (elapsedTime * moveSpeed));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final position is aligned with the grid
        transform.position = endPos;
        currentGridPosition = targetGridPosition;

        // Stop animation and sound
        anim.SetBool("isMoving", false);
        audioSource.Stop();
        dustEffect.Stop();

        isMoving = false; // Only set isMoving to false after the movement is complete
    }

    void UpdateAnimator()
    {
        // Update the Animator parameters based on the movement direction
        if (isMoving)
        {
            anim.SetFloat("MoveX", currentInput.x);
            anim.SetFloat("MoveY", currentInput.y);
        }
        else
        {
            // Set to zero to trigger idle animation when not moving
            anim.SetFloat("MoveX", 0);
            anim.SetFloat("MoveY", 0);
        }
    }

    void PlayMovementAudio()
    {
        // Determine which audio to play based on the tile content
        if (IsPellet(targetGridPosition))
        {
            audioSource.clip = pelletAudio;
        }
        else
        {
            audioSource.clip = movingAudio;
        }
        audioSource.Play();
    }

    bool IsPellet(Vector2Int position)
    {
        // Check if there is a pellet at the given position in the pellet tilemap
        Vector3Int tilePosition = new Vector3Int(position.x, position.y, 0);
        return pelletMap.HasTile(tilePosition);
    }
}
