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
        float randomSide = Random.Range(0, 4);

        switch (randomSide)
        {
            case 0: // Left
                startPos = new Vector3(mainCamera.transform.position.x - mainCamera.orthographicSize * 2, Random.Range(-5, 5), 0);
                endPos = new Vector3(mainCamera.transform.position.x + mainCamera.orthographicSize * 2, Random.Range(-5, 5), 0);
                break;
            case 1: // Right
                startPos = new Vector3(mainCamera.transform.position.x + mainCamera.orthographicSize * 2, Random.Range(-5, 5), 0);
                endPos = new Vector3(mainCamera.transform.position.x - mainCamera.orthographicSize * 2, Random.Range(-5, 5), 0);
                break;
            case 2: // Top
                startPos = new Vector3(Random.Range(-5, 5), mainCamera.transform.position.y + mainCamera.orthographicSize, 0);
                endPos = new Vector3(Random.Range(-5, 5), mainCamera.transform.position.y - mainCamera.orthographicSize, 0);
                break;
            case 3: // Bottom
                startPos = new Vector3(Random.Range(-5, 5), mainCamera.transform.position.y - mainCamera.orthographicSize, 0);
                endPos = new Vector3(Random.Range(-5, 5), mainCamera.transform.position.y + mainCamera.orthographicSize, 0);
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
