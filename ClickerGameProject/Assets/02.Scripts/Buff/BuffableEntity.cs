using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffableEntity : MonoBehaviour
{
    //����ϰ� �ִ� ���� �����̳�
    private readonly Dictionary<int, TimedBuff> _buffs = new Dictionary<int, TimedBuff>();

    private void Start()
    {
        StartCoroutine(Cor_LoopCheckBuffs());
    }

    private IEnumerator Cor_LoopCheckBuffs()
    {
        while (true)
        {
            if (_buffs.Count != 0)
            {
                HashSet<int> ids=new HashSet<int>();

                foreach (var buff in _buffs.Values)
                {
                    buff.Tick(Time.deltaTime);

                    //���� �ð��� �����ٸ� �����̳ʿ� id ����
                    if (buff.isFinished == true)
                    {
                        ids.Add(buff.Id);
                    }
                }

                //���� ������ �����̳ʿ��� �����Ѵ�.
                foreach(var id in ids)
                {
                    _buffs.Remove(id);
                }
            }
            yield return null;
        }
    }

   
    /// <summary>
    /// ������ �߰��ϰ� �ߵ���Ų��.
    /// </summary>
    /// <param name="buff"></param>
    public void AddBuff(TimedBuff buff)
    {
        if(!_buffs.ContainsKey(buff.Id))
        {
            _buffs.Add(buff.Id, buff);
            buff.Activate();
        }
    }
}
