using DiskCardGame;
using JLPlugin.SigilCode;
using ItemData = JLPlugin.Data.ItemData;

public class ConfigConsumableItemLogic : ABaseConfigilLogic
{
    public override object ability => "ConsumableItem";
    public override object Instance => consumableItem;
    public override PlayableCard PlayableCard => null;
    public override Card Card => null;

    private readonly ConfigurableConsumableItem consumableItem;
    
    public ConfigConsumableItemLogic(ConfigurableConsumableItem triggerReceiver, ItemData data) : base(data)
    {
        consumableItem = triggerReceiver;
    }
}