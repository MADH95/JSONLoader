using System.Collections;
using DiskCardGame;
using JLPlugin.Data;

public class ConfigilSpecialAbilityLogic : ABaseConfigilLogic
{
    public override object ability => specialTriggeredAbility;
    public override PlayableCard PlayableCard => specialCardBehaviour.GetComponent<PlayableCard>();
    public override Card Card => specialCardBehaviour.Card;

    private SpecialCardBehaviour specialCardBehaviour;
    private SpecialTriggeredAbility specialTriggeredAbility;
    
    public ConfigilSpecialAbilityLogic(SpecialCardBehaviour triggerReceiver, SigilData sigilData,
        SpecialTriggeredAbility ability) : base(sigilData)
    {
        specialCardBehaviour = triggerReceiver;
        specialTriggeredAbility = ability;
    }

    public override IEnumerator LearnAbility(float startDelay = 0.0f)
    {
        yield break; // not supported
    }
}