using System.Collections.Generic;
using System.Linq;

using Cysharp.Threading.Tasks;

using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "SpellDeathRipple", menuName = "Game/Attribute/Spell/17_DeathRipple")]
public class SpellDeathRipple : ScriptableAttributeSpell
{
    public async override UniTask<List<GridArenaNode>> ChooseTarget(ArenaManager arenaManager, EntityHero hero, Player player = null)
    {
        List<GridArenaNode> nodes = arenaManager
            .GridArenaHelper
            .GetAllGridNodes()
            .Where(t =>
                t.OccupiedUnit != null
                && ((ScriptableAttributeCreature)t.OccupiedUnit.Entity.ScriptableDataAttribute).TypeFaction != TypeFaction.Necropolis
            )
            .ToList();

        await UniTask.Delay(1);
        return nodes;
    }

    public async override UniTask AddEffect(GridArenaNode node, EntityHero heroRunSpell, ArenaManager arenaManager, Player player = null)
    {
        await base.AddEffect(node, heroRunSpell, arenaManager);

        var creatureArena = node.OccupiedUnit;
        ScriptableAttributeSecondarySkill baseSSkill = SchoolMagic.BaseSecondarySkill;
        SpellItem dataCurrent = new();
        if (creatureArena.Hero != null)
        {
            int levelSSkill = heroRunSpell.Data.SSkills.ContainsKey(baseSSkill.TypeTwoSkill)
                ? heroRunSpell.Data.SSkills[baseSSkill.TypeTwoSkill].level + 1
                : 0;
            dataCurrent = LevelData[levelSSkill];
        }

        // Add modification data.
        var totalDamage = dataCurrent.Effect + creatureArena.Hero.Data.PSkills[TypePrimarySkill.Power] * 5;

        // Run effect.
        if (AnimatePrefab.RuntimeKeyIsValid())
        {
            var asset = Addressables.InstantiateAsync(
               AnimatePrefab,
               new Vector3(0, 1, 0),
               Quaternion.identity,
               creatureArena.ArenaMonoBehavior.transform
           );
            var obj = await asset.Task;
            obj.gameObject.transform.localPosition = new Vector3(0, 1, 0);
            await UniTask.Delay(1000);
            Addressables.Release(asset);
        }

        await creatureArena.ArenaMonoBehavior.RunGettingHitSpell();
        creatureArena.SetDamage(totalDamage);
    }
}