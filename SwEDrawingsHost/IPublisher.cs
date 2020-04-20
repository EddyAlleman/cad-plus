﻿//*********************************************************************
//xTools
//Copyright(C) 2020 Xarial Pty Limited
//Product URL: https://xtools.xarial.com
//License: https://xtools.xarial.com/license/
//*********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xarial.XTools.Xport.SwEDrawingsHost
{
    public interface IPublisher : IDisposable
    {
        Task OpenDocument(string path);
        Task SaveDocument(string path);
        Task CloseDocument();
    }
}
