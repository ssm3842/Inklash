using UnityEngine;
using System.Collections.Generic;

public class UnitDataManager : MonoBehaviour
{
    [SerializeField] private CardContentSO cardContentSO;
    [SerializeField] private TextAsset[] csvFiles;

    Dictionary<string, CardContent> playerCardDatas = new Dictionary<string, CardContent>();
    public Dictionary<string, CardContent> enemyUnitDatas = new Dictionary<string, CardContent>();

public void LoadCsvData()
    {
        Dictionary<string, CardContent> allCardTemplates = new Dictionary<string, CardContent>();
        foreach (CardContent card in cardContentSO.cardContents)
        {
            if (!allCardTemplates.ContainsKey(card.id)) //카드 아이디를 키로 해서 카드 정보를 딕셔너리로 신규 생성.
            {
                allCardTemplates.Add(card.id, card);
            }
        }

        foreach (TextAsset csvFile in csvFiles)
        {
            if (csvFile.name.Equals("UnitCard")) //유닛의 경우.
            {
                ParseUnitData(csvFile, allCardTemplates);
            }
            else if (csvFile.name.Equals("SpellCard")) //마법의 경우.
            {
                ParseSpellData(csvFile, allCardTemplates);
            }
        }
    }
    void ParseUnitData(TextAsset csvFile, Dictionary<string, CardContent> templates)
    {
        string[] lines = csvFile.text.Split('\n'); //한줄로 입력된 유닛 정보를 lines로 저장.

        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i]; //유닛별로 정보를 저장.
            if (string.IsNullOrWhiteSpace(line)) continue;

            string[] columns = line.Trim().Split(','); //콤마, 로 정보를 구분.

            string cardId = columns[0];
            string cardName = columns[1];

            if (templates.ContainsKey(cardId))
            {
                CardContent targetCard = templates[cardId];

                targetCard.id = columns[0];
                targetCard.name = columns[1];
                targetCard.cost = int.Parse(columns[2]);
                targetCard.type = CardType.Unit;
                targetCard.description = columns[9];

                targetCard.stats.baseMaxHp = float.Parse(columns[3]);
                targetCard.stats.baseATK = float.Parse(columns[4]);
                targetCard.stats.baseATKSpd = float.Parse(columns[5]);
                targetCard.stats.baseRange = float.Parse(columns[6]);
                targetCard.stats.baseSpd = float.Parse(columns[7]);

                if (cardId.StartsWith("P", System.StringComparison.OrdinalIgnoreCase))
                {
                    if (!playerCardDatas.ContainsKey(cardName))
                        playerCardDatas.Add(cardName, targetCard);
                }
                else if (cardId.StartsWith("E", System.StringComparison.OrdinalIgnoreCase))
                {
                    if (!enemyUnitDatas.ContainsKey(cardName))
                        enemyUnitDatas.Add(cardName, targetCard);
                }
            }

        }
    }
    void ParseSpellData(TextAsset csvFile,Dictionary<string, CardContent> templates)
    {
        string[] lines = csvFile.text.Split('\n');

        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i];
            if (string.IsNullOrWhiteSpace(line)) continue;

            string[] columns = line.Trim().Split(',');

            string cardId = columns[0];
            string cardName = columns[1];

            if (templates.ContainsKey(cardId))
            {
                CardContent targetCard = templates[cardId];

                targetCard.id = columns[0];
                targetCard.name = columns[1];
                targetCard.cost = int.Parse(columns[2]);
                targetCard.type = CardType.Spell;

                // targetCard.stats.hp = 1;
                targetCard.stats.baseATK = float.Parse(columns[3]);
                // targetCard.stats.atkSpd = 1;
                // targetCard.stats.range = 1;
                // targetCard.stats.spd = 1;

                if (cardId.StartsWith("P", System.StringComparison.OrdinalIgnoreCase))
                {
                    if (!playerCardDatas.ContainsKey(cardName)) playerCardDatas.Add(cardName, targetCard);
                }
            }
        }
    }

}