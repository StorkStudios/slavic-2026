using StorkStudios.CoreNest;
using UnityEngine;

public class UIPicturesController : Singleton<UIPicturesController>
{
    [SerializeField]
    private SerializedDictionary<string, GameObject> pictures;

    public string CurrentPicture => currentPicture;

    private string currentPicture = null;

    private void Start()
    {
        foreach (GameObject picture in pictures.Values)
        {
            picture.SetActive(false);
        }
    }

    public void ShowPicture(string key)
    {
        if (currentPicture != null)
        {
            return;
        }
        pictures[key].SetActive(true);
        currentPicture = key;
    }

    public void HideCurrent()
    {
        if (currentPicture == null)
        {
            return;
        }
        pictures[currentPicture].SetActive(false);
        currentPicture = null;
    }
}
