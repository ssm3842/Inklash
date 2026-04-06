using UnityEngine;

public class SpriteDataContainer : MonoBehaviour
{
    public static SpriteDataContainer Inst { get; private set; }
    private void Awake()
    {
        if (Inst != null && Inst != this)
        {
            Destroy(gameObject);
            return;
        }

        Inst = this;

        DontDestroyOnLoad(gameObject);
    }

    [SerializeField]Sprite[] cardBackgrounds;
    [SerializeField]SealDataSO[] sealDatas;
    [SerializeField]Sprite[] mapIconWhite;
    [SerializeField]Sprite[] mapIconBlack;


    public Sprite GetCardBackgroundSprite(CardType cardType)
    {
        switch(cardType)
        {
            case CardType.Unit:
                return cardBackgrounds[0];
            case CardType.Spell:
                return cardBackgrounds[1];
            case CardType.Word:
                return cardBackgrounds[2];
            default:
                return null;
        }
    }

    public SealDataSO GetSealData(SealType sealType)
    {
        switch(sealType)
        {
            case SealType.Ignite:
                return sealDatas[0];
            case SealType.Cold:
                return sealDatas[1];
            case SealType.ExtraHit:
                return sealDatas[2];
            case SealType.KnockBack:
                return sealDatas[3];
            case SealType.Pierce:
                return sealDatas[4];
            case SealType.Weak:
                return sealDatas[5];
            case SealType.Mark:
                return sealDatas[6];
            case SealType.Ultimate:
                return sealDatas[7];
            case SealType.Split:
                return sealDatas[8];
            case SealType.Explosion:
                return sealDatas[9];
            case SealType.Purity:
                return sealDatas[10];
            case SealType.Copy:
                return sealDatas[11];
            default:
                return null;
        }
    }

    public Sprite GetMapIconWhite(RoomType roomType)
    {
        switch(roomType)
        {
            case RoomType.BATTLE:
                return mapIconWhite[0];
            case RoomType.BOSS:
                return mapIconWhite[1];
            case RoomType.SHOP:
                return mapIconWhite[2];
            default:
                return null;
        }
    }

    public Sprite GetMapIconBlack(RoomType roomType)
    {
        switch(roomType)
        {
            case RoomType.BATTLE:
                return mapIconBlack[0];
            case RoomType.BOSS:
                return mapIconBlack[1];
            case RoomType.SHOP:
                return mapIconBlack[2];
            default:
                return null;
        }
    }

    public Sprite GetMapEventIconWhite(EventRoomType roomType)
    {
        switch(roomType)
        {
            case EventRoomType.ADDCARD:
                return mapIconWhite[3];
            case EventRoomType.UPGRADE:
                return mapIconWhite[4];
            case EventRoomType.MIXCARD:
                return mapIconWhite[5];
            case EventRoomType.MAKESEAL:
                return mapIconWhite[6];
            default:
                return null;
        }
    }

    public Sprite GetMapEventIconBlack(EventRoomType roomType)
    {
        switch(roomType)
        {
            case EventRoomType.ADDCARD:
                return mapIconBlack[3];
            case EventRoomType.UPGRADE:
                return mapIconBlack[4];
            case EventRoomType.MIXCARD:
                return mapIconBlack[5];
            case EventRoomType.MAKESEAL:
                return mapIconBlack[6];
            default:
                return null;
        }
    }
}
