using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboLynx.Umbraco.QRCodeGenerator.Cache
{
    public interface IQRCodeCacheFactory
    {
        IQRCodeCache CreateCache<T>(string name, IQRCodeCacheFileSystem fileSystem, IQRCodeCacheUrlProvider urlProvider);
    }
}
