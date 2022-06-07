using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private const float HeartHp = 10;
    [SerializeField] private Image[] hpUI = new Image[5];
    [SerializeField] private Text overHpText;
    [SerializeField] private Text hpText;

    private void HeartActive(int count, float remain)
    {
        for (var i = 0; i < 5; i++)
        {
            var fill = i < count ? 1 : 0;
            fill = i == count ? (int) (remain / HeartHp) : fill;
            hpUI[i].fillAmount = fill;
            i++;
        }
    }
    public void UpdateHpUI(float hp)
    {
        var heartCount = (int) (hp / HeartHp);
        var remainHp = hp % HeartHp;

        if (heartCount > 5)
        {
            overHpText.text = $"+{heartCount + Mathf.Ceil(remainHp)}";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
