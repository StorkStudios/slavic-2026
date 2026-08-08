using StorkStudios.CoreNest;
using UnityEngine;

public class CommonSoundManager : Singleton<CommonSoundManager>
{
    [SerializeField]
    private SerializedDictionary<string, AudioSource> audioSources;

    public void PlaySound(string name)
    {
        if (audioSources.ContainsKey(name))
        {
            audioSources[name].Play();
        }
        else
        {
            Debug.LogWarning($"Sound not found in {gameObject.name}. Key: {name}");
        }
    } 
}
