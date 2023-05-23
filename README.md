# Lightweight Ability System for Unity

Lightweight ability system for unity. Easily customizable and provides a good base for game projects that use abilities.

**Features**
 
 - Easily add abilities by adding the ability to the component
 - Write abilities and ability logic without worrying about stuff like cooldowns, which makes the code more maintainable and readable
  
## Getting Started

There are 3 scripts, -AbilityBase, AbilityComponent and  AbilityDataHolder- and these work together to make the system tick.

In short, AbilityBase is the script where the ability logic like cooldowns, what happens when its ready, what happens when its activated etc. happens. This is the system's "logic"

AbilityDataHolder is a ScriptableObject, and with overrideable functions, you can write your own ability logic, to then add to AbilityComponent. You can think of this as "logic data"

AbilityComponent holds creates and holds AbilityBases, basically its the initializer and maintainer.

-----
Give a game object AbilityComponent component, then create a script, for this example, I'll call it AbilityDash.cs. This should inherit from AbilityDataHolder. Since **this is a scriptable object**, delete start and update functions, since they wont work.

After inheriting the AbilityDataHolder, you can basically override and write logic for every state the ability goes in. These states are 

 - Ready
 - CheckResources
 - Activated
 - CurrentlyActive
 - Finish
 - Cooldown

For this example, we will only use CheckResources, and Activated.

Override the CheckResources and OnAbilityActivated functions. Right now the script looks like this:


```c#
public class AbilityDash : AbilityDataHolder
{
    public override void CheckResources(GameObject go) {
        base.CheckResources(go);
    }

    public override void OnAbilityActivated(GameObject go) {
        base.OnAbilityActivated(go);
    }
}
```

In the check resources function, check the resource, and remember, you have to make doesHaveResources flag true if it does have the resources, if it doesnt, then set this flag to false.

```c#
public override void CheckResources(GameObject go) {
    base.CheckResources(go);
    float currStamina = //Get the stamina;
    if (currStamina >= dashStaminaNeedAmount) {
        doesHaveResources = true;
    } else {
        doesHaveResources = false;
    }
}
```

After this you can override as many functions as you want to create your own ability. After that, dont forget to add "Create Asset Menu" at the top, which will let you create the SO object itself in editor. This is the final script used in this example project:


```c#
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
```

After these steps, simply create the Scriptable Object, then append this to the Abilities part of the AbilityComponent. Then set the settings in the scriptable object, press play and you'll see that your ability is working perfectly without problems!

## Last Words
---
You can also clone this repository, open the example project and test things out for yourself. (2020.3.3f1 LTS) 

In this repository, I added the ability, and bound stuff like cooldowns to UI. You can get an idea of how all things work by looking at this project.

One thing to consider is that this is extremely customizable and easy to create more functionality. For example, for this project, I have added a OnCooldownEvent and called this when the ability is on cooldown, and then used this event to update the UI. 

You can also create other events for other states, e.g. when it becomes Ready, you can then send an event, bind that to an UI object and then make the UI sprite pop for a second to let the player know that the ability is ready.

