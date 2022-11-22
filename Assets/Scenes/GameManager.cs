using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using TMPro;
public class GameManager : MonoBehaviour
{
    private List<int> playerTaskList = new List<int>();
    private List<int> playerSequenceList = new List<int>();
    public List<AudioClip> buttonSoundList = new List<AudioClip>();
    public List<List<Color32>> buttonColors = new List<List<Color32>>();
    public List<Button> clickableButtons;
    public AudioClip loseSound;
    public AudioClip startSound;
    public AudioSource audioSource;
    public CanvasGroup buttons;
    public GameObject startButton;
    [SerializeField]
    bool alive = true;
    int scorePoints = 0;
    TextMeshProUGUI scoreText;
    TextMeshProUGUI gameOver;
    void Awake()
    {
        scoreText = GameObject.Find("Score").GetComponent<TextMeshProUGUI>();
        gameOver = GameObject.Find("GameOver").GetComponent<TextMeshProUGUI>();
        audioSource.volume = 0.3f;
        scoreText.text = "Score: " + scorePoints;
        buttonColors.AddRange(new List<List<Color32>>
        { new List<Color32> { new Color32(110, 0, 0, 100), new Color32(255, 0, 0, 255) }, //red
          new List<Color32> { new Color32(130, 110, 0, 100), new Color32(255, 255, 0, 255) }, //yellow
          new List<Color32> { new Color32(4, 110, 2, 100), new Color32(0, 255, 0, 255) }, //green
          new List<Color32> { new Color32(0, 15, 111, 100), new Color32(0, 0, 255, 255) } }); //blue

        for (int i = 0; i < 4; i++)
        {
            clickableButtons[i].GetComponent<Image>().color = buttonColors[i][0];
        }
    }
    public void AddToPlayerSequenceList(int buttonId)
    {
        if (!alive)
        {
            return;
        }
        StartCoroutine(HighlightButton(buttonId, 0.25f));
        playerSequenceList.Add(buttonId);

        for (int i = 0; i < playerSequenceList.Count; i++)
        {
            if (playerTaskList[i] != playerSequenceList[i])
            {
                StartCoroutine(PlayerLost());
                return;
            }
            continue;
        }
        if (playerSequenceList.Count == playerTaskList.Count)
        {
            scorePoints++;
            scoreText.text = "Score:  " + scorePoints;
            StartCoroutine(StartNextRound()); 
        }
    }
    public void StartGame()
    {
        alive = true;
        scoreText.text = "Score: " + scorePoints;
        gameOver.enabled = false;
        StartCoroutine(StartNextRound());
        startButton.SetActive(false);
    }

    IEnumerator PlayerLost()
    {
        gameOver.enabled = true;
        audioSource.PlayOneShot(loseSound);
        playerSequenceList.Clear();
        playerTaskList.Clear();
        scorePoints = 0;
        alive = false;
        yield return new WaitForSeconds(3f);
        startButton.SetActive(true);
    }
    IEnumerator HighlightButton(int buttonId, float d)
    {
        clickableButtons[buttonId].GetComponent<Image>().color = buttonColors[buttonId][1];
        audioSource.PlayOneShot(buttonSoundList[buttonId]);
        yield return new WaitForSeconds(d);
        clickableButtons[buttonId].GetComponent<Image>().color = buttonColors[buttonId][0];
    }
    IEnumerator StartNextRound()
    {
        playerSequenceList.Clear();
        buttons.interactable = false;
        yield return new WaitForSeconds(0.9f);
        playerTaskList.Add(Random.Range(0, 4));
        foreach (int index in playerTaskList)
        {
            StartCoroutine(HighlightButton(index, 0.40f));
            yield return new WaitForSeconds(0.65f);
        }
        yield return new WaitForSeconds(0.1f);
        buttons.interactable = true;
        yield return null;
    }
}
