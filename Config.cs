using p3ppc.QoL.disableGetKeyboardLayout.Template.Configuration;
using System.ComponentModel;

namespace p3ppc.QoL.disableGetKeyboardLayout.Configuration
{
    public class Config : Configurable<Config>
    {
        [DisplayName("Debug Mode")]
        [Description("Logs additional information to the console that is useful for debugging.")]
        [DefaultValue(false)]
        public bool DebugEnabled { get; set; } = false;
        [DisplayName("Default KeyboardLayout")]
        [Description("To use this convert hexademical identifier to decimal. Example: Russian layout is 0x0419419 -> 68748313")]
        [DefaultValue(0x04190419)]
        public uint FixedLayoutIdentifier { get; set; } = 0x04190419;
    }
    /// <summary>
    /// Allows you to override certain aspects of the configuration creation process (e.g. create multiple configurations).
    /// Override elements in <see cref="ConfiguratorMixinBase"/> for finer control.
    /// </summary>
    public class ConfiguratorMixin : ConfiguratorMixinBase
    {
        // 
    }
}
