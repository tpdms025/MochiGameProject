using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffControl : MonoBehaviour
{
    //발동중인 버프들
    private BuffableEntity buffableEntity;

    //버프와 연결할 이벤트 함수
    private List<Action<float>> buffActions;

    //버프와 연결할 버프버튼 UI들
    [SerializeField] private BuffButton[] buffButtons;

    //버프와 연결할 스킬버튼 UI들
    [SerializeField] private SkillButton[] skillButtons;

    public static Action SaveAction;


    //원형 버프버튼들의 회전값
    public Quaternion rotation;



    #region Unity methods

    private void Awake()
    {
        buffableEntity = new BuffableEntity();

        //버프 엔더티에 버프를 추가하는 이벤트를 등록
        buffActions = new List<Action<float>>();
        buffActions.Add((float duration) => buffableEntity.AddObserver(new AutomaticMiningBuff(0, duration, MoneyManager.Inst.JewelPerTouch)));
        buffActions.Add((float duration) => buffableEntity.AddObserver(new IncreaseTimedBuff(1, duration, MoneyManager.Inst.JewelPerTouch)));
        buffActions.Add((float duration) => buffableEntity.AddObserver(new IncreaseTimedBuff(2, duration, MoneyManager.Inst.JewelPerSec)));
        buffActions.Add((float duration) => buffableEntity.AddObserver(new FeverBuff(3, duration, MoneyManager.Inst.JewelPerTouch)));

    }

    private void Start()
    {
        //(*중요*)강화기능이 적용된 데이터를 UI버튼에 넣기 때문에 Start함수에서 호출해야함.
        //스킬버튼 초기화
        for (int i = 0; i < skillButtons.Length; i++)
        {
            Database.SkillTable skill = DBManager.Inst.GetSkillTable(i);
            skillButtons[i].onStartedBuff += buffActions[i];
            skillButtons[i].LoadData(DBManager.Inst.inventory.remaining_skillTimes[i], skill.buffTime, DBManager.Inst.inventory.remaining_skillCooldowns[i], skill.maxCooldown);
        }
        //버프버튼 초기화
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
