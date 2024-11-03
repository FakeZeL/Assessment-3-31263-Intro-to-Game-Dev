using System.Collections;
using UnityEngine;

public class BonusController : MonoBehaviour
{
    public GameObject cherryPrefab;
    public float moveSpeed = 2f;
    private float spawnInterval = 10f;
    private Camera mainCamera;
    private Vector3 startPos, endPos;

    void Start()
    {
        mainCamera = Camera.main;
        StartCoroutine(SpawnCherryRoutine());
    }

    IEnumerator SpawnCherryRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnCherry();
        }
    }

    void SpawnCherry()
    {
        int dir = Random.Range(0, 4);
        Vector3 leftEdge = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height / 2, 0));
        Vector3 rightEdge = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height / 2, 0));
        Vector3 topEdge = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height, 0));
        Vector3 bottomEdge = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, 0, 0));

        switch (dir)
        {
            case 0: // From left to right
                startPos = new Vector3(leftEdge.x - 1, Random.Range(bottomEdge.y, topEdge.y), 0);
                endPos = new Vector3(rightEdge.x + 1, Random.Range(bottomEdge.y, topEdge.y), 0);
                break;

            case 1: // From right to left
                startPos = new Vector3(rightEdge.x + 1, Random.Range(bottomEdge.y, topEdge.y), 0);
                endPos = new Vector3(leftEdge.x - 1, Random.Range(bottomEdge.y, topEdge.y), 0);
                break;

            case 2: // From top to bottom
                startPos = new Vector3(Random.Range(leftEdge.x, rightEdge.x), topEdge.y + 1, 0);
                endPos = new Vector3(Random.Range(leftEdge.x, rightEdge.x), bottomEdge.y - 1, 0);
                break;

            case 3: // From bottom to top
                startPos = new Vector3(Random.Range(leftEdge.x, rightEdge.x), bottomEdge.y - 1, 0);
                endPos = new Vector3(Random.Range(leftEdge.x, rightEdge.x), topEdge.y + 1, 0);
                break;
        }

        GameObject cherry = Instantiate(cherryPrefab, startPos, Quaternion.identity);
        StartCoroutine(MoveCherry(cherry));
    }

    IEnumerator MoveCherry(GameObject cherry)
    {
        Vector3 startPos = cherry.transform.position;
        float distance = Vector3.Distance(startPos, endPos);
        float travelTime = distance / moveSpeed; 

        float elapsedTime = 0f;
        while (elapsedTime < travelTime)
        {
            cherry.transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / travelTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        cherry.transform.position = endPos;

        Destroy(cherry);
    }
}
