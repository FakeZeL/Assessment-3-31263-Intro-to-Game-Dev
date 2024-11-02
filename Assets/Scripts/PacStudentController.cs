using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PacStudentController : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of PacStudent's movement
    public AudioClip pelletAudio;
    public AudioClip movingAudio;
    public ParticleSystem dustEffect;
    public Tilemap walkableTilemap; // Reference to the walkable area Tilemap

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
        Vector3Int startPosition = walkableTilemap.WorldToCell(transform.position);
        currentGridPosition = new Vector2Int(startPosition.x, startPosition.y);
        targetGridPosition = currentGridPosition;
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

        // Check if the next position contains a walkable tile
        if (IsWalkable(tilePosition))
        {
            currentInput = direction;
            targetGridPosition = nextPosition;
            StartCoroutine(LerpMovement());
        }
    }

    bool IsWalkable(Vector3Int tilePosition)
    {
        return walkableTilemap.HasTile(tilePosition); // Check if the tile exists
    }

    IEnumerator LerpMovement()
    {
        isMoving = true;
        float elapsedTime = 0f;
        Vector3 startPos = transform.position;
        Vector3 endPos = walkableTilemap.CellToWorld(new Vector3Int(targetGridPosition.x, targetGridPosition.y, 0));

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

        isMoving = false;
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
        // This method can be expanded to check for pellets based on tile types
        // For now, it just returns false.
        return false;
    }
}
