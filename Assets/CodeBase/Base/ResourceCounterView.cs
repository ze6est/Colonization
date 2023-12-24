using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class ResourceCounterView : MonoBehaviour
{
    private TextMeshProUGUI _countResourcesHUD;

    private void Awake() =>
        _countResourcesHUD = GetComponent<TextMeshProUGUI>();

    public void RefreshText(int countResources) =>
        _countResourcesHUD.text = countResources.ToString();
}