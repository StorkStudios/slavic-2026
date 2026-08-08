using StorkStudios.CoreNest;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

using SceneEnum = StorkStudios.CoreNest.Scene;

public class MainMenuController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI continueLabel;

    private void Start()
    {
        continueLabel.text = $"Continue: Day {ShiftDays.Instance.CurrentDayNumber}";
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene(SceneEnum.SampleScene.GetBuildIndex());
    }
}
