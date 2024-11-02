using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PacStudentController : MonoBehaviour
{
    public float moveSpeed = 2f; 
    public AudioClip pelletAudio;
    public AudioClip movingAudio;
    //public ParticleSystem dustEffect;
    public Tilemap walkableTilemap; 

    private Vector2Int currentGridPosition;  
    private Vector2Int targetGridPosition;  
    private Vector2Int lastInput = Vector2Int.zero;  
    private Vector2Int currentInput = Vector2Int.zero;

    private bool isMoving = false; 
    private Animator anim;
    private AudioSource audioSource;

    void Start()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        Vector3Int startPosition = walkableTilemap.WorldToCell(transform.position);
        currentGridPosition = new Vector2Int(startPosition.x, startPosition.y);
        targetGridPosition = currentGridPosition;
    }

    void Update()
    {
        HandleInput();

        if (!isMoving)
        {
            TryMove(lastInput);

            if (!isMoving)
            {
                TryMove(currentInput); 
            }
        }
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.W)) lastInput = Vector2Int.up;
        if (Input.GetKeyDown(KeyCode.S)) lastInput = Vector2Int.down;
        if (Input.GetKeyDown(KeyCode.A)) lastInput = Vector2Int.left;
        if (Input.GetKeyDown(KeyCode.D)) lastInput = Vector2Int.right;
    }

    void TryMove(Vector2Int direction)
    {
        Vector2Int nextPosition = currentGridPosition + direction;
        Vector3Int tilePosition = new Vector3Int(nextPosition.x, nextPosition.y, 0);

        if (IsWalkable(tilePosition))
        {
            currentInput = direction;
            targetGridPosition = nextPosition;
            StartCoroutine(LerpMovement());
        }
    }

    bool IsWalkable(Vector3Int tilePosition)
    {
        return walkableTilemap.HasTile(tilePosition); 
    }

    IEnumerator LerpMovement()
    {
        isMoving = true;
        float elapsedTime = 0f;
        Vector3 startPos = transform.position;
        Vector3 endPos = walkableTilemap.CellToWorld(new Vector3Int(targetGridPosition.x, targetGridPosition.y, 0));

        anim.SetBool("isMoving", true);
        PlayMovementAudio();

        //dustEffect.Play(); 

        while (elapsedTime < 1f / moveSpeed)
        {
            transform.position = Vector3.Lerp(startPos, endPos, (elapsedTime * moveSpeed));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = endPos;
        currentGridPosition = targetGridPosition;

        anim.SetBool("isMoving", false);
        audioSource.Stop();
        //dustEffect.Stop();

        isMoving = false;
    }

    void PlayMovementAudio()
    {
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
