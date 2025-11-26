using System.Collections.Generic;
using UnityEngine;

public class BuffController : MonoBehaviour
{
    [SerializeField] StatController statController;
    List<Buffs> buffList = new List<Buffs>();
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
            Destroy(buff);
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
        buffList.Add(newBuff);
        newBuff.statController = statController;
        newBuff.OnGetBuff();
    }
}
