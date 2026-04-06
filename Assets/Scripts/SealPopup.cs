using TMPro;
using UnityEngine;

public class SealPopup : MonoBehaviour
{
    [SerializeField]TextMeshProUGUI sealName;
    [SerializeField]TextMeshProUGUI sealDescription;

    void Start()
    {
        gameObject.SetActive(false);
    }
    
    public void SetupPopup(SealDataSO sealDataSO)
    {
        sealName.text = sealDataSO.sealName;
        sealDescription.text = sealDataSO.sealDescription;
    }
}
