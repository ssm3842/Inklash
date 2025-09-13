using UnityEngine;
using System.Collections.Generic;

public class DataManager : MonoBehaviour
{
    public static DataManager Inst { get; private set; }
    void Awake()
    {
        Inst = this;
        LoadCsvData();
    }

    [SerializeField] private CardContentSO cardContentSO;

    //[SerializeField] private TextAsset UnitCsvFile;
    //[SerializeField] private TextAsset MagicCsvFile;
    [SerializeField] private TextAsset[] csvFiles;

    public Dictionary<string, CardContent> playerCardDatas = new Dictionary<string, CardContent>();
    public Dictionary<string, CardContent> enemyUnitDatas = new Dictionary<string, CardContent>();

void LoadCsvData()
    {
        Dictionary<string, CardContent> allCardTemplates = new Dictionary<string, CardContent>();
        foreach (CardContent card in cardContentSO.cardContents)
        {
            if (!allCardTemplates.ContainsKey(card.name))
            {
                allCardTemplates.Add(card.name, card);
            }
        }

        foreach (TextAsset csvFile in csvFiles)
        {
            if (csvFile.name.Equals("UnitCard"))
            {
                ParseUnitData(csvFile, allCardTemplates);
            }
            else if (csvFile.name.Equals("SpellCard"))
            {
                ParseSpellData(csvFile, allCardTemplates);
            }
        }
    }
    void ParseUnitData(TextAsset csvFile, Dictionary<string, CardContent> templates)
    {
        string[] lines = csvFile.text.Split('\n');

        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i];
            if (string.IsNullOrWhiteSpace(line)) continue;

            string[] columns = line.Trim().Split(',');

            string cardId = columns[0];
            string cardName = columns[1];

            if (templates.ContainsKey(cardName))
            {
                CardContent targetCard = templates[cardName];

                targetCard.id = columns[0];
                targetCard.name = columns[1];
                targetCard.type = columns[2];
                targetCard.size = columns[3];
                targetCard.description = columns[11];

                targetCard.stats.cost = int.Parse(columns[4]);
                targetCard.stats.hp = float.Parse(columns[5]);
                targetCard.stats.atk = float.Parse(columns[6]);
                targetCard.stats.atkSpd = float.Parse(columns[7]);
                targetCard.stats.range = float.Parse(columns[8]);
                targetCard.stats.spd = float.Parse(columns[9]);

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

            if (templates.ContainsKey(cardName))
            {
                CardContent targetCard = templates[cardName];

                targetCard.id = columns[0];
                targetCard.name = columns[1];
                targetCard.type = columns[2];
                targetCard.size = columns[3];

                targetCard.stats.cost = int.Parse(columns[4]);
                targetCard.stats.hp = 1;
                targetCard.stats.atk = float.Parse(columns[6]);
                targetCard.stats.atkSpd = 1;
                targetCard.stats.range = 1;
                targetCard.stats.spd = 1;

                //targetCard.stats.hp = float.Parse(columns[5]);
                //targetCard.stats.atk = float.Parse(columns[6]);
                //targetCard.stats.atkSpd = float.Parse(columns[7]);
                //targetCard.stats.range = float.Parse(columns[8]);
                //targetCard.stats.spd = float.Parse(columns[9]);

                if (cardId.StartsWith("P", System.StringComparison.OrdinalIgnoreCase))
                {
                    if (!playerCardDatas.ContainsKey(cardName)) playerCardDatas.Add(cardName, targetCard);
                }
            }
        }
    }

}