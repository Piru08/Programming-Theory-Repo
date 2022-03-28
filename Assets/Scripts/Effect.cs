using TMPro;
using UnityEngine;

public class Effect : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private CanvasGroup group;
    void Update()
    {
        group.alpha = Mathf.Lerp(group.alpha, 0, Time.deltaTime * 4);
        transform.position += Vector3.up * Time.deltaTime * 60;
        if (group.alpha < 0.1f)
            Destroy(gameObject);
    }

    public void SetPosition(Vector2 position)
    {
        transform.position = position;
    }
    public void SetValue(int value)
    {
        text.text = "+" + value;
    }
    public void SetPassiveColor()
    {
        text.color = Color.blue;
    }
}
