using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCardTemplate : ScriptableObject, ICardTemplate
{
    [SerializeField]
    List<CardData> data;

    public CardData this[int i, int j]
    {
        get => data[i * 3 + j];
    }

    public bool IsValid(CardData[,] field)
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (this[i, j] != null && field[i, j] != null)
                    return false;
            }
        }
        return true;
    }

    public void SetCard(CardData[,] field)
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                field[i, j] = this[i, j];
            }
        }
    }
}
