using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShieldUI : WeaponUI
{
    public override void ShowUI()
    {
        if (isUIActive == false && weaponData.attachment == WeaponDataStorage.WeaponAttachment.Nobody)
        {
            weaponUI = Instantiate(weaponUIPrefab, worldCanvas.transform);
            textsUGUIs = weaponUI.GetComponentsInChildren<TextMeshProUGUI>();

            textsUGUIs[0].text = weaponData.GetName();
            textsUGUIs[1].text = "Damage Block:" + weaponData.GetDamageBlock().ToString() + "%";
            textsUGUIs[2].text = "Stamina Cost: " + weaponData.GetStaminaCost().ToString();
            textsUGUIs[3].text = "Cost: " + weaponData.GetCost().ToString();


            isUIActive = true;
        }
    }
}
