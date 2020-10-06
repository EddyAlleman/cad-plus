﻿//*********************************************************************
//CAD+ Toolset
//Copyright(C) 2020 Xarial Pty Limited
//Product URL: https://cadplus.xarial.com
//License: https://cadplus.xarial.com/license/
//*********************************************************************

using System.Collections.Generic;
using System.Linq;
using Xarial.CadPlus.CustomToolbar.Structs;
using Xarial.CadPlus.CustomToolbar.UI.Base;

namespace Xarial.CadPlus.CustomToolbar.UI.ViewModels
{
    public class CommandGroupVM : CommandVM<CommandGroupInfo>
    {
        private readonly CommandGroupInfo m_CmdGrp;
        private readonly CommandsCollection<CommandMacroVM> m_Commands;

        public CommandsCollection<CommandMacroVM> Commands
        {
            get
            {
                return m_Commands;
            }
        }

        public CommandGroupVM()
            : this(new CommandGroupInfo())
        {
        }

        public CommandGroupVM(CommandGroupInfo cmdGrp) : base(cmdGrp)
        {
            m_CmdGrp = cmdGrp;
            m_Commands = new CommandsCollection<CommandMacroVM>(
                (cmdGrp.Commands ?? new CommandMacroInfo[0])
                .Select(c => new CommandMacroVM(c)));

            m_Commands.CommandsChanged += OnCommandsCollectionChanged;
        }

        private void OnCommandsCollectionChanged(IEnumerable<CommandMacroVM> cmds)
        {
            m_CmdGrp.Commands = cmds.Select(c => c.Command).ToArray();
        }
    }
}