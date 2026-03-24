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
    [SerializeField]Sprite[] sealSprites;
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

    public Sprite GetSealSprite(SealType sealType)
    {
        switch(sealType)
        {
            case SealType.Ignite:
                return sealSprites[0];
            case SealType.Cold:
                return sealSprites[1];
            case SealType.ExtraHit:
                return sealSprites[2];
            case SealType.KnockBack:
                return sealSprites[3];
            case SealType.Pierce:
                return sealSprites[4];
            case SealType.Weak:
                return sealSprites[5];
            case SealType.Mark:
                return sealSprites[6];
            case SealType.Ultimate:
                return sealSprites[7];
            case SealType.Split:
                return sealSprites[8];
            case SealType.Explosion:
                return sealSprites[9];
            case SealType.Purity:
                return sealSprites[10];
            case SealType.Copy:
                return sealSprites[11];
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
