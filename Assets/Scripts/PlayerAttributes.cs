using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttributes : MonoBehaviour
{
    public static PlayerAttributes Instance;
    public delegate void NoArgumentSignature();

    public event NoArgumentSignature StaminaChangeEvent;
    public float MaxDashStamina = 0.0f;
    private float currentDashStamina = 0.0f;
    public float staminaRefillSpeed = 10.0f;

    //On enable runs before Awake
    private void OnEnable() {
        Instance = this;
    }
    private void Awake() {
    }

    void Start()
    {
        SetDashStamina(MaxDashStamina);
    }

    // Update is called once per frame
    void Update()
    {
        SetDashStamina(GetCurrentDashStamina()+Time.deltaTime*staminaRefillSpeed);
    }

    public void SetDashStamina(float val) {
        currentDashStamina = Mathf.Clamp(val, 0.0f, MaxDashStamina);
        if (StaminaChangeEvent != null)
            StaminaChangeEvent.Invoke();
    }

    public float GetCurrentDashStamina() {
        return currentDashStamina;
    }
}
