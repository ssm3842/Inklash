using UnityEngine;

public class SpellAreaViewer : MonoBehaviour
{
    public void SetAreaWidth(float spellRange)
    {
        GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 146 * spellRange);
    }
}
