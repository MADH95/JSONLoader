
using APIPlugin;

using DiskCardGame;

using System;
using System.Collections.Generic;

using UnityEngine;

namespace JSONLoaderPlugin
{
    [System.Serializable]
    public class CardData
    {
        #region Fields

        public List<string> fieldsToEdit;
        public string name, displayedName, description;
        public List<string> metaCategories;
        public string cardComplexity;
        public string temple;
        public int baseAttack;
        public int baseHealth;
        public bool hideAttackAndHealth;
        public int cost, bonesCost, energyCost;
        public List<string> gemsColour;
        public string specialStatIcon;
        public List<string> tribes;
        public List<string> traits;
        public List<string> specialAbilities;
        public List<string> abilities;
        public string evolve_evolutionName;
        public int evolve_turnsToEvolve;
        public string defaultEvolutionName;
        public string tail_cardName;
        public string tail_tailLostPortrait;
        public string iceCube_creatureWithin;
        public bool flipPortraitForStrafe;
        public bool onePerDeck;
        public List<string> appearanceBehaviour;
        public string texture, altTexture;
        public string titleGraphic;
        public string pixelTexture;
        public string animatedPortrait;
        public List<string> decals;

        #endregion

        public static CardData CreateFromJSON( string jsonString )
            => JsonUtility.FromJson<CardData>( jsonString );

        public static void GenerateNewCard( CardData card )
        {
            NewCard.Add(
                name:                   card.name,
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
                gemsCost:               JLUtils.Assign( card, card.gemsColour,          nameof( gemsColour ),           Dicts.GemColour ),

                metaCategories:         JLUtils.Assign( card, card.metaCategories,      nameof( metaCategories ),       Dicts.MetaCategory ),
                cardComplexity:         JLUtils.Assign( card, card.cardComplexity,      nameof( cardComplexity ),       Dicts.Complexity ),
                temple:                 JLUtils.Assign( card, card.temple,              nameof( temple ),               Dicts.Temple ),
                tribes:                 JLUtils.Assign( card, card.tribes,              nameof( tribes ),               Dicts.Tribes ),

                abilities:              JLUtils.Assign( card, card.abilities,           nameof( abilities ),            Dicts.Abilities ),
                traits:                 JLUtils.Assign( card, card.traits,              nameof( traits ),               Dicts.Traits ),
                specialAbilities:       JLUtils.Assign( card, card.specialAbilities,    nameof( specialAbilities ),     Dicts.SpecialAbilities ),
                specialStatIcon:        JLUtils.Assign( card, card.specialStatIcon,     nameof( specialStatIcon ),      Dicts.StatIcon ),

                appearanceBehaviour:    JLUtils.Assign( card, card.appearanceBehaviour, nameof( appearanceBehaviour ),  Dicts.AppearanceBehaviour ),

                tex:                    JLUtils.Assign( card, card.texture,             nameof( texture ) ),
                altTex:                 JLUtils.Assign( card, card.texture,             nameof( altTexture ) ),
                titleGraphic:           JLUtils.Assign( card, card.texture,             nameof( titleGraphic ) ),
                pixelTex:               JLUtils.Assign( card, card.pixelTexture,        nameof( pixelTexture ) ),
                animatedPortrait: null, //TODO: implement
                decals:                 JLUtils.Assign( card, card.decals,              nameof( decals ) )
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
                displayedName           = check( nameof( displayedName ) )         ? card.displayedName         : null,
                description             = check( nameof( description ) )           ? card.description           : null,

                baseAttack              = check( nameof( baseAttack ) )            ? card.baseAttack            : null,
                baseHealth              = check( nameof( baseHealth ) )            ? card.baseHealth            : null,
                hideAttackAndHealth     = check( nameof( hideAttackAndHealth ) )   ? card.hideAttackAndHealth   : null,

                onePerDeck              = check( nameof( onePerDeck ) )            ? card.onePerDeck            : null,
                flipPortraitForStrafe   = check( nameof( flipPortraitForStrafe ) ) ? card.flipPortraitForStrafe : null,
                defaultEvolutionName    = check( nameof( defaultEvolutionName ) )  ? card.defaultEvolutionName  : null,

                cost                    = check( nameof( cost ) )                  ? card.cost                  : null,
                bonesCost               = check( nameof( bonesCost ) )             ? card.bonesCost             : null,
                energyCost              = check( nameof( energyCost ) )            ? card.energyCost            : null,
                gemsCost                = JLUtils.CheckThenAssign( card,             card.gemsColour,             nameof( gemsColour ),           Dicts.GemColour ),

                metaCategories          = JLUtils.CheckThenAssign( card,             card.metaCategories,         nameof( metaCategories ),       Dicts.MetaCategory ),
                cardComplexity          = JLUtils.CheckThenAssign( card,             card.cardComplexity,         nameof( cardComplexity ),       Dicts.Complexity ),
                temple                  = JLUtils.CheckThenAssign( card,             card.temple,                 nameof( temple ),               Dicts.Temple ),
                tribes                  = JLUtils.CheckThenAssign( card,             card.tribes,                 nameof( tribes ),               Dicts.Tribes ),

                abilities               = JLUtils.CheckThenAssign( card,             card.abilities,              nameof( abilities ),            Dicts.Abilities),
                traits                  = JLUtils.CheckThenAssign( card,             card.traits,                 nameof( traits ),               Dicts.Traits ),
                specialAbilities        = JLUtils.CheckThenAssign( card,             card.specialAbilities,       nameof( specialAbilities ),     Dicts.SpecialAbilities ),
                specialStatIcon         = JLUtils.CheckThenAssign( card,             card.specialStatIcon,        nameof( specialStatIcon ),      Dicts.StatIcon ),

                appearanceBehaviour     = JLUtils.CheckThenAssign( card,             card.appearanceBehaviour,    nameof( appearanceBehaviour ),  Dicts.AppearanceBehaviour ),

                tex                     = JLUtils.CheckThenAssign( card,             card.texture,                nameof( texture ) ),
                pixelTex                = JLUtils.CheckThenAssign( card,             card.pixelTexture,           nameof( pixelTexture ) ),
                altTex                  = JLUtils.CheckThenAssign( card,             card.altTexture,             nameof( altTexture ) ),
                titleGraphic            = JLUtils.CheckThenAssign( card,             card.titleGraphic,           nameof( titleGraphic ) ),
                animatedPortrait        = null, //TODO: animatedPortrait                                                                                               
                decals                  = JLUtils.CheckThenAssign( card,             card.decals,                 nameof( decals ) )
            };

            if ( card.evolve_evolutionName is not null || card.tail_cardName is not null || card.iceCube_creatureWithin is not null )
                JLUtils.AssignAbilityData( card );
        }
    }
}
