using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingTextManager : MonoBehaviour
{
    static FloatingTextManager instance;

    [SerializeField]
    GameObject textPrefab;

    ObjectPoolManager objPool;

    Canvas canvas
    {
        get
        {
            if (_canvas == null)
            {
                var cs = FindObjectsOfType<Canvas>();
                foreach (var c in cs)
                {
                    if (_canvas == null || _canvas.sortingOrder < c.sortingOrder)
                    {
                        _canvas = c;
                    }
                }
            }

            if (_canvas == null)
                Debug.LogError("To use floatingtext, please create canvas on scene");
            return _canvas;
        }
    }
    Canvas _canvas;

    public void Initialize()
    {
        instance = this;
        objPool = ObjectPoolManager.Instance;
        objPool.InitializeSpawn(textPrefab, 10, 10, AP_enum.EmptyBehavior.ReuseOldest, AP_enum.MaxEmptyBehavior.ReuseOldest);
    }

    public static void Print(string text, Vector2 pos, Color color, int size = 48, float floatingTime = 1, Transform parent = null)
    {
        if (!instance) 
        {
            Debug.LogError("Floating Text를 사용하려 하지만 Floating Text매니저가 없습니다");
            return;
        }
        if (parent == null)
        {
            parent = instance.canvas.transform;
        }

        instance.DoPrint(text, pos, size, color, floatingTime, parent);
    }

    void DoPrint(string text, Vector2 pos, int size, Color color, float floatingTime, Transform parent)
    {
        GameObject t = objPool.Spawn(textPrefab.name);
        t.GetComponent<FloatingTextObject>().Float(text, pos, size, color, floatingTime, parent);
    }

    private void OnDestroy()
    {
        ObjectPoolManager.Instance?.DespawnPool(textPrefab.name);
    }
}
