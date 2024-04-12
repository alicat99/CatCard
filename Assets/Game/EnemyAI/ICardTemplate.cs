using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface ICardTemplate
{
    public bool IsValid(CardData[,] field);

    public void SetCard(CardData[,] field);
}
