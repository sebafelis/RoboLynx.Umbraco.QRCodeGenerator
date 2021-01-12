using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Models;

namespace RoboLynx.Umbraco.QRCodeGenerator.QRCodeSources
{
    public interface IQRCodeSource
    {
        string Name { get; }
        string Description { get; }
        T GetValue<T>(int index, string key) where T : IConvertible;
    }
}
