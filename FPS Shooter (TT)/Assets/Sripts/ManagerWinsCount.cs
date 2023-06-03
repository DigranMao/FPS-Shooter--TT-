using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ManagerWinsCount : MonoBehaviour
{
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private int enemyCount;
    private bool hasWon = false;

    void Update()
    {
        enemyCount = FindObjectsOfType<EnemyAI>().Length;
        if(enemyCount == 0 && !hasWon)
        {
            hasWon = true;

            int currentWinCount = PlayerPrefs.GetInt("WinsCount");
            int winsCount = currentWinCount + 1;
            PlayerPrefs.SetInt("WinsCount", winsCount);

            victoryPanel.SetActive(true);
            Invoke("OnMenu", 6);
        }
    }

    void OnMenu()
    {
        SceneManager.LoadScene(0);
    }
}
