using UnityEngine;
using System.Collections;

public class PacStudentController : MonoBehaviour
{
    public GameObject pacStudent;
    public float moveSpeed = 2f;
    private bool isMoving = false;
    private Vector3 targetPos;

    private Animator anim;
    private AudioSource walkingAudio;

    void Start()
    {
        // Initialize Animator and AudioSource components from the PacStudent GameObject
        anim = pacStudent.GetComponent<Animator>();
        walkingAudio = pacStudent.GetComponent<AudioSource>();

    }

    void Update()
    {
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

        if (Vector3.Distance(pacStudent.transform.position, targetPos) < 0.01f)
        {
            anim.SetBool("isMoving", false);
            isMoving = false;

            walkingAudio.Stop();
        }
    }
}
