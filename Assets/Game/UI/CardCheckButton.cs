using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardCheckButton : MonoBehaviour
{
    [SerializeField]
    CardField field;

    Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() =>
        {
            StartCoroutine(OnClickHandler());
        });
    }

    private IEnumerator OnClickHandler()
    {
        button.enabled = false;

        yield return null;

        yield return field.RunField();

        button.enabled = true;
    }
}
