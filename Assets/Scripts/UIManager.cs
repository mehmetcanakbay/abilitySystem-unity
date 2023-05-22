using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Slider StaminaSlider;
    public Slider AbilitySlider;

    private AbilityComponent playerAbilityComponent;
    private bool startInitialize = false;
    // Start is called before the first frame update

    void Start()
    {
        playerAbilityComponent = GameManager.Instance.Player.GetComponent<AbilityComponent>();
        if (startInitialize) {
            PlayerAttributes.Instance.StaminaChangeEvent += UpdateStaminaSlider;
            playerAbilityComponent.AbilityBases[0].OnCooldownEvent += UpdateAbilitySlider;
        }
    }

    private void OnEnable() {
        if (PlayerAttributes.Instance == null) {
            startInitialize = true;
            return;
        }
        PlayerAttributes.Instance.StaminaChangeEvent += UpdateStaminaSlider;
        playerAbilityComponent.AbilityBases[0].OnCooldownEvent += UpdateAbilitySlider;
    }

    private void OnDisable() {
        PlayerAttributes.Instance.StaminaChangeEvent -= UpdateStaminaSlider;
        playerAbilityComponent.AbilityBases[0].OnCooldownEvent -= UpdateAbilitySlider;
    }

    void UpdateStaminaSlider() {
        StaminaSlider.value = PlayerAttributes.Instance.GetCurrentDashStamina()/PlayerAttributes.Instance.MaxDashStamina;
    }

    void UpdateAbilitySlider(float ratio) {
        // 1->0 to 0->1
        float changeNumPos = Mathf.Lerp(1.0f, 0.0f, ratio);
        AbilitySlider.value = changeNumPos;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
