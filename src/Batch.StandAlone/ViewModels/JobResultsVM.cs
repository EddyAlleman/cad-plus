﻿//*********************************************************************
//CAD+ Toolset
//Copyright(C) 2021 Xarial Pty Limited
//Product URL: https://cadplus.xarial.com
//License: https://cadplus.xarial.com/license/
//*********************************************************************

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xarial.CadPlus.Batch.Base.Models;
using Xarial.CadPlus.Plus.Applications;
using Xarial.CadPlus.Plus.Services;
using Xarial.CadPlus.Batch.Base.Core;
using Xarial.CadPlus.Batch.Base.ViewModels;
using Xarial.XToolkit.Wpf.Extensions;
using Xarial.XCad.Base;

namespace Xarial.CadPlus.Batch.StandAlone.ViewModels
{
    public class JobResultsVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private JobResultVM m_Selected;

        public JobResultVM Selected 
        {
            get => m_Selected;
            set 
            {
                m_Selected = value;
                this.NotifyChanged();
            }
        }

        public ObservableCollection<JobResultVM> Items { get; }

        private readonly BatchJob m_Job;

        private readonly Func<BatchJob, IBatchRunJobExecutor> m_ExecFact;

        private readonly ICadDescriptor m_CadDesc;
        private readonly IXLogger m_Logger;

        public JobResultsVM(BatchJob job, 
            Func<BatchJob, IBatchRunJobExecutor> execFact, ICadDescriptor cadDesc, IXLogger logger) 
        {
            m_Job = job;

            m_CadDesc = cadDesc;
            m_Logger = logger;

            m_ExecFact = execFact;
            Items = new ObservableCollection<JobResultVM>();
        }

        public void StartNewJob()
        {
            var newRes = new JobResultVM($"Job #{Items.Count + 1}", m_ExecFact.Invoke(m_Job), m_CadDesc, m_Logger);
            Items.Add(newRes);
            Selected = newRes;
            newRes.TryRunBatchAsync();
        }
    }
}
