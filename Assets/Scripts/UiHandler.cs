using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiHandler : MonoBehaviour
{
    private static UiHandler ui;

    [SerializeField] private GameObject scorePanel;
    [SerializeField] private GameObject newMaxScore;
    [SerializeField] private Sprite[] bulletDisplays;
    [SerializeField] private Image bulletHud;
    [SerializeField] private TextMeshProUGUI scoreText;

    public static UiHandler UI { get => ui; }

    private void Awake()
    {
        ui = this;
    }

    // Start is called before the first frame update
    private void Start()
    {
        //configurando eventos 
        PlayerBehavior.Instance.OnGunFired += UpdateBulletDisplay;
        PlayerBehavior.Instance.OnGunReloaded += UpdateBulletDisplay;

        GameManager.Game.OnNewMaxScore += DisplayNewMaxScoreText;

        scorePanel.SetActive(false);
        newMaxScore.SetActive(false);

        UpdateBulletDisplay();
    }

    public void UpdateBulletDisplay()
    {
        bulletHud.sprite = bulletDisplays[PlayerBehavior.Instance.Ammo];
    }

    public void UpdateScoreText(string value)
    {
        scoreText.text = value.ToString();
    }

    public void SetupScorePanel()
    {
        TextMeshProUGUI scoreText = scorePanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI maxScoreText = scorePanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>();

        scoreText.text += "\n" + Mathf.Round(GameManager.Score);
        maxScoreText.text += "\n" + GameManager.MaxScore;

        StartCoroutine(OpenScoreMenu());
    }



    private IEnumerator OpenScoreMenu()
    {
        yield return new WaitForSeconds(0.6f);
        scorePanel.SetActive(true);
    }

    private void DisplayNewMaxScoreText()
    {
        newMaxScore.SetActive(true);
    }

    private void FixedUpdate()
    {
        UpdateScoreText("" + Mathf.Round(GameManager.Score));
    }
}
