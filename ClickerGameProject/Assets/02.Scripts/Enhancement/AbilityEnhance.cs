using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//DB에서 읽어온 소유한 강화 레벨만큼 능력을 향상시키는 클래스입니다.
public class AbilityEnhance : MonoBehaviour
{
    private List<ValueModifiers> ability;

    private List<Database.ProductOriginData_> originDatas
    {
        get { return DBManager.Inst.GetEnhancementOriginDatas(); }
    }

    private float[] curAmountList
    {
        get { return DBManager.Inst.inventory.enhanceAmounts_owned; }
        set { DBManager.Inst.inventory.enhanceAmounts_owned = value; }
    }

    private void Awake()
    {
        ability = new List<ValueModifiers>();

        //증가 및 감소를 할 대상을 배열에 입력
        AddAbility();

        //저장된 DB 데이터를 읽어와 강화기능을 적용
        ApplyAbilityEnhance();
    }


    /// <summary>
    /// 능력을 향상시킨다.
    /// </summary>
    public void Enhance(int idx, float rate)
    {
        //예외처리
        if (ability.Count < idx)
            return;
        if (curAmountList.Length < idx)
            return;

        //능력 계산기에 비율 퍼센트을 추가 및 갱신
        ability[idx].rateCalc.RefreshRate(originDatas[idx].name, rate);
        //변화된 퍼센트를 DB에 저장
        curAmountList[idx] = rate;

        //능력의 값이 변화했으므로 이벤트 함수를 호출
        if (ability[idx].onValueChanged != null)
        {
            ability[idx].onValueChanged.Invoke(idx);
        }
    }


    #region Private Method

    /// <summary>
    /// 증가 및 감소를 할 대상을 배열에 입력
    /// </summary>
    private void AddAbility()
    {
        ability.Add(MoneyManager.Inst.JewelPerTouch);
        for (int i = 0; i < MoneyManager.Inst.workmansAmount.Count; i++)
        {
            ability.Add(MoneyManager.Inst.workmansAmount[i]);
        }
        ability.Add(MoneyManager.Inst.autoMiningDuration);
        ability.Add(MoneyManager.Inst.autoMiningTouchCnt);
        ability.Add(MoneyManager.Inst.feverDuration);
        ability.Add(MoneyManager.Inst.feverAmount);
    }

    /// <summary>
    /// 저장된 DB 데이터를 읽어와 능력을 강화하는 기능을 적용
    /// </summary>
    private void ApplyAbilityEnhance()
    {
         for (int i = 0; i < curAmountList.Length; i++)
        {
            if (curAmountList[i] != 0)
            {
                Enhance(i, curAmountList[i]);
            }
        }
    }

    #endregion

}
