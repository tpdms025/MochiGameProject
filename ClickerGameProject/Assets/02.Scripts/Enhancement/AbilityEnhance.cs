using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//DB���� �о�� ������ ��ȭ ������ŭ �ɷ��� ����Ű�� Ŭ�����Դϴ�.
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

        //���� �� ���Ҹ� �� ����� �迭�� �Է�
        AddAbility();

        //����� DB �����͸� �о�� ��ȭ����� ����
        ApplyAbilityEnhance();
    }


    /// <summary>
    /// �ɷ��� ����Ų��.
    /// </summary>
    public void Enhance(int idx, float rate)
    {
        //����ó��
        if (ability.Count < idx)
            return;
        if (curAmountList.Length < idx)
            return;

        //�ɷ� ���⿡ ���� �ۼ�Ʈ�� �߰� �� ����
        ability[idx].rateCalc.RefreshRate(originDatas[idx].name, rate);
        //��ȭ�� �ۼ�Ʈ�� DB�� ����
        curAmountList[idx] = rate;

        //�ɷ��� ���� ��ȭ�����Ƿ� �̺�Ʈ �Լ��� ȣ��
        if (ability[idx].onValueChanged != null)
        {
            ability[idx].onValueChanged.Invoke(idx);
        }
    }


    #region Private Method

    /// <summary>
    /// ���� �� ���Ҹ� �� ����� �迭�� �Է�
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
    /// ����� DB �����͸� �о�� �ɷ��� ��ȭ�ϴ� ����� ����
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
