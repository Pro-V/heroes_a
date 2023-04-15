using Cysharp.Threading.Tasks;

using UnityEngine.Localization;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

public class MapEntityCreature : BaseMapEntity, IDialogMapObjectOperation
{
    public override void InitUnit(BaseEntity mapObject)
    {
        base.InitUnit(mapObject);
    }

    public async override void OnGoHero(Player player)
    {
        base.OnGoHero(player);

        if (LevelManager.Instance.ActivePlayer.DataPlayer.playerType != PlayerType.Bot)
        {
            DataResultDialog result = await OnTriggeredHero();

            if (result.isOk)
            {
                OnHeroGo(player);
            }
            else
            {
                // Click cancel.
            }
        }
        else
        {
            OnHeroGo(player);
        }
    }

    private void OnHeroGo(Player player)
    {
        MapObjectClass.OccupiedNode.DisableProtectedNeigbours(MapObjectClass);

        // TODO ARENA

        Destroy(gameObject);
    }

    public async UniTask<DataResultDialog> OnTriggeredHero()
    {

        // var creature = (EntityCreature)MapObjectClass;
        // string nameText = Helpers.GetStringNameCountWarrior(creature.Data.quantity);
        // LocalizedString stringCountWarriors = new LocalizedString(Constants.LanguageTable.LANG_TABLE_ADVENTURE, nameText);

        // var title = MapObjectClass.ScriptableData.Text.title.GetLocalizedString();
        // LocalizedString message = new LocalizedString(Constants.LanguageTable.LANG_TABLE_ADVENTURE, "army_attack")
        // {
        //     { "name", new StringVariable { Value = "<color=#FFFFAB>" + title + "</color>" } },
        // };

        var dialogData = new DataDialogMapObject()
        {
            // Header = string.Format("{0} {1}", stringCountWarriors.GetLocalizedString(), title),
            // Description = message.GetLocalizedString(),
            Sprite = MapObjectClass.ScriptableData.MenuSprite,
            TypeCheck = TypeCheck.Default
        };

        var dialogWindow = new DialogMapObjectProvider(dialogData);
        return await dialogWindow.ShowAndHide();
    }

}
