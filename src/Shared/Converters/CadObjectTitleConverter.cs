﻿//*********************************************************************
//CAD+ Toolset
//Copyright(C) 2020 Xarial Pty Limited
//Product URL: https://cadplus.xarial.com
//License: https://cadplus.xarial.com/license/
//*********************************************************************

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Xarial.CadPlus.Plus.Shared.Controls;
using Xarial.XCad.Documents;
using Xarial.XCad.Features;

namespace Xarial.CadPlus.Plus.Shared.Converters
{
    public class CadObjectTitleConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var obj = values[0];

            try
            {
                switch (obj)
                {
                    case IXDocument doc:
                        if (!string.IsNullOrEmpty(doc.Path))
                        {
                            var dispType = (DocumentTitleDisplayType_e)values[1];

                            var path = doc.Path;

                            if (string.IsNullOrEmpty(path)) 
                            {
                                path = doc.Title;
                            }

                            switch (dispType)
                            {
                                case DocumentTitleDisplayType_e.Path:
                                    return path;

                                case DocumentTitleDisplayType_e.FileName:
                                    return Path.GetFileName(path);

                                case DocumentTitleDisplayType_e.FileNameWithoutExtension:
                                    return Path.GetFileNameWithoutExtension(path);
                            }
                        }
                        break;

                    case IXConfiguration conf:
                        return conf.Name;

                    case IXSheet sheet:
                        return sheet.Name;

                    case IXCutListItem cutList:
                        return cutList.Name;

                    case ICustomObject custom:
                        return custom.Title;
                }
            }
            catch
            {
            }

            return "";
        }
        
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
