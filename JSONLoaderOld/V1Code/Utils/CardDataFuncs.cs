using System.Collections.Generic;

using DiskCardGame;

using APIPlugin;

namespace JLPlugin.Data
{
    using System;
    using Utils;

    [Obsolete]
    public partial class CardData
    {
        public void GenerateNew()
        {
            ErrorUtil.Card = this.name;
            ErrorUtil.Message = "{0} - {2} is an invalid value for {1}";

            if (this.baseHealth == 0)
                this.baseHealth = 1;

            List<CardMetaCategory> metaCategories = CDUtils.Assign( this.metaCategories, nameof( this.metaCategories ), Dicts.MetaCategory );

            if (this.abilities is not null && ( this.abilities.Count == 0 || this.abilities[ 0 ] == "None" ) )
                this.abilities = null;

            NewCard.Add(
                name:                       this.name,
                displayedName:              this.displayedName ?? "",
                description:                this.description ?? "",

                baseAttack:                 this.baseAttack,
                baseHealth:                 this.baseHealth,
                hideAttackAndHealth:        this.hideAttackAndHealth,

                metaCategories:             metaCategories ?? new(),
                onePerDeck:                 this.onePerDeck,
                flipPortraitForStrafe:      this.flipPortraitForStrafe,
                defaultEvolutionName:       this.defaultEvolutionName,

                bloodCost:                  this.bloodCost,
                bonesCost:                  this.bonesCost,
                energyCost:                 this.energyCost,
                gemsCost:                   CDUtils.Assign( this.gemsColour,            nameof( this.gemsColour ),              Dicts.GemColour           ),
                
                cardComplexity:             CDUtils.Assign( this.cardComplexity,        nameof( this.cardComplexity ),          Dicts.Complexity          ),
                temple:                     CDUtils.Assign( this.temple,                nameof( this.temple ),                  Dicts.Temple              ),
                tribes:                     CDUtils.Assign( this.tribes,                nameof( this.tribes ),                  Dicts.Tribes              ),
                                            
                abilities:                  CDUtils.Assign( this.abilities,             nameof( this.abilities ),               Dicts.Abilities           ),
                                            
                traits:                     CDUtils.Assign( this.traits,                nameof( this.traits ),                  Dicts.Traits              ),
                specialAbilities:           CDUtils.Assign( this.specialAbilities,      nameof( this.specialAbilities ),        Dicts.SpecialAbilities    ),
                specialStatIcon:            CDUtils.Assign( this.specialStatIcon,       nameof( this.specialStatIcon ),         Dicts.StatIcon            ),
                                            
                appearanceBehaviour:        CDUtils.Assign( this.appearanceBehaviour,   nameof( this.appearanceBehaviour ),     Dicts.AppearanceBehaviour ),
                                            
                defaultTex:                 CDUtils.Assign( this.texture,               nameof( this.texture )         ),
                altTex:                     CDUtils.Assign( this.altTexture,            nameof( this.altTexture )      ),
                emissionTex:                CDUtils.Assign( this.emissionTexture,       nameof( this.emissionTexture ) ),
                titleGraphic:               CDUtils.Assign( this.titleGraphic,          nameof( this.titleGraphic )    ),
                pixelTex:                   CDUtils.Assign( this.pixelTexture,          nameof( this.pixelTexture )    ),
                decals: CDUtils.Assign( this.decals,                nameof( this.decals )          ),
                animatedPortrait:           null, //TODO: implement       

                evolveId:                   IDUtils.GenerateEvolveIdentifier( this ),
                tailId:                     IDUtils.GenerateTailIdentifier( this ),
                iceCubeId:                  IDUtils.GenerateIceCubeIdentifier( this ),
                abilityIdsParam:            IDUtils.GenerateAbilityIdentifiers( this.customAbilities ),
                specialAbilitiesIdsParam:   IDUtils.GenerateSpecialAbilityIdentifiers( this.customSpecialAbilities )
            )
            ;

            ErrorUtil.Clear();
        }

        public void Edit()
        {
            ErrorUtil.Card = this.name;
            ErrorUtil.Message = "{0} - Can't change {1} to {2}";

            CDUtils.CheckValidFields( this.fieldsToEdit );

            bool check( string fieldName ) => this.fieldsToEdit.Contains( fieldName );

            bool hasCustomAbilities         = check( nameof( this.customAbilities ) );
            bool hasCustomSpecialAbilities  = check( nameof( this.customSpecialAbilities ) );

            bool hasEvolveData              = check( nameof( this.evolution ) );
            bool hasTailData                = check( nameof( this.tail ) );
            bool hasIceCubeData             = check( nameof( this.iceCube ) );

            var _ = new CustomCard(
                name: this.name,
                abilityIdParam:         hasCustomAbilities          ? IDUtils.GenerateAbilityIdentifiers( this.customAbilities )                : null,
                specialAbilityIdParam:  hasCustomSpecialAbilities   ? IDUtils.GenerateSpecialAbilityIdentifiers( this.customSpecialAbilities )  : null,
                evolveId:               hasEvolveData               ? IDUtils.GenerateEvolveIdentifier( this )                                  : null,
                tailId:                 hasTailData                 ? IDUtils.GenerateTailIdentifier( this )                                    : null,
                iceCubeId:              hasIceCubeData              ? IDUtils.GenerateIceCubeIdentifier( this )                                 : null
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
                                                                                        
                cost                  = check( nameof( this.bloodCost  ) )            ? this.bloodCost             : null,
                bonesCost             = check( nameof( this.bonesCost  ) )            ? this.bonesCost             : null,
                energyCost            = check( nameof( this.energyCost ) )            ? this.energyCost            : null,
                gemsCost              = check( nameof( this.gemsColour ) )            ?
                                            CDUtils.Assign( this.gemsColour,              nameof( this.gemsColour ),           Dicts.GemColour )                : null,
                                                                                                      
                metaCategories        = check( nameof( this.metaCategories ) )        ?
                                            CDUtils.Assign( this.metaCategories,          nameof( this.metaCategories ),       Dicts.MetaCategory )             : null,
                cardComplexity        = check( nameof( this.cardComplexity ) )        ?
                                            CDUtils.Assign( this.cardComplexity,          nameof( this.cardComplexity ),       Dicts.Complexity )               : null,
                temple                = check( nameof( this.temple ) )                ?
                                            CDUtils.Assign( this.temple,                  nameof( this.temple ),               Dicts.Temple )                   : null,
                tribes                = check( nameof( this.tribes ) )                ?                                                                                       
                                            CDUtils.Assign( this.tribes,                  nameof( this.tribes ),               Dicts.Tribes )                   : null,
                                                                                                                                                                 
                abilities             = check( nameof( this.abilities ) )             ?                                                                                    
                                            CDUtils.Assign( this.abilities,               nameof( this.abilities ),            Dicts.Abilities)                 : null,
                traits                = check( nameof( this.traits ) )                ?                                                                                       
                                            CDUtils.Assign( this.traits,                  nameof( this.traits ),               Dicts.Traits )                   : null,
                specialAbilities      = check( nameof( this.specialAbilities ) )      ?
                                            CDUtils.Assign( this.specialAbilities,        nameof( this.specialAbilities ),     Dicts.SpecialAbilities )         : null,
                specialStatIcon       = check( nameof( this.specialStatIcon ) )       ?
                                            CDUtils.Assign( this.specialStatIcon,         nameof( this.specialStatIcon ),      Dicts.StatIcon )                 : null,
                                                                                                        
                appearanceBehaviour   = check( nameof( this.appearanceBehaviour ) )   ?
                                            CDUtils.Assign( this.appearanceBehaviour,     nameof( this.appearanceBehaviour ),  Dicts.AppearanceBehaviour )      : null,
                                                                                                      
                tex                   = check( nameof( this.texture ) )         ? CDUtils.Assign( this.texture,                nameof( this.texture ) )         : null,
                altTex                = check( nameof( this.altTexture ) )      ? CDUtils.Assign( this.altTexture,             nameof( this.altTexture ) )      : null,
                emissionTex           = check( nameof( this.emissionTexture ) ) ? CDUtils.Assign( this.emissionTexture,        nameof( this.emissionTexture ) ) : null,    
                pixelTex              = check( nameof( this.pixelTexture ) )    ? CDUtils.Assign( this.pixelTexture,           nameof( this.pixelTexture ) )    : null,
                titleGraphic          = check( nameof( this.titleGraphic ) )    ? CDUtils.Assign( this.titleGraphic,           nameof( this.titleGraphic ) )    : null,
                animatedPortrait      = null, //TODO: animatedPortrait                                                                                              
                decals                = check( nameof( this.decals ) )          ? CDUtils.Assign( this.decals,                 nameof( this.decals ) )          : null
            };

            ErrorUtil.Clear();
        }
    }
}
