using StorkStudios.CoreNest;
using UnityEngine;

public class MeatCutting : Minigame
{
    [Header("Prefabs")]
    [SerializeField]
    private GameObject markerPrefab;
    [SerializeField]
    private GameObject cutAnimationPrefab;

    [Header("Settings")]
    [SerializeField]
    private int numberOfMarkers = 2;

    [Header("References")]
    [SerializeField]
    private AudioSource cutAudioSource;
    [SerializeField]
    private Sprite cursorSprite;

    [Header("Events")]
    [SerializeField]
    private int hotinEventThreshold;
    public Trigger cutHotinEvent;

    private MeatCuttingMarker currentMarker;
    private int markersCut = 0;

    public override string Name => "Chop";

    private static Texture2D scaledCursor;

    protected override void Start()
    {
        base.Start();

        if (scaledCursor == null)
        {
            scaledCursor = ScaleTexture(cursorSprite.texture, 32, 32);
        }
    }

    public override void StartMinigame()
    {
        base.StartMinigame();

        ShowNextMarker();

        Cursor.SetCursor(scaledCursor, Vector2.zero, CursorMode.Auto);
    }

    public override void EndMinigame(bool win)
    {
        base.EndMinigame(win);

        if (currentMarker != null)
        {
            Destroy(currentMarker.gameObject);
        }

        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

        markersCut = 0;
    }

    private void OnMarkerCut()
    {
        Vector2 pos = currentMarker.GetComponent<RectTransform>().anchoredPosition;
        ShowCutAnimation(pos);

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

    private void ShowCutAnimation(Vector2 position)
    {
        RectTransform animation = Instantiate(cutAnimationPrefab, canvas.transform).GetComponent<RectTransform>();
        animation.anchoredPosition = position;
        this.CallDelayed(2f, () => Destroy(animation.gameObject));
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
