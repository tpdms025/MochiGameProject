using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffControl : MonoBehaviour
{

    //발동중인 버프들
    private BuffableEntity buffableEntity;

    //버프와 연결할 이벤트 함수
    private List<Action<float>> buffAction;

    //버프와 연결할 버프버튼UI들
    [SerializeField] private BuffButton[] buffButtons;

    //버프와 연결할 스킬버튼UI들
    [SerializeField] private SkillButton[] skillButtons;


    //원형 버프버튼들의 회전값
    public Quaternion rotation;



    #region Unity methods

    private void Awake()
    {
        buffableEntity = GetComponent<BuffableEntity>();

        //버프 엔더티에 버프를 추가하는 이벤트 등록
        buffAction = new List<Action<float>>();
        buffAction.Add((float duration) => buffableEntity.AddBuff(new AutomaticMiningBuff(0, duration, MoneyManager.Instance.totalJewelPerTouch)));
        buffAction.Add((float duration) => buffableEntity.AddBuff(new IncreaseTimedBuff(1, duration, MoneyManager.Instance.totalJewelPerTouch)));
        buffAction.Add((float duration) => buffableEntity.AddBuff(new IncreaseTimedBuff(2, duration, MoneyManager.Instance.totalJewelPerSec)));
        buffAction.Add((float duration) => buffableEntity.AddBuff(new FeverBuff(3, duration, MoneyManager.Instance.totalJewelPerTouch)));

        //스킬버튼 초기화
        for (int i = 0; i < skillButtons.Length; i++)
        {
            Database.SkillTable skill = DBManager.Inst.GetSkillTable(i);
            skillButtons[i].LoadData(skill.buffRemainingTime, skill.buffTime,skill.cooldownRemaining,skill.maxCooldown);
            skillButtons[i].onStartedBuff += buffAction[i];
        }
        //버프버튼 초기화
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
