using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface BuffPublisher
{
    //������ ��ü �߰�
    public void AddObserver(TimedBuff timedBuff);
    //�����ڵ鿡�� �̺�Ʈ �߻� ����
    public void NotifyObservers();
}

public class BuffableEntity : BuffPublisher
{
    //����ϰ� �ִ� ���� �����̳�
    private readonly Dictionary<int, TimedBuff> _buffs = new Dictionary<int, TimedBuff>();


    //@Override
    public void AddObserver(TimedBuff buff)
    {
        if (!_buffs.ContainsKey(buff.Id))
        {
            _buffs.Add(buff.Id, buff);
            buff.Activate();
        }
    }

    //�������鿡�� ��������� ����
    //@Override
    public void NotifyObservers()
    {
        if (_buffs.Count != 0)
        {
            foreach (var buff in _buffs.Values)
            {
                buff.Tick(Time.deltaTime);
            }
        }
    }

    //�̺�Ʈ �߻� �Լ�
    public IEnumerator Cor_LoopCheckBuffs()
    {
        while (!DBManager.Inst.isGameStop)
        {
            NotifyObservers();

            if (_buffs.Count != 0)
            {
                HashSet<int> ids = new HashSet<int>();
                foreach (var buff in _buffs.Values)
                {
                    //���� �ð��� �����ٸ� �����̳ʿ� id ����
                    if (buff.isFinished == true)
                    {
                        ids.Add(buff.Id);
                    }
                }
                //���� ������ �����̳ʿ��� �����Ѵ�.
                foreach (var id in ids)
                {
                    _buffs.Remove(id);
                }
            }
            yield return null;
        }
    }

    //private void Start()
    //{
    //    StartCoroutine(Cor_LoopCheckBuffs());
    //}

    //public IEnumerator Cor_LoopCheckBuffs()
    //{
    //    while (!DBManager.Inst.isGameStop)
    //    {
    //        if (_buffs.Count != 0)
    //        {
    //            HashSet<int> ids=new HashSet<int>();

    //            foreach (var buff in _buffs.Values)
    //            {
    //                buff.Tick(Time.deltaTime);

    //                //���� �ð��� �����ٸ� �����̳ʿ� id ����
    //                if (buff.isFinished == true)
    //                {
    //                    ids.Add(buff.Id);
    //                }
    //            }

    //            //���� ������ �����̳ʿ��� �����Ѵ�.
    //            foreach(var id in ids)
    //            {
    //                _buffs.Remove(id);
    //            }
    //        }
    //        yield return null;
    //    }
    //}


    ///// <summary>
    ///// ������ �߰��ϰ� �ߵ���Ų��.
    ///// </summary>
    ///// <param name="buff"></param>
    //public void AddBuff(TimedBuff buff)
    //{
    //    if(!_buffs.ContainsKey(buff.Id))
    //    {
    //        _buffs.Add(buff.Id, buff);
    //        buff.Activate();
    //    }
    //}
}
