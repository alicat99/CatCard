using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameOverTransitionUI : MonoBehaviour
{
    [SerializeField]
    GameObject xpOrb;
    [SerializeField]
    Transform targetPos;

    public void Activate()
    {
        DontDestroyOnLoad(gameObject);

        var image = GetComponent<Image>();
        image.DOFade(1, 3).From(0);
    }

    public void CreateOrb(Vector3 position)
    {
        for (int i = 0; i < 5; i++)
        {
            var obj = Instantiate(xpOrb, position, Quaternion.identity, transform);
            float scale = Random.value + 0.2f;
            obj.transform.DOScale(scale, 0.5f).From(0).SetEase(Ease.OutExpo);
            obj.GetComponent<XpOrbUI>().Initialize(targetPos.position, new Vector3(Random.value * 2000 + 2000, Random.value * 1000 - 100));
        }

        for (int i = 0; i < 3; i++)
        {
            var obj = Instantiate(xpOrb, position, Quaternion.identity, transform);
            float scale = Random.value * 0.2f + 0.2f;
            obj.transform.DOScale(scale, 0.5f).From(0).SetEase(Ease.OutExpo);
            XpOrbUI xpOrbUI = obj.GetComponent<XpOrbUI>();
            xpOrbUI.Initialize(targetPos.position, new Vector3(Random.value * 2000 - 1000, Random.value * 500 - 100));
            xpOrbUI.useClip = false;
        }
    }
}
