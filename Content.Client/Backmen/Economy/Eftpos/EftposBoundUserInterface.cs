﻿using Content.Client.Backmen.Economy.Eftpos.UI;
using Content.Shared.Backmen.Economy.Eftpos;
using Robust.Client.GameObjects;

namespace Content.Client.Backmen.Economy.Eftpos;

public sealed class EftposBoundUserInterface : BoundUserInterface
{
    [ViewVariables]
    private EftposMenu? _menu;

    public EftposBoundUserInterface(ClientUserInterfaceComponent owner, Enum uiKey) : base(owner, uiKey)
    {
    }
    protected override void Open()
    {
        base.Open();
        _menu = new EftposMenu { Title = IoCManager.Resolve<IEntityManager>().GetComponent<MetaDataComponent>(Owner.Owner).EntityName };

        _menu.OnChangeValue += (_, value) => SendMessage(new EftposChangeValueMessage(value));
        _menu.OnResetValue += (_) => SendMessage(new EftposChangeValueMessage(null));
        _menu.OnChangeLinkedAccount += (_, accountNumber) => SendMessage(new EftposChangeLinkedAccountNumberMessage(accountNumber));
        _menu.OnResetLinkedAccount += (_) => SendMessage(new EftposChangeLinkedAccountNumberMessage(null));

        _menu.OnSwipeCard += (_) => SendMessage(new EftposSwipeCardMessage());
        _menu.OnLock += (_) => SendMessage(new EftposLockMessage());

        _menu.OnClose += Close;
        _menu.OpenCentered();
    }
    protected override void UpdateState(BoundUserInterfaceState state)
    {
        base.UpdateState(state);
        var castState = (SharedEftposComponent.EftposBoundUserInterfaceState) state;
        _menu?.UpdateState(castState);
    }
    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (!disposing)
            return;

        if (_menu == null)
            return;

        _menu.OnClose -= Close;
        _menu.Dispose();
    }
}
