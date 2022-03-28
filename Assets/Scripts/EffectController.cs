using UnityEngine;

public class EffectController : MonoBehaviour
{
    public static EffectController Instance;
    [SerializeField] private Effect effectPref;
    private void Awake()
    {
        Instance = this;
    }
    public void CreateClickEffect(int value)
    {
        var pref = Instantiate(effectPref, transform, false);
        pref.SetPosition(Input.mousePosition);
        pref.SetValue(value);
    }
    public void CreatePassiveEffect(int value)
    {
        var pref = Instantiate(effectPref, transform, false);
        pref.SetPosition(new Vector2(Screen.width * 2 / 3, Screen.height / 2));
        pref.SetValue(value);
        pref.SetPassiveColor();
    }
}
