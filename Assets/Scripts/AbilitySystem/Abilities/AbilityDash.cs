using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/AbilityDash")]
public class AbilityDash : AbilityDataHolder
{
    [Header("ThisAbilitySettings")]
    public float dashStaminaNeedAmount = 10.0f; 
    public float distAmount = 50.0f; 
    public override void CheckResources(GameObject go)
    {
        base.CheckResources(go);
        float currStamina = PlayerAttributes.Instance.GetCurrentDashStamina();
        if (currStamina >= dashStaminaNeedAmount) {
            doesHaveResources = true;
        } else {
            doesHaveResources = false;
        }
    }

    public override void OnAbilityActivated(GameObject go)
    {
        base.OnAbilityActivated(go);
        Rigidbody rb = go.GetComponent<Rigidbody>();
        PlayerMovement movementComponent = go.GetComponent<PlayerMovement>();
        if (rb == null) return;

        rb.AddForce(movementComponent.moveDirection*distAmount, ForceMode.Impulse);

        ConfirmResourceExpense();
    }

    void ConfirmResourceExpense() {
        PlayerAttributes.Instance.SetDashStamina(
            PlayerAttributes.Instance.GetCurrentDashStamina()-
            dashStaminaNeedAmount
        );
    }
}
