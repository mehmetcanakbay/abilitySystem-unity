using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityBase : MonoBehaviour
{

    //Get the ability. Overridable functions come from here.
    //You do the logic in this AbilityDataHolder scriptable object.
    public AbilityDataHolder AbilityData;

    public delegate void SingleFloatArgumentSignature(float arg1);
    //You could create several of these for other states if you want to.
    public event SingleFloatArgumentSignature OnCooldownEvent;

    //These are just timers
    public float _internal_cooldowntime;
    public float _internal_durationtime;



    public float ActivateHoldBufferTime = 2.0f;
    private float _internal_activateKeyBufferTime;
    private bool pressedButton = false;

    //Ability States
    //Important: 
    //Ready, CurrentlyActive and Cooldown states have STAY overridable functions.
    //So you can override these functions and do stuff while they are ready, active, and on cooldown.
    //Activated and Finish states are on-off states. They fire off, and immediately switch to the next state.
    //So they do NOT have stay overridable functions
    public enum AbilityState {
        Ready, //
        CheckResources,
        Activated,
        CurrentlyActive,
        Finish,
        Cooldown
    }

    private AbilityState CurrentState;

    //getter setter, dont touch CurrentState, use this property
    public AbilityState StateProperty {
        get {return CurrentState;}
        set {
            if (value != CurrentState) {
                CurrentState = value;
                _INTERNAL_EnumChange(value);
            }
        }
    }

    //on enum change, fire off this event depending on the value
    void _INTERNAL_EnumChange(AbilityState newValue) {
        switch (newValue) {
            case AbilityState.Ready:
                AbilityData.OnAbilityReady(this.gameObject);
                break;

            case AbilityState.CheckResources:
                AbilityData.CheckResources(this.gameObject);
                break;

            case AbilityState.Activated:
                AbilityData.OnAbilityActivated(this.gameObject);
                StateProperty = AbilityState.CurrentlyActive;
                break;

            case AbilityState.CurrentlyActive:
                AbilityData.OnAbilityCurrentlyActive(this.gameObject);
                break;

            case AbilityState.Finish:
                AbilityData.OnAbilityFinish(this.gameObject);
                StateProperty = AbilityState.Cooldown;
                break;
            
            case AbilityState.Cooldown:
                AbilityData.OnAbilityCooldown(this.gameObject);
                break;
        }
    }

    //Event bindings happen here
    private void OnEnable() {
    }

    private void OnDisable() {
    }

    //Initialization
    void Start()
    {
        StateProperty = AbilityState.Ready;
    }


    void Update()
    {
        switch (StateProperty) {
            //WHILE READY:
            //IF INPUT KEY DOWN,
            //SWITCH STATE, START DURATION TIME
            //IF NOT CONTINUE FIRING OFF READY STAY FUNCTION
            case AbilityState.Ready:
                if (Input.GetKeyDown(AbilityData.ActivatingKey)) {
                    // if its a click and hold ability, you press the button
                    //activates the bool
                    if (AbilityData.clickAndHoldAbility && pressedButton == false) {
                        AbilityData.OnAbilityReadyStay(this.gameObject);
                        pressedButton = true;
                        _internal_activateKeyBufferTime = ActivateHoldBufferTime;
                        AbilityData.keyPressPos = AbilityData.mainCamera.ScreenToWorldPoint(Input.mousePosition);
                    } else {
                        StateProperty = AbilityState.CheckResources;
                        _internal_durationtime = AbilityData.Duration;
                    }
                } else {
                    AbilityData.OnAbilityReadyStay(this.gameObject);
                }     


                //if the bool is active, and you get the release command continue
                if (AbilityData.clickAndHoldAbility && pressedButton == true) { 
                    if (Input.GetKeyUp(AbilityData.ActivatingKey)) {
                        AbilityData.OnAbilityReadyStay(this.gameObject);
                        pressedButton = false;
                        AbilityData.keyReleasePos = AbilityData.mainCamera.ScreenToWorldPoint(Input.mousePosition);
                        _internal_activateKeyBufferTime = ActivateHoldBufferTime;
                        _internal_durationtime = AbilityData.Duration;
                        StateProperty = AbilityState.CheckResources;
                    } 

                    else {
                        AbilityData.OnAbilityReadyStay(this.gameObject);
                        _internal_activateKeyBufferTime -= Time.deltaTime;
                        if (_internal_activateKeyBufferTime <= 0.0f) {
                            pressedButton = false;
                        }
                    }
                } 
                    // StateProperty = AbilityState.Activated;
                    // _internal_durationtime = AbilityData.Duration;

                break;
            
            //WHILE CURRENTLY ACTIVE,
            //IF DURATION IS NEGATIVE, THAT MEANS ITS INSTANT. GO TO THE NEXT STATE
            //IF DURATION IS BIGGER THAN 0, CONTINUE FIRING OFF STAY FUNCTION AND SUBTRACT FROM TIME
            //WHEN THE DURATION IS <= 0, GO TO THE NEXT STATE, AND START THE COOLDOWN TIMER

            case AbilityState.CheckResources:
                if (AbilityData.doesHaveResources) {
                    StateProperty = AbilityState.Activated;
                    _internal_durationtime = AbilityData.Duration;
                } else {
                    StateProperty = AbilityState.Ready;
                }
                break;

            case AbilityState.CurrentlyActive:
                //if duration is INSTANT (-1)
                //OnAbilityCurrentlyActiveStay doesnt fire, and ONLY OnAbilityCurrentlyActive fires.
                if (_internal_durationtime == -1) {
                    StateProperty = AbilityState.Finish;
                    _internal_cooldowntime = AbilityData.Cooldown;
                    break;
                }
                if (_internal_durationtime > 0) {
                    AbilityData.OnAbilityCurrentlyActiveStay(this.gameObject);
                    _internal_durationtime -= Time.deltaTime;
                } else {
                    StateProperty = AbilityState.Finish;
                    _internal_cooldowntime = AbilityData.Cooldown;
                }
                break;

            
            //WHILE ON COOLDOWN,
            //IF COOLDOWN TIME IS HIGHER THAN 0,
            //CONTINUE FIRING OFF COOLDOWN STAY FUNCTION, SUBTRACT FROM REMAINING CD TIME
            //WHEN CD HITS <= 0, GO TO THE NEXT STATE
            case AbilityState.Cooldown:
                if (_internal_cooldowntime > 0) {
                    AbilityData.OnAbilityCooldownStay(this.gameObject);
                    _internal_cooldowntime -= Time.deltaTime;
                    float ratio = Mathf.Clamp(_internal_cooldowntime, 0, AbilityData.Cooldown) / AbilityData.Cooldown;
                    OnCooldownEvent?.Invoke(ratio); //1 to zero
                } else {
                    StateProperty = AbilityState.Ready;
                }
                break;
        }
        
    }

    //For example, you could call this after you change the ability data, so that the newly appointed
    //ability can be fired off immediately. There's an example of this in AbilityComponent

    public void ResetInternalState() {
        pressedButton = false;
        CurrentState = AbilityState.Ready;
        _internal_activateKeyBufferTime = ActivateHoldBufferTime;
        _internal_cooldowntime = AbilityData.Cooldown;
        _internal_durationtime = AbilityData.Duration;
    }

    public void ChangeAbilityData(AbilityDataHolder newAbility) {
        _internal_cooldowntime = newAbility.Cooldown;
        _internal_durationtime = AbilityData.Duration;
        CurrentState = AbilityState.Ready;
        AbilityData = newAbility;
    }

}
