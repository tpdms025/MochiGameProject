using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrestigeSystem : MonoBehaviour
{
    //�ٷ� ���� ���ʽ�(%)
    public int STRLevelBonus { get; private set; }

    //���� ���� ���ʽ�(%)
    public int oreCollectionBonus { get; private set; }

    //�ϲ� ���� ���ʽ�(%)
    public int workmanCollectionBonus { get; private set; }

    //ȯ�� Ƚ�� ���ʽ�(%)
    public int prestigeCountBonus { get; private set; }

    //ȯ�� �������� �ּ� �ٷ� ����
    public const int minSTRLevel = 90;

    //�ִ� ���ʽ��� (%)
    public readonly int STR_MaxBonus = 103;
    public readonly int ore_MaxBonus = 50;
    public readonly int workman_MaxBonus = 40;



    //���� ���ʽ� �ۼ�Ʈ ����(%)
    public int totalBonusPercent { get; private set; }




    private void Awake()
    {
        //����� DB �����͸� �о�� ���� ȯ�����ʽ��� ����
        ApplyCumBouns();
    }



    /// <summary>
    /// ȯ���Ͽ� ó������ �ٽ� �����Ѵ�.
    /// </summary>
    public void Replay()
    {
        //ȯ�� ������ �������� ���� ��� ����
        if (!IsPrestigeable()) 
            return;


        //������ �ʱ�ȭ
        DBManager.Inst.ResetDataOnPrestige();

        //ȯ�� ���ʽ��� ������Ŵ.
        DBManager.Inst.playData.AddPrestige(totalBonusPercent);
        ApplyCumBouns();

        //������ �� ��ȯ
        StartCoroutine(Cor_PlayAnimation());
    }

    /// <summary>
    /// ȯ���� ������ ��´�.
    /// </summary>
    public int AwardBonus()
    {
        STRLevelBonus = GetStrengthLevelBonus(DBManager.Inst.inventory.enhanceLevels_owned[0]);
        oreCollectionBonus = GetOreCollectionBonus(DBManager.Inst.inventory.oreIdx_lastOwned+1);
        workmanCollectionBonus = GetWorkerCollectionBonus(DBManager.Inst.inventory.workmanCount);   //�����ʿ�
        prestigeCountBonus = GetPrestigeCountBonus(DBManager.Inst.playData.prestigeCount);          //�����ʿ�

        totalBonusPercent = STRLevelBonus + oreCollectionBonus + workmanCollectionBonus + prestigeCountBonus;
        return totalBonusPercent;
    }



    /// <summary>
    /// ȯ���� �� �ִ��� Ȯ���Ѵ�.
    /// </summary>
    public bool IsPrestigeable()
    {
        //ȯ������ : �ּ� �ٷ·����� �����Ǿ�� �Ѵ�.
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
    /// ���� ȯ�����ʽ��� ��ġ�� ȹ�淮�� �����Ѵ�.
    /// </summary>
    private void ApplyCumBouns()
    {
        if (DBManager.Inst.playData.cumPrestigeRate != 0)
        {
            MoneyManager.Inst.JewelPerTouch.rateCalc.RefreshRate("���� ȯ�����ʽ�", DBManager.Inst.playData.cumPrestigeRate);
        }
    }



    private IEnumerator Cor_PlayAnimation()
    {
        DBManager.Inst.isGameStop = true;
        //TODO:����
        //

        //�ӽü���
        yield return new WaitForSeconds(2.0f);

        DBManager.Inst.isGameStop = false;

        //�� �ε�
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    /// <summary>
    /// ��ȭ�� �ٷ� ������ ���� ���ʽ��� �����´�.
    /// </summary>
    private int GetStrengthLevelBonus(int level)
    {
        int result = 0;
        int unit = 5;
        int[] levelStep = { 50, 60, 80, 150, 200 };
        int[] addBonus = { 1, 2, 3, 5 };

        //�ּҷ������� ���� ��� ����
        if (level < levelStep[0])
            return result;

        for (int i = 1; i < levelStep.Length; i++)
        {
            int count;
            //�� ���������� ���� ������ŭ �߰� ���ʽ��� ����
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
    /// ���� �������� ���� ���ʽ��� �����´�
    /// </summary>
    private int GetOreCollectionBonus(int count)
    {
        int result = 0;
        int[] countStep = { 5, 10 };
        int[] bonuses = { 20, 50 };

        for (int i = countStep.Length-1; i >= 0; i--)
        {
            //�������� �� ���� ���
            if (count >= countStep[i])
            {
                result = bonuses[i];
                break;
            }
        }
        return result;
    }

    /// <summary>
    /// �ϲ� �������� ���� ���ʽ��� �����´�.
    /// </summary>
    private int GetWorkerCollectionBonus(int count)
    {
        int result = 0;
        int[] countStep = { 4, 8 };
        int[] bonuses = { 15, 40 };

        for (int i = countStep.Length - 1; i >= 0; i--)
        {
            //�������� �� ���� ���
            if (count >= countStep[i])
            {
                result = bonuses[i];
                break;
            }
        }
        return result;
    }

    /// <summary>
    /// ���� ȯ�� Ƚ���� ���� ���ʽ��� �����´�.
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
