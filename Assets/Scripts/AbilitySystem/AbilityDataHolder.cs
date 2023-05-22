using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityDataHolder : ScriptableObject
{
    public float Cooldown;
    [Header("Set this to -1 if instant")]
    public float Duration;
    public KeyCode ActivatingKey;
    public bool clickAndHoldAbility = false; 
    public Vector3 keyPressPos;
    public Vector3 keyReleasePos;
    public Camera mainCamera;
    public bool doesHaveResources;

    protected AbilityBase.AbilityState currentState;
    protected AbilityComponent holderAbilityComponent;

    public virtual void OnStart(GameObject go) {
        holderAbilityComponent = go.GetComponent<AbilityComponent>();
        mainCamera = GameManager.Instance.mainCamera;
    }

    #region OnStayState
    public virtual void OnAbilityReadyStay(GameObject go) {
        currentState = AbilityBase.AbilityState.Ready;

    }


    public virtual void OnAbilityCurrentlyActiveStay(GameObject go) {
        currentState = AbilityBase.AbilityState.CurrentlyActive;
    }


    public virtual void OnAbilityCooldownStay(GameObject go) {
        currentState = AbilityBase.AbilityState.Cooldown;
    }
    #endregion

    #region OnEnterState

    public virtual void CheckResources(GameObject go) {

    } 
    public virtual void OnAbilityReady(GameObject go) {
        currentState = AbilityBase.AbilityState.Ready;
    }

    public virtual void OnAbilityActivated(GameObject go) {
        currentState = AbilityBase.AbilityState.Activated;
        
    }

    public virtual void OnAbilityCurrentlyActive(GameObject go) {
        currentState = AbilityBase.AbilityState.CurrentlyActive;
    }

    public virtual void OnAbilityFinish(GameObject go) {
        currentState = AbilityBase.AbilityState.Finish;
        
    }

    public virtual void OnAbilityCooldown(GameObject go) {
        currentState = AbilityBase.AbilityState.Cooldown;
    }

    #endregion
}
