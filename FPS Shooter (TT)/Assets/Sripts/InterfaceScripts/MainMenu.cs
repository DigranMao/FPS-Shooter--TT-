using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI winsText, lossesText;
    private int winsCount, lossesCount;

    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        lossesCount = PlayerPrefs.GetInt("LossesCount");
        winsCount = PlayerPrefs.GetInt("WinsCount");

        winsText.text = "Wins: " + winsCount.ToString();
        lossesText.text = "Losses: " + lossesCount.ToString();
    }

    public void OnPlay()
    {
        SceneManager.LoadScene(1);
    }

    public void OnExit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
