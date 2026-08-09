using StorkStudios.CoreNest;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering.LookDev;
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

    [SerializeField]
    private GameObject defeatScreen;
    [SerializeField]
    private GameObject victoryScreen;

    public static bool? Win = null;

    private void Start()
    {
        continueLabels.ForEach(label => label.text = $"Continue");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        StartCoroutine(MenuCoroutine());

        defeatScreen.SetActive(false);
        victoryScreen.SetActive(false);
        if (Win.HasValue)
        {
            if (Win.Value)
            {
                victoryScreen.SetActive(true);
            }
            else
            {
                defeatScreen.SetActive(true);
            }
        }
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
