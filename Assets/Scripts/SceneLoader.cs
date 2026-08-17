using StorkStudios.CoreNest;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneLoader", menuName = "ScriptableObjects/SceneLoader")]
public class SceneLoader : ScriptableObjectSingleton<SceneLoader>
{
    public void LoadScene(int buildIndex)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(buildIndex);
    }

    public void LoadScene(Scene scene)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene.GetBuildIndex());
    }
}
