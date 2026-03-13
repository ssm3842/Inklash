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
            case SealType.Chill:
                return sealSprites[7];
            case SealType.Ultimate:
                return sealSprites[8];
            case SealType.Split:
                return sealSprites[9];
            case SealType.Explosion:
                return sealSprites[10];
            case SealType.Purity:
                return sealSprites[11];
            case SealType.Copy:
                return sealSprites[12];
            default:
                return null;
        }
    }
}
