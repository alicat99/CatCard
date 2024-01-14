using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingTextObject : MonoBehaviour
{
    TextMeshProUGUI textMesh;

    float time;
    float floatST;
    Vector2 startPos;
    new RectTransform transform;
    [SerializeField]
    AnimationCurve ac;

    private void Awake()
    {
        transform = GetComponent<RectTransform>();
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    public void Float(string text, Vector2 pos, int size, Color color, float floatingTime, Transform parent)
    {
        textMesh.text = text;
        startPos = pos;
        textMesh.fontSize = size;
        textMesh.color = color;
        transform.SetParent(parent);
        transform.anchoredPosition = pos;
        time = floatingTime;
        floatST = Time.time;
    }

    private void Update()
    {
        if (time == 0) return;
        float d = Time.time - floatST;
        d /= time;
        if(d < 1)
        {
            Vector2 pos = startPos;
            pos.y += ac.Evaluate(d);
            transform.anchoredPosition = pos;
            Color color = textMesh.color;
            color.a = 1 - d;
            textMesh.color = color;
        }
        else
        {
            ObjectPoolManager.Instance.Despawn(gameObject);
        }
    }
}
