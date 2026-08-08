using StorkStudios.CoreNest;
using UnityEngine;

public class MeatCutting : Minigame
{
    [Header("Prefabs")]
    [SerializeField]
    private GameObject markerPrefab;

    [Header("Settings")]
    [SerializeField]
    private int numberOfMarkers = 2;

    [Header("References")]
    [SerializeField]
    private AudioSource cutAudioSource;

    [Header("Events")]
    [SerializeField]
    private int hotinEventThreshold;
    [SerializeField]
    public Trigger cutHotinEvent;

    private MeatCuttingMarker currentMarker;
    private int markersCut = 0;

    public override string Name => "Chop";

    public override void StartMinigame()
    {
        base.StartMinigame();

        ShowNextMarker();
    }

    public override void EndMinigame(bool win)
    {
        base.EndMinigame(win);

        if (currentMarker != null)
        {
            Destroy(currentMarker.gameObject);
        }

        markersCut = 0;
    }

    private void OnMarkerCut()
    {
        CleanupMarker();
        markersCut++;
        Debug.Log("Marker cut");
        if (cutAudioSource != null)
        {
            cutAudioSource.Play();
        }
        if (Hotin.Instance.Value > hotinEventThreshold)
        {
            cutHotinEvent.Invoke();
        }
        if (markersCut < numberOfMarkers)
        {
            ShowNextMarker();
        }
        else
        {
            EndMinigame(true);
        }
    }

    private void CleanupMarker()
    {
        currentMarker.MarkerMissed -= OnMarkerMissed;
        currentMarker.MarkerCut -= OnMarkerCut;
        Destroy(currentMarker.gameObject);
        currentMarker = null;
    }

    private void OnMarkerMissed()
    {
        CleanupMarker();
        Debug.Log("Marker missed");
        ShowNextMarker();
    }

    private void ShowNextMarker()
    {
        Vector3 randomPosition = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
        currentMarker = Instantiate(markerPrefab, canvas.transform).GetComponent<MeatCuttingMarker>();
        currentMarker.GetComponent<RectTransform>().anchoredPosition = randomPosition * 100f;

        currentMarker.MarkerMissed += OnMarkerMissed;
        currentMarker.MarkerCut += OnMarkerCut;
    }
}
