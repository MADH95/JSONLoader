using System.Collections;
using DiskCardGame;
using JLPlugin.Data;

public class ConfigilAbilityLogic : ABaseConfigilLogic
{
    public override object ability => activatedAbilityBehaviour.Ability;
    public override object Instance => activatedAbilityBehaviour;
    public override PlayableCard PlayableCard => activatedAbilityBehaviour.GetComponent<PlayableCard>();
    public override Card Card => activatedAbilityBehaviour.Card;

    private readonly ActivatedAbilityBehaviour activatedAbilityBehaviour;
    
    public ConfigilAbilityLogic(ActivatedAbilityBehaviour triggerReceiver, SigilData sigilData) : base(sigilData)
    {
        activatedAbilityBehaviour = triggerReceiver;
    }

    public override IEnumerator LearnAbility(float startDelay = 0.0f)
    {
        yield return activatedAbilityBehaviour.LearnAbility(startDelay);
    }
}