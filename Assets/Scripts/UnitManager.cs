using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Inst { get; private set; }
    void Awake() => Inst = this;

    [SerializeField] Transform unitSpawnPoint;

    public void SpawnUnit(CardContent cardContent)
    {
        GameObject newUnit = Instantiate(cardContent.unit, unitSpawnPoint);
        newUnit.GetComponent<Rigidbody2D>().linearVelocityX = 1f;
    }
}
