using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace RoboLynx.Umbraco.QRCodeGenerator.Models
{
    [Serializable]
    public class QRCodeSettings : ICloneable
    {
        [Range(1, int.MaxValue)]
        public int Size { get; set; }

        public string Format { get; set; }

        public string DarkColor { get; set; }

        public string LightColor { get; set; }

        public string Icon { get; set; }

        [Range(0, int.MaxValue)]
        public int IconSizePercent { get; set; }

        [Range(0, int.MaxValue)]
        public int? IconBorderWidth { get; set; }

        [JsonConverter(typeof(BitBoolJsonConverter))]
        public bool? DrawQuiteZone { get; set; }

        public ECCLevel? ECCLevel { get; set; }

        public object Clone()
        {
            return new QRCodeSettings()
            {
                Size = Size,
                Format = Format,
                DarkColor = DarkColor,
                LightColor = LightColor,
                Icon = Icon,
                IconSizePercent = IconSizePercent,
                DrawQuiteZone = DrawQuiteZone,
                IconBorderWidth = IconBorderWidth,
                ECCLevel = ECCLevel
            };
        }
    }
}
