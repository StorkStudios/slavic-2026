using StorkStudios.CoreNest;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

using SceneEnum = StorkStudios.CoreNest.Scene;

public class MainMenuController : MonoBehaviour
{
    [SerializeField]
    private List<TextMeshProUGUI> continueLabels;
    [SerializeField]
    private GameObject normalMenu;
    [SerializeField]
    private GameObject scaryMenu;
    [SerializeField]
    private RangeBoundariesFloat normalDuration;
    [SerializeField]
    private RangeBoundariesFloat scaryDuration;

    private void Start()
    {
        continueLabels.ForEach(label => label.text = $"Continue: Day {ShiftDays.Instance.CurrentDayNumber}");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        StartCoroutine(MenuCoroutine());
    }

    private IEnumerator MenuCoroutine()
    {
        while (true)
        {
            normalMenu.SetActive(true);
            scaryMenu.SetActive(false);

            yield return new WaitForSeconds(normalDuration.GetRandomBetween());

            normalMenu.SetActive(false);
            scaryMenu.SetActive(true);

            yield return new WaitForSeconds(scaryDuration.GetRandomBetween());
        }
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene(SceneEnum.SampleScene.GetBuildIndex());
    }
}
