using System.ComponentModel;

namespace Wine.Core.Entities
{
    public enum Varietal : byte
    {
        [Description("Red wine")]
        Red,

        [Description("White wine")]
        White,

        [Description("Sparkling wine")]
        Sparkling,

        [Description("Rose wine")]
        Rose
    }
}