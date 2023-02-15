using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponUI : MonoBehaviour
{
    public TMPro.TextMeshProUGUI bulletText;
    private void OnEnable()
    {
        PlayerWeapons.onBulletCountChange += UpdateUI;
    }
    private void OnDisable()
    {
        PlayerWeapons.onBulletCountChange -= UpdateUI;
    }
    void UpdateUI(int bulletCount, int magazineSize)
    {
        string text = bulletCount.ToString() + "/" + magazineSize.ToString();
        bulletText.text = text;
    }
}
