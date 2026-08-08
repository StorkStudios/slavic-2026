using TMPro;
using UnityEngine;

public class tmpHotiinUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text;

    private void Update()
    {
        text.text = $"hotin{Hotin.Instance.Value}";
    }
}
