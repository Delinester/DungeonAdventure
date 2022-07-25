using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{    
    public GameObject healthSlider;
    public TextMeshProUGUI healthText;
    public GameObject staminaSlider;
    public TextMeshProUGUI staminaText;

    public GameObject shieldIconObject;
    private Image shieldIcon;
    private float shieldIconOpacity = 0.25f;

    public TextMeshProUGUI coinsAmountText;

    private PlayerController playerController;

    private Color staminaOriginalTextColor;
    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        staminaOriginalTextColor = staminaText.color;
        shieldIcon = shieldIconObject.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHealth();
        UpdateStamina();
        UpdateCoinsAmount();
        ShieldIconSwitcher();
    }

    public void UpdateHealth()
    {
        healthText.text = playerController.GetCurrentHealthPoints().ToString();
        healthSlider.GetComponent<Slider>().value = (float)playerController.GetCurrentHealthPoints() / 100;
    }
    public void UpdateStamina()
    {
        staminaText.text = Mathf.Round(playerController.GetCurrentStaminaPoints()).ToString();
        staminaSlider.GetComponent<Slider>().value = playerController.GetCurrentStaminaPoints() / playerController.GetMaxStaminaPoints();

        if (playerController.IsInCooldownState()) staminaText.color = Color.red;
        else staminaText.color = staminaOriginalTextColor;
    }

    public void UpdateCoinsAmount()
    {
        coinsAmountText.text = playerController.GetMoneyAmount().ToString();
    }
    public void ShieldIconSwitcher()
    {
        if (playerController.isDefending)
        {            
            shieldIcon.color = new Color(shieldIcon.color.r, shieldIcon.color.g, shieldIcon.color.b, 1);
        }
        else
        {           
            shieldIcon.color = new Color(shieldIcon.color.r, shieldIcon.color.g, shieldIcon.color.b, shieldIconOpacity);
        }
    }
}
