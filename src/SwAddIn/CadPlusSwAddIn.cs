﻿//*********************************************************************
//CAD+ Toolset
//Copyright(C) 2021 Xarial Pty Limited
//Product URL: https://cadplus.xarial.com
//License: https://cadplus.xarial.com/license/
//*********************************************************************

using System;
using System.Linq;
using System.Runtime.InteropServices;
using Xarial.XCad.SolidWorks;
using Xarial.XCad.Base.Attributes;
using System.ComponentModel;
using Xarial.XToolkit.Wpf.Dialogs;
using Xarial.CadPlus.AddIn.Base;
using Xarial.XCad;
using Xarial.XCad.Base;
using Xarial.CadPlus.Common.Services;
using Xarial.XCad.UI.PropertyPage;
using System.Reflection;
using Xarial.XToolkit.Reflection;
using Xarial.CadPlus.Plus;
using Autofac;
using Xarial.CadPlus.Common;
using Xarial.CadPlus.Init;
using Xarial.CadPlus.Plus.Applications;
using Xarial.CadPlus.Plus.Shared;
using Xarial.CadPlus.Plus.Services;
using Xarial.CadPlus.Plus.Extensions;
using Xarial.CadPlus.AddIn.Sw.Services;
using Xarial.XCad.SolidWorks.Services;
using System.Windows.Threading;
using Xarial.XCad.Base.Enums;
using Xarial.CadPlus.Plus.Shared.Services;
using Xarial.XToolkit.Helpers;

namespace Xarial.CadPlus.AddIn.Sw
{
    [ComVisible(true), Guid("AC45BDF0-66CB-4B08-8127-06C1F0C9452F")]
    [Title("CAD+ Toolset")]
    [Description("The toolset of utilities to complement SOLIDWORKS functionality")]
    public class CadPlusSwAddIn : SwAddInEx
    {
        private class LocalAppConfigBindingRedirectReferenceResolver : AppConfigBindingRedirectReferenceResolver
        {
            internal LocalAppConfigBindingRedirectReferenceResolver() : base("CAD+ SOLIDWORKS Add-In") 
            {
            }

            protected override Assembly[] GetRequestingAssemblies(Assembly requestingAssembly)
            {
                if (requestingAssembly != null)
                {
                    return new Assembly[] { requestingAssembly };
                }
                else
                {
                    return new Assembly[] 
                    {
                        typeof(CadPlusSwAddIn).Assembly,
                        typeof(CadPlusCommands_e).Assembly 
                    };
                }
            }   
        }

        private static readonly AssemblyResolver m_AssmResolver;

        static CadPlusSwAddIn() 
        {
            m_AssmResolver = new AssemblyResolver(AppDomain.CurrentDomain, "CAD+ Toolset");
            m_AssmResolver.RegisterAssemblyReferenceResolver(new LocalAppConfigBindingRedirectReferenceResolver());
        }

        private readonly AddInHost m_Host;

        public CadPlusSwAddIn()
        {
            var disp = Dispatcher.CurrentDispatcher;
            if (disp != null)
            {
                disp.UnhandledException += OnDispatcherUnhandledException;
            }
            AppDomain.CurrentDomain.UnhandledException += OnDomainUnhandledException;

            m_Host = new AddInHost(new SwAddInApplication(this), 
                new Initiator());
            m_Host.ConfigureServices += OnConfigureModuleServices;
        }
        
        private void OnConfigureModuleServices(IContainerBuilder builder)
        {
            var svc = ((ContainerBuilderWrapper)builder).Builder;

            svc.RegisterType<CadPlusPropertyPageHandlerProvider>().As<IPropertyPageHandlerProvider>();
            svc.RegisterType<CadPlusTaskPaneControlProvider>().As<ITaskPaneControlProvider>();
            
            builder.RegisterAdapter<IXApplication, ISwApplication>(a => (ISwApplication)a);
            builder.Register(x => x.GetService<IMacroExecutor>(CadApplicationIds.SolidWorks));
            builder.Register(x => x.GetService<ICadDescriptor>(CadApplicationIds.SolidWorks));
        }

        private void OnDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = e.ExceptionObject as Exception ?? new Exception("");

            var logger = m_Host?.Services?.GetService<IXLogger>() ?? new AppLogger();
            var msg = m_Host?.Services?.GetService<IMessageService>() ?? new GenericMessageService();

            logger.Log("Unhandled domain exception", LoggerMessageSeverity_e.Fatal);
            logger.Log(ex, true, LoggerMessageSeverity_e.Fatal);
            msg.ShowError(ex, "Unknown error");
        }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            var logger = m_Host?.Services?.GetService<IXLogger>() ?? new AppLogger();
            var msg = m_Host?.Services?.GetService<IMessageService>() ?? new GenericMessageService();

            logger.Log("Unhandled dispatcher exception", LoggerMessageSeverity_e.Fatal);
            logger.Log(e.Exception, true, LoggerMessageSeverity_e.Fatal);
            msg.ShowError(e.Exception, "Unknown error");

            e.Handled = true;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
