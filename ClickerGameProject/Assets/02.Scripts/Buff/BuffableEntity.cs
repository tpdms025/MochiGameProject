using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface BuffPublisher
{
    //관찰자 객체 추가
    public void AddObserver(TimedBuff timedBuff);
    //관찰자들에게 이벤트 발생 전달
    public void NotifyObservers();
}

public class BuffableEntity : BuffPublisher
{
    //사용하고 있는 버프 컨테이너
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

    //옵저버들에게 변경사항을 전달
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

    //이벤트 발생 함수
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
                    //버프 시간이 끝났다면 컨테이너에 id 저장
                    if (buff.isFinished == true)
                    {
                        ids.Add(buff.Id);
                    }
                }
                //끝난 버프를 컨테이너에서 제거한다.
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

    //                //버프 시간이 끝났다면 컨테이너에 id 저장
    //                if (buff.isFinished == true)
    //                {
    //                    ids.Add(buff.Id);
    //                }
    //            }

    //            //끝난 버프를 컨테이너에서 제거한다.
    //            foreach(var id in ids)
    //            {
    //                _buffs.Remove(id);
    //            }
    //        }
    //        yield return null;
    //    }
    //}


    ///// <summary>
    ///// 버프를 추가하고 발동시킨다.
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
