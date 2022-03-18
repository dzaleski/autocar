using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class LoadButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    [HideInInspector] public string NetworkId { get; set; }

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI fitnessText;
    [SerializeField] private TextMeshProUGUI createdDateText;

    private LoadButtonsGroup loadButtonsGroup;

    private void Awake()
    {
        loadButtonsGroup = transform.parent.GetComponent<LoadButtonsGroup>();
        loadButtonsGroup.Subscribe(this);
    }

    public void SetCreatedDate(DateTime date)
    {
        createdDateText.text = date.ToString();
        
    }

    public void SetFitnessText(float fitness)
    {
        fitnessText.text = $"Fitness: {fitness}";
    }

    public void SetNetworkId(string networkId)
    {
        NetworkId = networkId;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        loadButtonsGroup.OnBoardPointerExit(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        loadButtonsGroup.OnBoardPointerEnter(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        loadButtonsGroup.OnBoardPointerClick(this);
    }
}
