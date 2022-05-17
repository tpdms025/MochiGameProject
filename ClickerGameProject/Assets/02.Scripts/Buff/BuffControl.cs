using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffControl : MonoBehaviour
{
    //�ߵ����� ������
    private BuffableEntity buffableEntity;

    //������ ������ �̺�Ʈ �Լ�
    private List<Action<float>> buffActions;

    //������ ������ ������ư UI��
    [SerializeField] private BuffButton[] buffButtons;

    //������ ������ ��ų��ư UI��
    [SerializeField] private SkillButton[] skillButtons;

    public static Action SaveAction;


    //���� ������ư���� ȸ����
    public Quaternion rotation;



    #region Unity methods

    private void Awake()
    {
        buffableEntity = new BuffableEntity();

        //���� ����Ƽ�� ������ �߰��ϴ� �̺�Ʈ�� ���
        buffActions = new List<Action<float>>();
        buffActions.Add((float duration) => buffableEntity.AddObserver(new AutomaticMiningBuff(0, duration, MoneyManager.Inst.JewelPerTouch)));
        buffActions.Add((float duration) => buffableEntity.AddObserver(new IncreaseTimedBuff(1, duration, MoneyManager.Inst.JewelPerTouch)));
        buffActions.Add((float duration) => buffableEntity.AddObserver(new IncreaseTimedBuff(2, duration, MoneyManager.Inst.JewelPerSec)));
        buffActions.Add((float duration) => buffableEntity.AddObserver(new FeverBuff(3, duration, MoneyManager.Inst.JewelPerTouch)));

    }

    private void Start()
    {
        //(*�߿�*)��ȭ����� ����� �����͸� UI��ư�� �ֱ� ������ Start�Լ����� ȣ���ؾ���.
        //��ų��ư �ʱ�ȭ
        for (int i = 0; i < skillButtons.Length; i++)
        {
            Database.SkillTable skill = DBManager.Inst.GetSkillTable(i);
            skillButtons[i].onStartedBuff += buffActions[i];
            skillButtons[i].LoadData(DBManager.Inst.inventory.remaining_skillTimes[i], skill.buffTime, DBManager.Inst.inventory.remaining_skillCooldowns[i], skill.maxCooldown);
        }
        //������ư �ʱ�ȭ
        for (int i = 0; i < buffButtons.Length; i++)
        {
            Database.BuffTable buff = DBManager.Inst.GetBuffTable(i);
            buffButtons[i].onStartedBuff += buffActions[skillButtons.Length + i];
            buffButtons[i].LoadData(DBManager.Inst.inventory.remainingbuffTimes[i], buff.time);
        }

        SaveAction += SaveSkillTime;
        SaveAction += SaveSkillCooldown;
        SaveAction += SaveBuffTime;

        StartCoroutine(buffableEntity.Cor_LoopCheckBuffs());
    }


    private void OnDestroy()
    {
        for (int i = 0; i < skillButtons.Length; i++)
        {
            skillButtons[i].onStartedBuff -= buffActions[i];
        }
        for (int i = 0; i < buffButtons.Length; i++)
        {
            buffButtons[i].onStartedBuff -= buffActions[skillButtons.Length + i];
        }
        SaveAction = null;
    }

    #endregion

    #region time Save

    public void SaveSkillTime()
    {
        int[] array = new int[skillButtons.Length];
        for(int i=0;i< array.Length;i++)
        {
            array[i] = (int)skillButtons[i].buffTimeRemaining;
        }

        DBManager.Inst.inventory.remaining_skillTimes = array;
    }
    public void SaveSkillCooldown()
    {
        int[] array = new int[skillButtons.Length];
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = (int)skillButtons[i].cooldownRemaining;
        }
        DBManager.Inst.inventory.remaining_skillCooldowns = array;
    }
    public void SaveBuffTime()
    {
        int[] array = new int[buffButtons.Length];
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = (int)buffButtons[i].timeRemaining;
        }
        DBManager.Inst.inventory.remainingbuffTimes = array;
    }

    #endregion

}
