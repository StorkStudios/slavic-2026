using UnityEngine;

public class MeatCutting : Minigame
{
    [Header("Prefabs")]
    [SerializeField]
    private GameObject meatPrefab;
    [SerializeField]
    private GameObject markerPrefab;

    [Header("References")]
    [SerializeField]
    private Transform meatSpawnPoint;

    private Transform meatTransform;
    private MeatCuttingMarker currentMarker;

    private void Start()
    {
        canvas.enabled = false;
        cinemachineCamera.enabled = false;
    }

    public override void StartMinigame()
    {
        //meatTransform = Instantiate(meatPrefab, meatSpawnPoint.position, meatSpawnPoint.rotation).transform;
        canvas.enabled = true;
        ShowNextMarker();
    }

    public override void EndMinigame()
    {
        base.EndMinigame();
        if (meatTransform != null)
        {
            Destroy(meatTransform.gameObject);
        }
        if (currentMarker != null)
        {
            Destroy(currentMarker.gameObject);
        }
        canvas.enabled = false;
    }

    private void ShowNextMarker()
    {
        Vector3 randomPosition = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
        currentMarker = Instantiate(markerPrefab, randomPosition, Quaternion.identity).GetComponent<MeatCuttingMarker>();
    }
}
