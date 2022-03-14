using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillControl : MonoBehaviour
{
    //��ų ���̺�
    public List<Skill> skills;

    //���� ������ ����Ƽ
    private BuffableEntity buffableEntity;

    //������ ������ ��ų ��ư��
    [SerializeField] private SkillButton[] skillButtons;




    #region Unity methods

    private void Awake()
    {
        buffableEntity = GetComponent<BuffableEntity>();
        skills = new List<Skill>();

        //TODO:�̺�Ʈ �߰��� awake�� ���� onEnable�� ���� ����غ���
        //*********************************************************
        skillButtons[0].OnStartedSkill += OnTouchUpSkill;
        skillButtons[1].OnStartedSkill += OnSecAmountUpSkill;

        //��ų ��ư�� ���� �����͸� �����Ѵ�.
        for (int i = 0; i < skillButtons.Length; i++)
        {
            skills.Add(new Skill(skillButtons[i], DBManager.Instance.buffTables[i]));
            skills[i].ProvideDataToBtn();
        }

    }

    private void OnDestroy()
    {
        skillButtons[0].OnStartedSkill -= OnTouchUpSkill;
        skillButtons[1].OnStartedSkill -= OnSecAmountUpSkill;
    }

    #endregion



    #region function to add buff
    private void OnTouchUpSkill(float elapsedTime)
    {
        buffableEntity.AddBuff(new IncreaseTimedBuff(0, elapsedTime, MoneyManager.Instance.totalJewelPerTouch));
    }
    private void OnSecAmountUpSkill(float elapsedTime)
    {
        buffableEntity.AddBuff(new IncreaseTimedBuff(1, elapsedTime, MoneyManager.Instance.totalJewelPerSec));
    }
    #endregion
}

public class Skill
{
    public float elapsedTime { get; private set; }
    public float maxTime { get; private set; }
    public SkillButton skillBtn { get; private set; }


    public Skill(SkillButton skillButton, Database.BuffTable buffTable)
    {
        maxTime = buffTable.time;
        //TODO:��� �׽�Ʈ�� ���� �ּ���.
        //curTime = CalcCurSkillTime(buffTable.curTime);
        elapsedTime = buffTable.curTime;
        this.skillBtn = skillButton;
    }

    public void ProvideDataToBtn()
    {
        skillBtn.LoadData(elapsedTime, maxTime);
    }


    /// <summary>
    /// �������� �ð��� ���Ͽ� ������ ������ ���������� �Ǵ��ϸ�,
    /// ���� ��ų ���� �ð��� ����Ѵ�.
    /// </summary>
    /// <returns></returns>
    private float CalcCurSkillTime(float prevElapsedTime)
    {
        //���������� ������ �� ��ų�ð��� ������ Ȱ��ȭ������ �Ǵ��Ѵ�.
        //���� ��ų�ð��� �ʱ�ȭ���¶�� ��ų�� ��Ȱ��ȭ
        if (prevElapsedTime == maxTime)
        {
            return maxTime;
        }

        float m_time = 0.0f;
        double intervalTime = TimerManager.Instance.offlineTimeSpan.TotalSeconds;
        if (0 < intervalTime && intervalTime < maxTime)
        {
            //��ų Ȱ��ȭ
            m_time = maxTime - (float)intervalTime;
        }
        else
        {
            //��ų ��Ȱ��ȭ
            m_time = maxTime;
        }
        return m_time;
    }
}