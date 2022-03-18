using System.Collections.Generic;
using UnityEngine;

public abstract class Group<T> : MonoBehaviour where T : class
{
    [HideInInspector] public List<T> Items { get; private set; }

    public void Subscribe(T board)
    {
        if (Items == null)
        {
            Items = new List<T>();
        }

        Items.Add(board);
    }

    public abstract void OnBoardPointerExit(T item);

    public abstract void OnBoardPointerEnter(T item);

    public abstract void OnBoardPointerClick(T item);
}
