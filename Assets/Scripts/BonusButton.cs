using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusButton : MonoBehaviour
{
    public void StageBonus(string type)    //보너스 지급 함수
    {
        GameManager.Instance.StageBonus(type);
    }
}