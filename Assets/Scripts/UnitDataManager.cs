using UnityEngine;
using System.Collections.Generic;

public class UnitDataManager : MonoBehaviour
{
    [SerializeField] private CardDataLinkSO cardDataLinkSO;
    [SerializeField] private TextAsset[] csvFiles;

    // 텍스트 데이터를 저장할 캐시 선언
    private Dictionary<string, string> stringTable = new Dictionary<string, string>();
    //카드 보상을 받을 보상 풀 설정
    List<CardDataSO> cardRewardPool;
    List<CardContent> shopUseCardPool;
    List<CardContent> shopWordCardPool;

    public void LoadCsvData()
    {
        //카드 풀을 초기화
        cardRewardPool = new List<CardDataSO>();
        shopUseCardPool = new List<CardContent>();
        shopWordCardPool = new List<CardContent>();

        // 1. StringTable을 가장 먼저 찾아서 로드
        // 다른 카드들이 이름과 설명 데이터를 읽을 때 이 테이블을 참조
        foreach (TextAsset csvFile in csvFiles)
        {
            if (csvFile.name.Equals("04_StringTable")) 
            {
                ParseStringData(csvFile);
                break; // 찾았으면 1단계 종료
            }
        }

        // 2. 텍스트 로드가 끝난 후, 나머지 데이터(유닛, 마법 등) 로드
        foreach (TextAsset csvFile in csvFiles)
        {
            // StringTable은 이미 읽었으니 건너뜀
            if (csvFile.name.Equals("04_StringTable")) continue;

            if (csvFile.name.Equals("00_UnitCard")) //유닛카드의 경우.
            {
                ParseUnitData(csvFile);
            }
            else if (csvFile.name.Equals("01_SpellCard")) //마법카드의 경우.
            {
                ParseSpellData(csvFile);
            }
            else if (csvFile.name.Equals("02_WordCard")) //단어카드의 경우.
            {
                ParseWordData(csvFile);
            }
            else if (csvFile.name.Equals("03_EnemyCard")) //적 유닛의 경우.
            {
                ParseEnemyUnitData(csvFile);
            }
        }

        Debug.Log("모든 데이터 로드 및 텍스트 할당 완료");
    }
    
    // 키값을 주면 실제 텍스트를 반환하는 함수
    private string GetText(string key)
    {
        if (string.IsNullOrEmpty(key)) return "";

        if (stringTable.ContainsKey(key))
        {
            return stringTable[key];
        }
        
        // 키가 없으면 에러 대신 키 자체를 반환 (디버깅용: "UNIT_NAME_SWORD"가 그대로 게임에 뜸)
        return key;
    }

    void ParseStringData(TextAsset csvFile)
    {
        stringTable.Clear(); 
        string[] lines = csvFile.text.Split('\n');

        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i];
            // 빈 줄이나 // 주석 줄은 건너뛰기
            if (string.IsNullOrWhiteSpace(line) || line.StartsWith("//")) continue;

            string[] row = line.Split(',');

            // CSV 구조: [0]=Key, [1]=KR, [2]=EN
            string key = row[0].Trim();
            string krText = row[1].Trim(); // 한국어 가져오기

            if (!stringTable.ContainsKey(key) && !string.IsNullOrEmpty(key))
            {
                stringTable.Add(key, krText);
            }
        }
        Debug.Log($"StringTable 준비 완료: {stringTable.Count}개 단어");
    }

    void ParseUnitData(TextAsset csvFile)
    {
        // 한줄로 입력된 유닛 정보를 lines로 저장.
        string[] lines = csvFile.text.Split('\n'); 

        // 첫 줄은 각 항목들의 설명이므로 1부터 시작.
        for (int i = 1; i < lines.Length; i++)
        {
            //유닛별로 정보를 저장.
            string line = lines[i]; 
            if (string.IsNullOrWhiteSpace(line)) continue;

            //콤마로 정보를 구분.
            string[] columns = line.Trim().Split(','); 

            foreach(CardLink cardLink in cardDataLinkSO.playerUnits)
            {
                //id가 일치하지 않으면 읽지 않음.
                if(cardLink.id != columns[0]) continue;
                
                // 1. 대분류 (Unit/Spell/Word)
                // ParseUnitData이므로 무조건 Unit으로 고정.
                cardLink.cardContents.card.cardType = CardType.Unit;

                // 2. 기본 정보 파싱
                //id가 일치한다면 데이터를 읽어서 CardDataSO에 저장.
                cardLink.cardContents.card.id = columns[0];

                // 텍스트 정보 가져오기
                cardLink.cardContents.card.name = GetText(columns[1]);
                cardLink.cardContents.card.description = GetText(columns[9]);

                // 코스트 정보
                cardLink.cardContents.card.cost = int.Parse(columns[2]);

                // 공격 타입 (근거리/원거리) 저장
                // 데이터 테이블 수정할 때의 근접/원거리 유닛 가시성과 편의성을 위한 더미값에 가까움
                // 카드 UI에 공격 아이콘을 표시할 때에 일괄 처리가 가능하기도 함 
                string attackTypeStr = columns[3].Trim();
                if (attackTypeStr == "Melee") cardLink.cardContents.card.attackType = AttackType.Melee;
                else if (attackTypeStr == "Ranged") cardLink.cardContents.card.attackType = AttackType.Ranged;

                // 각종 수치 할당
                cardLink.cardContents.card.stats.baseMaxHp = float.Parse(columns[4]);
                cardLink.cardContents.card.stats.baseATK = float.Parse(columns[5]);
                cardLink.cardContents.card.stats.baseATKTerm = float.Parse(columns[6]);
                cardLink.cardContents.card.stats.baseATKSpd = 1f;
                cardLink.cardContents.card.stats.baseRange = float.Parse(columns[7]);
                cardLink.cardContents.card.stats.baseSpd = float.Parse(columns[8]);

                //카드 보상 풀에 추가
                cardRewardPool.Add(cardLink.cardContents);
                shopUseCardPool.Add(cardLink.cardContents.card);

                break;
            }
        }
    }
    void ParseSpellData(TextAsset csvFile)
    {
        // 한줄로 입력된 유닛 정보를 lines로 저장.
        string[] lines = csvFile.text.Split('\n'); 

        // 첫 줄은 각 항목들의 설명이므로 1부터 시작.
        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i];
            if (string.IsNullOrWhiteSpace(line)) continue;

            string[] columns = line.Trim().Split(',');

            foreach(CardLink cardLink in cardDataLinkSO.playerSpells)
            {
                //id가 일치하지 않으면 읽지 않음.
                if(cardLink.id != columns[0]) continue;
                
                // 1. 대분류 (Unit/Spell/Word)
                // ParseSpellData이므로 무조건 Spell으로 고정.
                cardLink.cardContents.card.cardType = CardType.Spell;

                // 2. 기본 정보 파싱
                //id가 일치한다면 데이터를 읽어서 CardDataSO에 저장.
                cardLink.cardContents.card.id = columns[0];

                // 텍스트 정보 가져오기
                cardLink.cardContents.card.name = GetText(columns[1]);
                cardLink.cardContents.card.description = GetText(columns[5]);

                // 코스트 정보
                cardLink.cardContents.card.cost = int.Parse(columns[2]);

                //메인 값들을 기록. 데미지, 공속 변화량 등.
                cardLink.cardContents.card.stats.baseATK = float.Parse(columns[3]); 

                // cardLink.cardContents.card.stats.baseATK = float.Parse(columns[4]);
                // cardLink.cardContents.card.stats.baseATKSpd = float.Parse(columns[5]);

                //사거리를 기록.
                cardLink.cardContents.card.stats.baseRange = float.Parse(columns[4]);

                // cardLink.cardContents.card.stats.baseSpd = float.Parse(columns[7]);

                //카드 보상 풀에 추가
                cardRewardPool.Add(cardLink.cardContents);
                shopUseCardPool.Add(cardLink.cardContents.card);

                break;
            }
        }
    }

    void ParseWordData(TextAsset csvFile)
    {
        // 한줄로 입력된 유닛 정보를 lines로 저장.
        string[] lines = csvFile.text.Split('\n'); 

        // 첫 줄은 각 항목들의 설명이므로 1부터 시작.
        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i];
            if (string.IsNullOrWhiteSpace(line)) continue;

            string[] columns = line.Trim().Split(',');

            foreach(CardLink cardLink in cardDataLinkSO.playerWords)
            {
                //id가 일치하지 않으면 읽지 않음.
                if(cardLink.id != columns[0]) continue;

                // 1. 대분류 (Unit/Spell/Word)
                // ParseWordData이므로 무조건 Word으로 고정.
                cardLink.cardContents.card.cardType = CardType.Word;

                //id가 일치한다면 데이터를 읽어서 CardDataSO에 저장.
                cardLink.cardContents.card.id = columns[0];

                // 텍스트 정보 가져오기
                cardLink.cardContents.card.name = GetText(columns[1]);
                cardLink.cardContents.card.description = GetText(columns[3]);

                // 코스트 정보
                cardLink.cardContents.card.cost = int.Parse(columns[2]);

                //카드 보상 풀에 추가
                cardRewardPool.Add(cardLink.cardContents);
                shopWordCardPool.Add(cardLink.cardContents.card);
                
                break;
            }
        }
    }

    void ParseEnemyUnitData(TextAsset csvFile)
    {
        // 한줄로 입력된 유닛 정보를 lines로 저장.
        string[] lines = csvFile.text.Split('\n'); 

        // 첫 줄은 각 항목들의 설명이므로 1부터 시작.
        for (int i = 1; i < lines.Length; i++)
        {
            //유닛별로 정보를 저장.
            string line = lines[i]; 
            if (string.IsNullOrWhiteSpace(line)) continue;

            //콤마로 정보를 구분.
            string[] columns = line.Trim().Split(','); 

            foreach(CardLink cardLink in cardDataLinkSO.EnemyUnits)
            {
                //id가 일치하지 않으면 읽지 않음.
                if(cardLink.id != columns[0]) continue;
                
                // 1. 대분류 (Unit/Spell/Word)
                // ParseEnemyData이므로 무조건 Unit으로 고정.
                cardLink.cardContents.card.cardType = CardType.Unit;

                //id가 일치한다면 데이터를 읽어서 CardDataSO에 저장.
                cardLink.cardContents.card.id = columns[0];

                // 텍스트 정보 가져오기
                cardLink.cardContents.card.name = columns[1];
                cardLink.cardContents.card.description = columns[8];

                // cardLink.cardContents.card.cost = int.Parse(columns[2]);
                // -> 적 유닛에는 코스트 값이 불필요함

                // 공격 타입 (근거리/원거리) 저장
                // 데이터 테이블 수정할 때의 근접/원거리 유닛 가시성과 편의성을 위한 더미값에 가까움
                // 인게임에서 적 유닛 위에 공격 아이콘을 표시할 때에 일괄 처리가 가능하기도 함 
                string attackTypeStr = columns[2].Trim();
                if (attackTypeStr == "Melee") cardLink.cardContents.card.attackType = AttackType.Melee;
                else if (attackTypeStr == "Ranged") cardLink.cardContents.card.attackType = AttackType.Ranged;

                // 각종 수치 할당
                cardLink.cardContents.card.stats.baseMaxHp = float.Parse(columns[3]);
                cardLink.cardContents.card.stats.baseATK = float.Parse(columns[4]);
                cardLink.cardContents.card.stats.baseATKTerm = float.Parse(columns[5]);
                cardLink.cardContents.card.stats.baseATKSpd = 1f;
                cardLink.cardContents.card.stats.baseRange = float.Parse(columns[6]);
                cardLink.cardContents.card.stats.baseSpd = float.Parse(columns[7]);

                break;
            }
        }
    }

    public List<CardDataSO> GetCardRewardPool()
    {
        return cardRewardPool;
    }
    public List<CardContent> GetShopUseCardPool()
    {
        return shopUseCardPool;
    }
    public List<CardContent> GetShopWordCardPool()
    {
        return shopWordCardPool;
    }
    
}