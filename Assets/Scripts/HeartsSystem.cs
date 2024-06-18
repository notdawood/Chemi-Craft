using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HeartSystem : MonoBehaviour
{
    public TextMeshProUGUI heartsText; // Reference to TextMeshProUGUI component
    public GameObject gameOverContainer; // Reference to the GameOverContainer object
    private int hearts = 3; // Start with 3 hearts

    // Start is called before the first frame update
    void Start()
    {
        UpdateHeartsText();
        gameOverContainer.SetActive(false); // Hide game over container at start
    }

    public void WrongChoice()
    {
        hearts--;

        if (hearts <= 0)
        {
            GameOver();
        }
        else
        {
            UpdateHeartsText();
        }
    }

    private void UpdateHeartsText()
    {
        heartsText.text = "" + hearts;
    }

    private void GameOver()
    {
        heartsText.text = "0";
        gameOverContainer.SetActive(true); // Show game over container
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        // Additional game over logic here (e.g., stopping the game)
    }
}
