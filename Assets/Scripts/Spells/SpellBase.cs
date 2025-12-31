using UnityEngine;

public class SpellBase : MonoBehaviour
{
    public virtual void CastSpell(float damage, float range, float castXPosition)
    {
        return;
    }

    public void _OnAnimationEnd()
    {
        Destroy(gameObject);
    }
}
