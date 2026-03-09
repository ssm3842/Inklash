using UnityEngine;

public class DeckSelectButton : MonoBehaviour
{
    [SerializeField] DeckSO buttonDeckSO;

    public void _OnButtonClicked()
    {
        DeckManager.Inst.SetStartDeck(buttonDeckSO);
    }
}
