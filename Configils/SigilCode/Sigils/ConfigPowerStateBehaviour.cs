using System.Collections;
using DiskCardGame;
using JLPlugin.Data;

public class ConfigPowerStateBehaviour : ABaseConfigilLogic
{
    public override object ability => id;
    public override PlayableCard PlayableCard => specialCardBehaviour.GetComponent<PlayableCard>();
    public override Card Card => specialCardBehaviour.Card;

    private SpecialCardBehaviour specialCardBehaviour;
    private SpecialStatIcon id;
    
    public ConfigPowerStateBehaviour(SpecialCardBehaviour triggerReceiver, SigilData sigilData,
        SpecialStatIcon ability) : base(sigilData)
    {
        specialCardBehaviour = triggerReceiver;
        id = ability;
    }

    public override IEnumerator LearnAbility(float startDelay = 0.0f)
    {
        yield break; // not supported
    }
}