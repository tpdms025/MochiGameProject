using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrestigeSystem : MonoBehaviour
{
    //근력 레벨 보너스(%)
    public int STRLevelBonus { get; private set; }

    //광석 수집 보너스(%)
    public int oreCollectionBonus { get; private set; }

    //일꾼 수집 보너스(%)
    public int workmanCollectionBonus { get; private set; }

    //환생 횟수 보너스(%)
    public int prestigeCountBonus { get; private set; }

    //환생 조건으로 최소 근력 레벨
    public const int minSTRLevel = 90;

    //최대 보너스값 (%)
    public readonly int STR_MaxBonus = 103;
    public readonly int ore_MaxBonus = 50;
    public readonly int workman_MaxBonus = 40;



    //최종 보너스 퍼센트 비율(%)
    public int totalBonusPercent { get; private set; }




    private void Awake()
    {
        //저장된 DB 데이터를 읽어와 누적 환생보너스를 적용
        ApplyCumBouns();
    }



    /// <summary>
    /// 환생하여 처음부터 다시 시작한다.
    /// </summary>
    public void Replay()
    {
        //환생 조건이 성립하지 않을 경우 리턴
        if (!IsPrestigeable()) 
            return;


        //데이터 초기화
        DBManager.Inst.ResetDataOnPrestige();

        //환생 보너스를 누적시킴.
        DBManager.Inst.playData.AddPrestige(totalBonusPercent);
        ApplyCumBouns();

        //연출후 씬 전환
        StartCoroutine(Cor_PlayAnimation());
    }

    /// <summary>
    /// 환생시 보상을 얻는다.
    /// </summary>
    public int AwardBonus()
    {
        STRLevelBonus = GetStrengthLevelBonus(DBManager.Inst.inventory.enhanceLevels_owned[0]);
        oreCollectionBonus = GetOreCollectionBonus(DBManager.Inst.inventory.oreIdx_lastOwned+1);
        workmanCollectionBonus = GetWorkerCollectionBonus(DBManager.Inst.inventory.workmanCount);   //수정필요
        prestigeCountBonus = GetPrestigeCountBonus(DBManager.Inst.playData.prestigeCount);          //수정필요

        totalBonusPercent = STRLevelBonus + oreCollectionBonus + workmanCollectionBonus + prestigeCountBonus;
        return totalBonusPercent;
    }



    /// <summary>
    /// 환생할 수 있는지 확인한다.
    /// </summary>
    public bool IsPrestigeable()
    {
        //환생조건 : 최소 근력레벨이 성립되어야 한다.
        if (DBManager.Inst.inventory.enhanceLevels_owned[0] < minSTRLevel)
        {
            return false;
        }
        else
        {
            return true;
        }
    }




    #region Private Method

    /// <summary>
    /// 누적 환생보너스를 터치당 획득량에 적용한다.
    /// </summary>
    private void ApplyCumBouns()
    {
        if (DBManager.Inst.playData.cumPrestigeRate != 0)
        {
            MoneyManager.Inst.JewelPerTouch.rateCalc.RefreshRate("누적 환생보너스", DBManager.Inst.playData.cumPrestigeRate);
        }
    }



    private IEnumerator Cor_PlayAnimation()
    {
        DBManager.Inst.isGameStop = true;
        //TODO:연출
        //

        //임시설정
        yield return new WaitForSeconds(2.0f);

        DBManager.Inst.isGameStop = false;

        //씬 로딩
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    /// <summary>
    /// 강화의 근력 레벨에 따른 보너스를 가져온다.
    /// </summary>
    private int GetStrengthLevelBonus(int level)
    {
        int result = 0;
        int unit = 5;
        int[] levelStep = { 50, 60, 80, 150, 200 };
        int[] addBonus = { 1, 2, 3, 5 };

        //최소레벨보다 낮을 경우 리턴
        if (level < levelStep[0])
            return result;

        for (int i = 1; i < levelStep.Length; i++)
        {
            int count;
            //두 레벨차에서 유닛 레벨만큼 추가 보너스가 누적
            if (level > levelStep[i])
            {
                count = (levelStep[i] - levelStep[i - 1]) / unit;
                result += (addBonus[i - 1] * count);
                if (i == 1) result++;
            }
            else
            {
                count = (level - levelStep[i - 1]) / unit;
                result += (addBonus[i - 1] * count);
                if (i == 1) result++;
                break;
            }
        }
        return result;
    }

    /// <summary>
    /// 광석 보유수에 따른 보너스를 가져온다
    /// </summary>
    private int GetOreCollectionBonus(int count)
    {
        int result = 0;
        int[] countStep = { 5, 10 };
        int[] bonuses = { 20, 50 };

        for (int i = countStep.Length-1; i >= 0; i--)
        {
            //보유수가 더 많을 경우
            if (count >= countStep[i])
            {
                result = bonuses[i];
                break;
            }
        }
        return result;
    }

    /// <summary>
    /// 일꾼 보유수에 따른 보너스를 가져온다.
    /// </summary>
    private int GetWorkerCollectionBonus(int count)
    {
        int result = 0;
        int[] countStep = { 4, 8 };
        int[] bonuses = { 15, 40 };

        for (int i = countStep.Length - 1; i >= 0; i--)
        {
            //보유수가 더 많을 경우
            if (count >= countStep[i])
            {
                result = bonuses[i];
                break;
            }
        }
        return result;
    }

    /// <summary>
    /// 누적 환생 횟수에 따른 보너스를 가져온다.
    /// </summary>
    private int GetPrestigeCountBonus(int count)
    {
        int unit = 5;

        if (count == 0)
        {
            return 0;
        }
        else
        {
            return count / unit;
        }
    }

    #endregion
}
