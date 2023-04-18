using UnityEngine;
using UnityEngine.UIElements;

public class UIAppMenuVariants : UILocaleBase
{
    [SerializeField] private UIDocument _rootDoc;
    public UIDocument RootDoc => _rootDoc;
    private VisualElement _root;
    [SerializeField] private UIMenuApp _parent;
    public UIMenuApp Parent => _parent;

    public void Init()
    {
        _root = _rootDoc.rootVisualElement;

        _root.Q<Button>("back").clickable.clicked += () =>
        {
            Hide();
        };

        _root.Q<Button>("singlegame").clickable.clicked += () =>
        {
            LevelManager.Instance.Level.Settings.TypeGame = TypeGame.Single;
            _parent.AppMenuNewGame.Show();
            Hide();
        };
        _root.Q<Button>("multiplegame").clickable.clicked += () =>
        {
            LevelManager.Instance.Level.Settings.TypeGame = TypeGame.MultipleOneDevice;
            _parent.AppMenuNewGame.Show();
            Hide();
        };

        Hide();
        base.Localize(_root);
    }

    public void Hide()
    {
        _root.style.display = DisplayStyle.None;
    }

    public void Show()
    {
        _root.style.display = DisplayStyle.Flex;
    }
}
