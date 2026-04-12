using System.Collections.Generic;
using UnityEngine;

public class BuffController : MonoBehaviour , IBuffable
{
    [SerializeField] StatController statController;
    public List<Buffs> buffList = new List<Buffs>();
    List<Buffs> buffsToRemove = new List<Buffs>();

    void Update()
    {
        
        if(buffList.Count <= 0) return;
        foreach(Buffs buff in buffList)
        {
            //남은 시간을 확인해 버프가 끝나면 제거.
            if(!buff.CheckBuffValid()) 
            {
                buffsToRemove.Add(buff);
            }
        }
        
        foreach(Buffs buff in buffsToRemove)
        {
            buffList.Remove(buff);
        }

    }

    public void GetBuff(Buffs newBuff)
    {   
        //현재 버프 중 같은 이름의 버프가 있는지 검사.
        foreach(Buffs existingBuff in buffList)
        {
            //같은 이름의 버프가 있으면 시간 갱신.
            if(existingBuff.buffName == newBuff.buffName)
            {
                existingBuff.remainTime = newBuff.remainTime;
                return;
            }
        }
        //반복문을 다 돌아도 같은 이름의 버프가 없으면 버프추가.
        newBuff.statController = statController;
        newBuff.OnGetBuff(this.gameObject);

        //단어 카드의 강화카드는 추적해서 시간에따라 제거하지 않음.
        //if(newBuff.remainTime == -1) return;

        //지속시간 추적이 필요한 버프는 리스트에 추가.
        buffList.Add(newBuff);
    }

    public void ClearBuffs()
    {
        foreach(Buffs buff in buffList)
        {
            buffsToRemove.Add(buff);
        }
        
        foreach(Buffs buff in buffsToRemove)
        {
            buffList.Remove(buff);
        }
    }

    public bool HaveDisruptEffect()
    {
        foreach(Buffs existingBuff in buffList)
        {
            if(existingBuff.order == 1) return true;
        }
        return false;
    }

}
