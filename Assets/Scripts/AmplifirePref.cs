using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class AmplifirePref : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TextMeshProUGUI level;
    [SerializeField] private TextMeshProUGUI price;
    private DamageAmplifier amplifier;
    private CanvasGroup group;

    public void SetData(DamageAmplifier amplifier)
    {
        group = GetComponent<CanvasGroup>();

        this.amplifier = amplifier;
        UpdateUI();
    }

    public void UpdateUI()
    {
        level.text = "x" + amplifier.Level;
        price.text = "$" + amplifier.Price;

        group.alpha = Clicker.Instance.Money >= amplifier.Price ? 1 : 0.5f;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Clicker.Instance.Money < amplifier.Price)
            return;
        Clicker.Instance.AddMoney(-amplifier.Price);
        amplifier.LevelUp();
        UpdateUI();
    }
}
