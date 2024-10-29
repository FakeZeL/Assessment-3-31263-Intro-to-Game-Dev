using UnityEngine;
using System.Collections;

public class PacStudentController : MonoBehaviour
{
    public GameObject pacStudent;
    public float moveSpeed = 2f;
<<<<<<< Updated upstream

    private int currentWaypointIndex = 0;
    private bool isMoving = false;

    public Vector3[] waypoints;
=======
    private bool isMoving = false;

>>>>>>> Stashed changes

    private Animator anim;
    private AudioSource walkingAudio;
    private Vector3 targetPos;

    void Start()
    {
        // Initialize Animator and AudioSource components from the PacStudent GameObject
        anim = pacStudent.GetComponent<Animator>();
        walkingAudio = pacStudent.GetComponent<AudioSource>();

<<<<<<< Updated upstream
        waypoints = new Vector3[] {
            new Vector3(-7.5f, 3.5f, 0f),
            new Vector3(-7.5f, -0.5f, 0f),
            new Vector3(-2.5f, -0.5f, 0f),
            new Vector3(-2.5f, 3.5f, 0f)
        };

        targetPos = waypoints[currentWaypointIndex];
=======
>>>>>>> Stashed changes
    }

    void Update()
    {
<<<<<<< Updated upstream
        if (Vector3.Distance(pacStudent.transform.position, targetPos) < 0.01f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            targetPos = waypoints[currentWaypointIndex]; // Update the target position
        }

=======
>>>>>>> Stashed changes
        MovePacStudent();
    }

    private void MovePacStudent()
    {
        float step = moveSpeed * Time.deltaTime;

        Vector3 movementDirection = (targetPos - pacStudent.transform.position).normalized;

        anim.SetFloat("MoveX", movementDirection.x);
        anim.SetFloat("MoveY", movementDirection.y);

        // Ensure PacStudent moves at a constant speed and plays the animation and sound
        if (!isMoving)
        {
            isMoving = true;
            anim.SetBool("isMoving", true);

            if (!walkingAudio.isPlaying)
            {
                walkingAudio.Play();
            }
        }

<<<<<<< Updated upstream
        pacStudent.transform.position = Vector3.MoveTowards(pacStudent.transform.position, targetPos, step);

=======
>>>>>>> Stashed changes
        if (Vector3.Distance(pacStudent.transform.position, targetPos) < 0.01f)
        {
            anim.SetBool("isMoving", false);
            isMoving = false;

            walkingAudio.Stop();
        }
    }
}
