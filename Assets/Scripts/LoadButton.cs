using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Network = Assets.Scripts.Persistance.Models.Network;

public class LoadButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    [HideInInspector] public Network Network { get; set; }

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
        fitnessText.text = $"Score: {fitness.ToString("#0.0")}";
    }

    public void SetSavedNetwork(Network network)
    {
        Network = network;
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
