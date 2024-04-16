using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WildcardCardTemplate : ScriptableObject, ICardTemplate
{
    [SerializeField]
    int weight;

    [SerializeField]
    private CardData card;

    public bool IsValid(CardData[,] field)
    {
        foreach (var data in field)
            if (data == null)
                return true;
        return false;
    }

    public void SetCard(CardData[,] field)
    {
        int n = 0;
        foreach (var data in field)
            if (data == null)
                n += 1;

        n = Random.Range(0, n);

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (field[i, j] != null) continue;

                if (n == 0)
                {
                    field[i, j] = card;
                    return;
                }
                else
                {
                    n -= 1;
                }
            }
        }
    }

    public int GetWeight() => weight;
}
