using TMPro;
using UnityEngine;

public class PopulationSize : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI popSizeText;

    private float groupSize;
    private float groupsCount;

    public void SetGroupSize(float size)
    {
        groupSize = size;
        SetPopSizeText();
    }

    public void SetGroupsCount(float count)
    {
        groupsCount = count;
        SetPopSizeText();
    }

    private void SetPopSizeText()
    {
        popSizeText.text = $"{groupsCount * groupSize}";
    }
}
