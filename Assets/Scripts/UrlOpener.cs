using UnityEngine;

public class UrlOpener : MonoBehaviour
{
    [SerializeField]
    private string url;

    public void OpenUrl()
    {
        Application.OpenURL(url);
    }

    public void OpenUrl(string url)
    {
        Application.OpenURL(url);
    }
}
