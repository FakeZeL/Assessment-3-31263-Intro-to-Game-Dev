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

        Vector3Int startPosition = floorMap.WorldToCell(transform.position);
        currentGridPosition = new Vector2Int(startPosition.x, startPosition.y);
        targetGridPosition = currentGridPosition;

        transform.position = floorMap.CellToWorld(startPosition) + cellOffset;
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

        UpdateAnimator();
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
        return floorMap.HasTile(tilePosition) && !wallMap.HasTile(tilePosition);
    }

    IEnumerator LerpMovement()
    {
        isMoving = true; 
        float elapsedTime = 0f;
        Vector3 startPos = transform.position;
        Vector3 endPos = floorMap.CellToWorld(new Vector3Int(targetGridPosition.x, targetGridPosition.y, 0)) + cellOffset;

        anim.SetBool("isMoving", true);
        PlayMovementAudio();

        dustEffect.Play(); 

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
        dustEffect.Stop();

        isMoving = false; 
    }

    void UpdateAnimator()
    {
        if (isMoving)
        {
            anim.SetFloat("MoveX", currentInput.x);
            anim.SetFloat("MoveY", currentInput.y);
        }
        else
        {
            anim.SetFloat("MoveX", 0);
            anim.SetFloat("MoveY", 0);
        }
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
        Vector3Int tilePosition = new Vector3Int(position.x, position.y, 0);
        return pelletMap.HasTile(tilePosition);
    }
}
