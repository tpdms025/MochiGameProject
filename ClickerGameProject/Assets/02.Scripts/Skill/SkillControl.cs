using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillControl : MonoBehaviour
{
    //스킬 테이블
    public List<Skill> skills;

    //버프 가능한 엔터티
    private BuffableEntity buffableEntity;

    //버프와 연결할 스킬 버튼들
    [SerializeField] private SkillButton[] skillButtons;




    #region Unity methods

    private void Awake()
    {
        buffableEntity = GetComponent<BuffableEntity>();
        skills = new List<Skill>();

        //TODO:이벤트 추가를 awake에 할지 onEnable에 할지 고민해볼것
        //*********************************************************
        skillButtons[0].OnStartedSkill += OnTouchUpSkill;
        skillButtons[1].OnStartedSkill += OnSecAmountUpSkill;

        //스킬 버튼에 버프 데이터를 적용한다.
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
        //TODO:잠시 테스트를 위해 주석함.
        //curTime = CalcCurSkillTime(buffTable.curTime);
        elapsedTime = buffTable.curTime;
        this.skillBtn = skillButton;
    }

    public void ProvideDataToBtn()
    {
        skillBtn.LoadData(elapsedTime, maxTime);
    }


    /// <summary>
    /// 오프라인 시간을 비교하여 이전에 버프가 가동중인지 판단하며,
    /// 현재 스킬 실행 시간을 계산한다.
    /// </summary>
    /// <returns></returns>
    private float CalcCurSkillTime(float prevElapsedTime)
    {
        //마지막으로 종료할 때 스킬시간을 가져와 활성화중인지 판단한다.
        //이전 스킬시간이 초기화상태라면 스킬을 비활성화
        if (prevElapsedTime == maxTime)
        {
            return maxTime;
        }

        float m_time = 0.0f;
        double intervalTime = TimerManager.Instance.offlineTimeSpan.TotalSeconds;
        if (0 < intervalTime && intervalTime < maxTime)
        {
            //스킬 활성화
            m_time = maxTime - (float)intervalTime;
        }
        else
        {
            //스킬 비활성화
            m_time = maxTime;
        }
        return m_time;
    }
}