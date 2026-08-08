using UnityEngine;

public class SoundPlayerTriggerHelper : MonoBehaviour
{
    public void PlayCommonSound(string name)
    {
        CommonSoundManager.Instance.PlaySound(name);
    }
}
