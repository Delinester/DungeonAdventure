using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponUI : MonoBehaviour
{
    public GameObject weaponUIPrefab;

    protected GameObject worldCanvas;

    protected TextMeshProUGUI[] textsUGUIs;
    protected GameObject weaponUI;
    protected WeaponDataStorage weaponData;
    protected GameObject player;

    protected bool isUIActive;
    // Start is called before the first frame update
    void Awake()
    {
        worldCanvas = GameObject.Find("WorldUI");
        player = GameObject.Find("PlayerCharacter");
        weaponData = GetComponent<WeaponDataStorage>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isUIActive)
        {
            UpdateTransform();            
        }        
    }

    public virtual void ShowUI()
    {
        if (isUIActive == false && weaponData.attachment == WeaponDataStorage.WeaponAttachment.Nobody)
        {
            weaponUI = Instantiate(weaponUIPrefab, worldCanvas.transform);
            textsUGUIs = weaponUI.GetComponentsInChildren<TextMeshProUGUI>();

            textsUGUIs[0].text = weaponData.GetName();
            textsUGUIs[1].text = "Damage:" + weaponData.GetDamage().ToString();
            textsUGUIs[2].text = "Stamina Cost: " + weaponData.GetStaminaCost().ToString();
            textsUGUIs[3].text = "Cost: " + weaponData.GetCost().ToString();


            isUIActive = true;
        }
    }

    public void DisableUI()
    {
        if (isUIActive)
        {
            isUIActive = false;
            Destroy(weaponUI);
        }
    }

    protected void UpdateTransform()
    {        
        weaponUI.transform.position = transform.position + new Vector3(0, 2, 2);
        weaponUI.GetComponent<RectTransform>().LookAt(Camera.main.transform);
        weaponUI.GetComponent<RectTransform>().Rotate(0, 180, 0);
    }
}
