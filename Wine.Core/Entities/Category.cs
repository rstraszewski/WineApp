using System.ComponentModel;

namespace Wine.Core.Entities
{
    public enum Category : byte
    {
        [Description("Dry")]
        Dry,

        [Description("Semi-dry")]
        SemiDry,

        [Description("Sweet")]
        Sweet
    }
}