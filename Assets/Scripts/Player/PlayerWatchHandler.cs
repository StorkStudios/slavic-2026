using UnityEngine;

[RequireComponent(typeof(TMPro.TextMeshProUGUI))]
public class PlayerWatchHandler : MonoBehaviour
{
    private TMPro.TextMeshProUGUI text;

    private void Start()
    {
        text = GetComponent<TMPro.TextMeshProUGUI>();
    }

    private void Update()
    {
        
    }
}
