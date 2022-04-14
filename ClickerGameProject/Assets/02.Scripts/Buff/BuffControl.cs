using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffControl : MonoBehaviour
{

    //�ߵ����� ������
    private BuffableEntity buffableEntity;

    //������ ������ �̺�Ʈ �Լ�
    private List<Action<float>> buffAction;

    //������ ������ ������ưUI��
    [SerializeField] private BuffButton[] buffButtons;

    //������ ������ ��ų��ưUI��
    [SerializeField] private SkillButton[] skillButtons;


    //���� ������ư���� ȸ����
    public Quaternion rotation;



    #region Unity methods

    private void Awake()
    {
        buffableEntity = GetComponent<BuffableEntity>();

        //���� ����Ƽ�� ������ �߰��ϴ� �̺�Ʈ ���
        buffAction = new List<Action<float>>();
        buffAction.Add((float duration) => buffableEntity.AddBuff(new AutomaticMiningBuff(0, duration, MoneyManager.Instance.totalJewelPerTouch)));
        buffAction.Add((float duration) => buffableEntity.AddBuff(new IncreaseTimedBuff(1, duration, MoneyManager.Instance.totalJewelPerTouch)));
        buffAction.Add((float duration) => buffableEntity.AddBuff(new IncreaseTimedBuff(2, duration, MoneyManager.Instance.totalJewelPerSec)));
        buffAction.Add((float duration) => buffableEntity.AddBuff(new FeverBuff(3, duration, MoneyManager.Instance.totalJewelPerTouch)));

        //��ų��ư �ʱ�ȭ
        for (int i = 0; i < skillButtons.Length; i++)
        {
            Database.SkillTable skill = DBManager.Inst.GetSkillTable(i);
            skillButtons[i].LoadData(skill.buffRemainingTime, skill.buffTime,skill.cooldownRemaining,skill.maxCooldown);
            skillButtons[i].onStartedBuff += buffAction[i];
        }
        //������ư �ʱ�ȭ
        for (int i = 0; i < buffButtons.Length; i++)
        {
            Database.BuffTable buff = DBManager.Inst.GetBuffTable(i);
            buffButtons[i].LoadData(buff.remainingTime, buff.time);
            buffButtons[i].onStartedBuff += buffAction[skillButtons.Length + i];
        }
    }

    private void OnDestroy()
    {
        for (int i = 0; i < skillButtons.Length; i++)
        {
            skillButtons[i].onStartedBuff -= buffAction[i];
        }
        for (int i = 0; i < buffButtons.Length; i++)
        {
            buffButtons[i].onStartedBuff -= buffAction[skillButtons.Length + i];
        }
    }

    #endregion
}
