using System.Collections.Generic;

using DiskCardGame;

using APIPlugin;

using TinyJson;

namespace JLPlugin.Data
{
    using Utils;

    public partial class CardData
    {
        public static CardData CreateFromJSON( string jsonString )
            => JSONParser.FromJson<CardData>( jsonString );

        public void GenerateNew()
        {
            ErrorUtil.Card = this.name;
            ErrorUtil.Message = "{0} - {2} is an invalid value for {1}";

            if (this.baseHealth == 0)
            {
                this.baseHealth = 1;
            }

            List<CardMetaCategory> metaCategories = JLUtils.Assign( this.metaCategories, nameof( this.metaCategories ), Dicts.MetaCategory );

            if (this.abilities is not null && ( this.abilities.Count == 0 || this.abilities[ 0 ] == "None" ) )
                this.abilities = null;

            NewCard.Add(
                name:                   this.name,
                displayedName:          this.displayedName ?? "",
                description:            this.description ?? "",

                baseAttack:             this.baseAttack,
                baseHealth:             this.baseHealth,
                hideAttackAndHealth:    this.hideAttackAndHealth,
                
                metaCategories:         metaCategories ?? new(),
                onePerDeck:             this.onePerDeck,
                flipPortraitForStrafe:  this.flipPortraitForStrafe,
                defaultEvolutionName:   this.defaultEvolutionName,
                
                cost:                   this.cost,
                bonesCost:              this.bonesCost,
                energyCost:             this.energyCost,
                gemsCost:               JLUtils.Assign( this.gemsColour,          nameof( this.gemsColour ),          Dicts.GemColour           ),
                                                                                                                      
                cardComplexity:         JLUtils.Assign( this.cardComplexity,      nameof( this.cardComplexity ),      Dicts.Complexity          ),
                temple:                 JLUtils.Assign( this.temple,              nameof( this.metaCategories ),      Dicts.Temple              ),
                tribes:                 JLUtils.Assign( this.tribes,              nameof( this.tribes ),              Dicts.Tribes              ),
                                                                                                                      
                abilities:              JLUtils.Assign( this.abilities,           nameof( this.abilities ),           Dicts.Abilities           ),
                                                                                                                      
                traits:                 JLUtils.Assign( this.traits,              nameof( this.traits ),              Dicts.Traits              ),
                specialAbilities:       JLUtils.Assign( this.specialAbilities,    nameof( this.specialAbilities ),    Dicts.SpecialAbilities    ),
                specialStatIcon:        JLUtils.Assign( this.specialStatIcon,     nameof( this.specialStatIcon ),     Dicts.StatIcon            ),
                                                                                                                      
                appearanceBehaviour:    JLUtils.Assign( this.appearanceBehaviour, nameof( this.appearanceBehaviour ), Dicts.AppearanceBehaviour ),

                tex:                    JLUtils.Assign( this.texture,             nameof( this.texture      ) ),
                altTex:                 JLUtils.Assign( this.altTexture,          nameof( this.altTexture   ) ),
                titleGraphic:           JLUtils.Assign( this.titleGraphic,        nameof( this.titleGraphic ) ),
                pixelTex:               JLUtils.Assign( this.pixelTexture,        nameof( this.pixelTexture ) ),
                decals:                 JLUtils.Assign( this.decals,              nameof( this.decals       ) ),
                animatedPortrait:       null, //TODO: implement       
                
                evolveId:               JLUtils.GenerateEvolveIdentifier( this ),
                tailId:                 JLUtils.GenerateTailIdentifier( this ),
                iceCubeId:              JLUtils.GenerateIceCubeIdentifier( this ),
                abilityIds:             JLUtils.GenerateAbilityIdentifiers( this.customAbilities )
            );

            ErrorUtil.Clear();
        }

        public void Edit()
        {
            ErrorUtil.Card = this.name;
            ErrorUtil.Message = "{0} - Can't change {1} to {2}";

            JLUtils.CheckValidFields( this.fieldsToEdit );

            bool check( string fieldName ) => this.fieldsToEdit.Contains( fieldName );

            bool hasCustomAbilities = check( nameof( customAbilities ) );
            bool hasEvolveData = check( nameof( this.evolution ) ) || check( nameof( this.evolve_evolutionName ) );
            bool hasTailData = check( nameof( this.tail ) ) || check( nameof( this.tail_cardName ) );
            bool hasIceCubeData = check( nameof( this.iceCube ) ) || check( nameof( this.iceCube_creatureWithin ) );

            var _ = new CustomCard(
                name: this.name,
                abilityId: hasCustomAbilities ? JLUtils.GenerateAbilityIdentifiers( this.customAbilities ) : null,
                evolveId:  hasEvolveData      ? JLUtils.GenerateEvolveIdentifier( this )                   : null,
                tailId:    hasTailData        ? JLUtils.GenerateTailIdentifier( this )                     : null,
                iceCubeId: hasIceCubeData     ? JLUtils.GenerateIceCubeIdentifier( this )                  : null
                )
            {
                displayedName         = check( nameof( this.displayedName ) )         ? this.displayedName         : null,
                description           = check( nameof( this.description ) )           ? this.description           : null,
                                      
                baseAttack            = check( nameof( this.baseAttack ) )            ? this.baseAttack            : null,
                baseHealth            = check( nameof( this.baseHealth ) )            ? this.baseHealth            : null,
                hideAttackAndHealth   = check( nameof( this.hideAttackAndHealth ) )   ? this.hideAttackAndHealth   : null,
                                                                                        
                onePerDeck            = check( nameof( this.onePerDeck ) )            ? this.onePerDeck            : null,
                flipPortraitForStrafe = check( nameof( this.flipPortraitForStrafe ) ) ? this.flipPortraitForStrafe : null,
                defaultEvolutionName  = check( nameof( this.defaultEvolutionName ) )  ? this.defaultEvolutionName  : null,
                                                                                        
                cost                  = check( nameof( this.cost ) )                  ? this.cost                  : null,
                bonesCost             = check( nameof( this.bonesCost ) )             ? this.bonesCost             : null,
                energyCost            = check( nameof( this.energyCost ) )            ? this.energyCost            : null,
                gemsCost              = check( nameof( this.gemsColour ) ) ?
                                            JLUtils.Assign( this.gemsColour,            nameof( this.gemsColour ),            Dicts.GemColour )             : null,
                                                                                                      
                metaCategories        = check( nameof( this.metaCategories ) )        ?
                                            JLUtils.Assign( this.metaCategories,         nameof( this.metaCategories ),       Dicts.MetaCategory )          : null,
                cardComplexity        = check( nameof( this.cardComplexity ) )        ?                                                                            
                                            JLUtils.Assign( this.cardComplexity,         nameof( this.cardComplexity ),       Dicts.Complexity )            : null,
                temple                = check( nameof( this.temple ) )                ?                                                                                    
                                            JLUtils.Assign( this.temple,                 nameof( this.temple ),               Dicts.Temple )                : null,
                tribes                = check( nameof( this.tribes ) )                ?                                                                                    
                                            JLUtils.Assign( this.tribes,                 nameof( this.tribes ),               Dicts.Tribes )                : null,
                                                                                                                                                              
                abilities             = check( nameof( this.abilities ) )             ?                                                                                 
                                            JLUtils.Assign( this.abilities,              nameof( this.abilities ),            Dicts.Abilities)              : null,
                traits                = check( nameof( this.traits ) )                ?                                                                                    
                                            JLUtils.Assign( this.traits,                 nameof( this.traits ),               Dicts.Traits )                : null,
                specialAbilities      = check( nameof( this.specialAbilities ) )      ?
                                            JLUtils.Assign( this.specialAbilities,       nameof( this.specialAbilities ),     Dicts.SpecialAbilities )      : null,
                specialStatIcon       = check( nameof( this.specialStatIcon ) )       ?
                                            JLUtils.Assign( this.specialStatIcon,        nameof( this.specialStatIcon ),      Dicts.StatIcon )              : null,
                                                                                                        
                appearanceBehaviour   = check( nameof( this.appearanceBehaviour ) )   ?
                                            JLUtils.Assign( this.appearanceBehaviour,    nameof( this.appearanceBehaviour ),  Dicts.AppearanceBehaviour )   : null,
                                                                                                      
                tex                   = check( nameof( this.texture ) )        ? JLUtils.Assign( this.texture,                nameof( this.texture ) )      : null,
                altTex                = check( nameof( this.altTexture ) )     ? JLUtils.Assign( this.altTexture,             nameof( this.altTexture ) )   : null,
                pixelTex              = check( nameof( this.pixelTexture ) )   ? JLUtils.Assign( this.pixelTexture,           nameof( this.pixelTexture ) ) : null,
                titleGraphic          = check( nameof( this.titleGraphic ) )   ? JLUtils.Assign( this.titleGraphic,           nameof( this.titleGraphic ) ) : null,
                animatedPortrait      = null, //TODO: animatedPortrait                                                                                          
                decals                = check( nameof( this.decals ) )         ? JLUtils.Assign( this.decals,                 nameof( this.decals ) )       : null
            };

            ErrorUtil.Clear();
        }
    }
}
