using TMPro;
using UnityEngine;

public class StatisticsManager : MonoBehaviour
{
    public static StatisticsManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI bestScoreText;
    [SerializeField] private TextMeshProUGUI currentGroupText;
    [SerializeField] private TextMeshProUGUI currentPopulationText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;

        UpdateBestScoreText(0f);
    }

    public void UpdateCurrentPopulationText(int currentPopulation)
    {
        currentPopulationText.text = currentPopulation.ToString();
    }

    public void UpdateCurrentGroupText(int currentGroup, int groups)
    {
        currentGroupText.text = $"{currentGroup}/{groups}";
    }

    public void UpdateBestScoreText(float bestScore)
    {
        bestScoreText.text = bestScore.ToString("#0.00");
    }
}
