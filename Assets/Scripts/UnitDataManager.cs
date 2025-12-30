using UnityEngine;
using System.Collections.Generic;

public class UnitDataManager : MonoBehaviour
{
    [SerializeField] private CardDataLinkSO cardDataLinkSO;
    [SerializeField] private TextAsset[] csvFiles;

    public void LoadCsvData()
    {
        // Dictionary<string, CardContent> allCardTemplates = new Dictionary<string, CardContent>();
        // foreach (CardContent card in cardContentSO.cardContents)
        // {
        //     if (!allCardTemplates.ContainsKey(card.id)) //카드 아이디를 키로 해서 카드 정보를 딕셔너리로 신규 생성.
        //     {
        //         allCardTemplates.Add(card.id, card);
        //     }
        // }

        foreach (TextAsset csvFile in csvFiles)
        {
            if (csvFile.name.Equals("UnitCard")) //유닛카드의 경우.
            {
                ParseUnitData(csvFile);
            }
            else if (csvFile.name.Equals("SpellCard")) //마법카드의 경우.
            {
                ParseSpellData(csvFile);
            }
            else if (csvFile.name.Equals("WordCard")) //단어카드의 경우.
            {
                ParseSpellData(csvFile);
            }
            else if (csvFile.name.Equals("EnemyCard")) //적 유닛의 경우.
            {
                ParseEnemyUnitData(csvFile);
            }
        }
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
                
                //id가 일치한다면 데이터를 읽어서 CardDataSO에 저장.
                cardLink.cardContents.card.id = columns[0];
                cardLink.cardContents.card.name = columns[1];
                cardLink.cardContents.card.cost = int.Parse(columns[2]);
                cardLink.cardContents.card.type = CardType.Unit;
                cardLink.cardContents.card.description = columns[9];

                cardLink.cardContents.card.stats.baseMaxHp = float.Parse(columns[3]);
                cardLink.cardContents.card.stats.baseATK = float.Parse(columns[4]);
                cardLink.cardContents.card.stats.baseATKSpd = float.Parse(columns[5]);
                cardLink.cardContents.card.stats.baseRange = float.Parse(columns[6]);
                cardLink.cardContents.card.stats.baseSpd = float.Parse(columns[7]);
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
                
                //id가 일치한다면 데이터를 읽어서 CardDataSO에 저장.
                cardLink.cardContents.card.id = columns[0];
                cardLink.cardContents.card.name = columns[1];
                cardLink.cardContents.card.cost = int.Parse(columns[2]);
                cardLink.cardContents.card.type = CardType.Spell;
                // cardLink.cardContents.card.description = columns[9];

                cardLink.cardContents.card.stats.baseATK = float.Parse(columns[3]);
                // cardLink.cardContents.card.stats.baseATK = float.Parse(columns[4]);
                // cardLink.cardContents.card.stats.baseATKSpd = float.Parse(columns[5]);
                // cardLink.cardContents.card.stats.baseRange = float.Parse(columns[6]);
                // cardLink.cardContents.card.stats.baseSpd = float.Parse(columns[7]);
                break;
            }
        }
    }

    void ParseWordData(TextAsset csvFile)
    {
        // // 한줄로 입력된 유닛 정보를 lines로 저장.
        // string[] lines = csvFile.text.Split('\n'); 

        // // 첫 줄은 각 항목들의 설명이므로 1부터 시작.
        // for (int i = 1; i < lines.Length; i++)
        // {
        //     string line = lines[i];
        //     if (string.IsNullOrWhiteSpace(line)) continue;

        //     string[] columns = line.Trim().Split(',');

        //     foreach(CardLink cardLink in cardDataLinkSO.playerWords)
        //     {
        //         //id가 일치하지 않으면 읽지 않음.
        //         if(cardLink.id != columns[0]) continue;
                
        //         //id가 일치한다면 데이터를 읽어서 CardDataSO에 저장.
        //         cardLink.cardContents.card.id = columns[0];
        //         cardLink.cardContents.card.name = columns[1];
        //         cardLink.cardContents.card.cost = int.Parse(columns[2]);
        //         cardLink.cardContents.card.type = CardType.Spell;
        //         // cardLink.cardContents.card.description = columns[9];

        //         cardLink.cardContents.card.stats.baseATK = float.Parse(columns[3]);
        //         // cardLink.cardContents.card.stats.baseATK = float.Parse(columns[4]);
        //         // cardLink.cardContents.card.stats.baseATKSpd = float.Parse(columns[5]);
        //         // cardLink.cardContents.card.stats.baseRange = float.Parse(columns[6]);
        //         // cardLink.cardContents.card.stats.baseSpd = float.Parse(columns[7]);
        //         break;
        //     }
        // }
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
                
                //id가 일치한다면 데이터를 읽어서 CardDataSO에 저장.
                cardLink.cardContents.card.id = columns[0];
                cardLink.cardContents.card.name = columns[1];
                cardLink.cardContents.card.cost = int.Parse(columns[2]);
                cardLink.cardContents.card.type = CardType.Unit;
                cardLink.cardContents.card.description = columns[9];

                cardLink.cardContents.card.stats.baseMaxHp = float.Parse(columns[3]);
                cardLink.cardContents.card.stats.baseATK = float.Parse(columns[4]);
                cardLink.cardContents.card.stats.baseATKSpd = float.Parse(columns[5]);
                cardLink.cardContents.card.stats.baseRange = float.Parse(columns[6]);
                cardLink.cardContents.card.stats.baseSpd = float.Parse(columns[7]);
                break;
            }
        }
    }

}