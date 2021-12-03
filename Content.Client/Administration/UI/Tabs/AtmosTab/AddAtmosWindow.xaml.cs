﻿using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Robust.Client.AutoGenerated;
using Robust.Client.Console;
using Robust.Client.Player;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.CustomControls;
using Robust.Shared.GameObjects;
using Robust.Shared.IoC;
using Robust.Shared.Map;

namespace Content.Client.Administration.UI.Tabs.AtmosTab
{
    [GenerateTypedNameReferences]
    [UsedImplicitly]
    public partial class AddAtmosWindow : SS14Window
    {
        private IEnumerable<IMapGrid>? _data;

        protected override void EnteredTree()
        {
            _data = IoCManager.Resolve<IMapManager>().GetAllGrids().Where(g => (int) g.Index != 0);
            foreach (var grid in _data)
            {
                IEntity? tempQualifier = IoCManager.Resolve<IPlayerManager>().LocalPlayer?.ControlledEntity;
                var playerGrid = (tempQualifier != null ? IoCManager.Resolve<IEntityManager>().GetComponent<TransformComponent>(tempQualifier.Uid) : null).GridID;
                GridOptions.AddItem($"{grid.Index} {(playerGrid == grid.Index ? " (Current)" : "")}");
            }

            GridOptions.OnItemSelected += eventArgs => GridOptions.SelectId(eventArgs.Id);
            SubmitButton.OnPressed += SubmitButtonOnOnPressed;
        }

        private void SubmitButtonOnOnPressed(BaseButton.ButtonEventArgs obj)
        {
            if (_data == null)
                return;
            var dataList = _data.ToList();
            var selectedGrid = dataList[GridOptions.SelectedId].Index;
            IoCManager.Resolve<IClientConsoleHost>().ExecuteCommand($"addatmos {selectedGrid}");
        }
    }
}
