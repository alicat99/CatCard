using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattlegroundUI : MonoBehaviour
{
    [SerializeField]
    Slider sliderP;
    [SerializeField]
    Slider sliderE;
    [SerializeField]
    TextMeshProUGUI hpTextP;
    [SerializeField]
    TextMeshProUGUI hpTextE;

    [SerializeField]
    GameOverUI gameOver;

    public Entity[] es;

    private CardField field;

    private void Start()
    {
        field = GameManager.Instance.card.field;

        es = new Entity[2];
        es[0] = new Entity(maxHealth: 10, entityType: EntityType.P);
        es[0].onValueUpdate.AddListener(UpdateUI);
        es[1] = new Entity(maxHealth: 10, entityType: EntityType.E);
        es[1].onValueUpdate.AddListener(UpdateUI);
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (es[0].health == 0 || es[1].health == 0)
        {
            field.StopExistingRunCoroutine();
            if (es[0].health == 0)
            {
                gameOver.OnLose();
            }
            else
            {
                gameOver.OnWin();
            }
            return;
        }

        sliderP.value = ((float)es[0].health) / es[0].maxHealth;
        hpTextP.text = $"{es[0].health}<#000>/{es[0].maxHealth}";
        sliderE.value = ((float)es[1].health) / es[1].maxHealth;
        hpTextE.text = $"{es[1].health}<#000>/{es[1].maxHealth}";
    }
}
