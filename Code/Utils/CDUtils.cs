using UnityEngine;

using APIPlugin;

namespace JLPlugin.Utils
{
    using Data;

    class CDUtils
    {
        public static CardData CreateFromJSON( string jsonString )
            => JsonUtility.FromJson<CardData>( jsonString );

        public static void GenerateNewCard( CardData card )
        {
            NewCard.Add(
                name: card.name,
                displayedName:          card.displayedName ?? "",
                description:            card.description ?? "",

                baseAttack:             card.baseAttack,
                baseHealth:             card.baseHealth == 0 ? 1 : card.baseHealth,
                hideAttackAndHealth:    card.hideAttackAndHealth,

                onePerDeck:             card.onePerDeck,
                flipPortraitForStrafe:  card.flipPortraitForStrafe,
                defaultEvolutionName:   card.defaultEvolutionName,

                cost:                   card.cost,
                bonesCost:              card.bonesCost,
                energyCost:             card.energyCost,
                gemsCost:               JLUtils.Assign( card, card.gemsColour,          nameof( card.gemsColour ),          Dicts.GemColour ),
                metaCategories:         JLUtils.Assign( card, card.metaCategories,      nameof( card.metaCategories ),      Dicts.MetaCategory ),
                cardComplexity:         JLUtils.Assign( card, card.cardComplexity,      nameof( card.cardComplexity ),      Dicts.Complexity ),
                temple:                 JLUtils.Assign( card, card.temple,              nameof( card.temple ),              Dicts.Temple ),
                tribes:                 JLUtils.Assign( card, card.tribes,              nameof( card.tribes ),              Dicts.Tribes ),

                abilities:              JLUtils.Assign( card, card.abilities,           nameof( card.abilities ),           Dicts.Abilities ),
                traits:                 JLUtils.Assign( card, card.traits,              nameof( card.traits ),              Dicts.Traits ),
                specialAbilities:       JLUtils.Assign( card, card.specialAbilities,    nameof( card.specialAbilities ),    Dicts.SpecialAbilities ),
                specialStatIcon:        JLUtils.Assign( card, card.specialStatIcon,     nameof( card.specialStatIcon ),     Dicts.StatIcon ),

                appearanceBehaviour:    JLUtils.Assign( card, card.appearanceBehaviour, nameof( card.appearanceBehaviour ), Dicts.AppearanceBehaviour ),

                tex:                    JLUtils.Assign( card, card.texture,             nameof( card.texture ) ),
                altTex:                 JLUtils.Assign( card, card.altTexture,          nameof( card.altTexture ) ),
                titleGraphic:           JLUtils.Assign( card, card.titleGraphic,        nameof( card.titleGraphic ) ),
                pixelTex:               JLUtils.Assign( card, card.pixelTexture,        nameof( card.pixelTexture ) ),
                animatedPortrait:       null, //TODO: implement
                decals:                 JLUtils.Assign( card, card.decals,              nameof( card.decals ) )
            );

            if ( card.evolve_evolutionName is not null || card.tail_cardName is not null || card.iceCube_creatureWithin is not null )
                JLUtils.AssignAbilityData( card );
        }

        public static void EditExistingCard( CardData card )
        {
            JLUtils.CheckValidFields( card );

            bool check( string fieldName ) => card.fieldsToEdit.Contains( fieldName );

            var _ = new CustomCard( card.name )
            {
                displayedName           = check( nameof( card.displayedName ) )         ? card.displayedName         : null,
                description             = check( nameof( card.description ) )           ? card.description           : null,

                baseAttack              = check( nameof( card.baseAttack ) )            ? card.baseAttack            : null,
                baseHealth              = check( nameof( card.baseHealth ) )            ? card.baseHealth            : null,
                hideAttackAndHealth     = check( nameof( card.hideAttackAndHealth ) )   ? card.hideAttackAndHealth   : null,

                onePerDeck              = check( nameof( card.onePerDeck ) )            ? card.onePerDeck            : null,
                flipPortraitForStrafe   = check( nameof( card.flipPortraitForStrafe ) ) ? card.flipPortraitForStrafe : null,
                defaultEvolutionName    = check( nameof( card.defaultEvolutionName ) )  ? card.defaultEvolutionName  : null,

                cost                    = check( nameof( card.cost ) )                  ? card.cost                  : null,
                bonesCost               = check( nameof( card.bonesCost ) )             ? card.bonesCost             : null,
                energyCost              = check( nameof( card.energyCost ) )            ? card.energyCost            : null,
                gemsCost                = JLUtils.CheckThenAssign( card,             card.gemsColour,             nameof( card.gemsColour ),           Dicts.GemColour ),

                metaCategories          = JLUtils.CheckThenAssign( card,             card.metaCategories,         nameof( card.metaCategories ),       Dicts.MetaCategory ),
                cardComplexity          = JLUtils.CheckThenAssign( card,             card.cardComplexity,         nameof( card.cardComplexity ),       Dicts.Complexity ),
                temple                  = JLUtils.CheckThenAssign( card,             card.temple,                 nameof( card.temple ),               Dicts.Temple ),
                tribes                  = JLUtils.CheckThenAssign( card,             card.tribes,                 nameof( card.tribes ),               Dicts.Tribes ),

                abilities               = JLUtils.CheckThenAssign( card,             card.abilities,              nameof( card.abilities ),            Dicts.Abilities),
                traits                  = JLUtils.CheckThenAssign( card,             card.traits,                 nameof( card.traits ),               Dicts.Traits ),
                specialAbilities        = JLUtils.CheckThenAssign( card,             card.specialAbilities,       nameof( card.specialAbilities ),     Dicts.SpecialAbilities ),
                specialStatIcon         = JLUtils.CheckThenAssign( card,             card.specialStatIcon,        nameof( card.specialStatIcon ),      Dicts.StatIcon ),

                appearanceBehaviour     = JLUtils.CheckThenAssign( card,             card.appearanceBehaviour,    nameof( card.appearanceBehaviour ),  Dicts.AppearanceBehaviour ),

                tex                     = JLUtils.CheckThenAssign( card,             card.texture,                nameof( card.texture ) ),
                pixelTex                = JLUtils.CheckThenAssign( card,             card.pixelTexture,           nameof( card.pixelTexture ) ),
                altTex                  = JLUtils.CheckThenAssign( card,             card.altTexture,             nameof( card.altTexture ) ),
                titleGraphic            = JLUtils.CheckThenAssign( card,             card.titleGraphic,           nameof( card.titleGraphic ) ),
                animatedPortrait        = null, //TODO: animatedPortrait                                                                                               
                decals                  = JLUtils.CheckThenAssign( card,             card.decals,                 nameof( card.decals ) )
            };

            if ( card.evolve_evolutionName is not null || card.tail_cardName is not null || card.iceCube_creatureWithin is not null )
                JLUtils.AssignAbilityData( card );
        }
    }
}
