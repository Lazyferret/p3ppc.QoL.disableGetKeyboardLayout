using p3ppc.QoL.disableGetKeyboardLayout.Configuration;
using p3ppc.QoL.disableGetKeyboardLayout.Template;
using Reloaded.Hooks.Definitions;
using Reloaded.Mod.Interfaces;
using System.Runtime.InteropServices;
using IReloadedHooks = Reloaded.Hooks.ReloadedII.Interfaces.IReloadedHooks;

namespace p3ppc.QoL.disableGetKeyboardLayout
{
    /// <summary>
    /// Your mod logic goes here.
    /// </summary>
    public class Mod : ModBase // <= Do not Remove.
    {
        /// <summary>
        /// Provides access to the mod loader API.
        /// </summary>
        private readonly IModLoader _modLoader;

        /// <summary>
        /// Provides access to the Reloaded.Hooks API.
        /// </summary>
        /// <remarks>This is null if you remove dependency on Reloaded.SharedLib.Hooks in your mod.</remarks>
        private readonly IReloadedHooks? _hooks;

        /// <summary>
        /// Provides access to the Reloaded logger.
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Entry point into the mod, instance that created this class.
        /// </summary>
        private readonly IMod _owner;

        /// <summary>
        /// Provides access to this mod's configuration.
        /// </summary>
        private Config _configuration;

        /// <summary>
        /// The configuration of the currently executing mod.
        /// </summary>
        private readonly IModConfig _modConfig;
        private IHook<GetKeyboardLayoutDelegate> _getGetKeyboardLayoutHook;
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr LoadLibrary(string lpLibFileName);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetKeyboardLayout(uint idThread);
        
        public Mod(ModContext context)
        {
            _modLoader = context.ModLoader;
            _hooks = context.Hooks;
            _logger = context.Logger;
            _owner = context.Owner;
            _configuration = context.Configuration;
            _modConfig = context.ModConfig;
            
            if (!Utils.Initialise(_logger, _configuration, _modLoader))
                return;
            var GetKeyboardLayoutPointer = GetProcAddress(LoadLibrary("user32.dll"), "GetKeyboardLayout");
            Utils.Log($"0x{GetKeyboardLayoutPointer:X}");
            _getGetKeyboardLayoutHook = _hooks.CreateHook<GetKeyboardLayoutDelegate>(GetFixedKeyboardLayout, (long)GetKeyboardLayoutPointer).Activate();


        }
        //private const uint FixedLayoutIdentifier = 0x04190419;
        private delegate IntPtr GetKeyboardLayoutDelegate(uint idThread);
        private IntPtr GetFixedKeyboardLayout(uint idThread)
        {
            try { return (IntPtr)_configuration.FixedLayoutIdentifier; }
            catch (Exception e) { return _getGetKeyboardLayoutHook.OriginalFunction(idThread); }
        }


        #region Standard Overrides
        public override void ConfigurationUpdated(Config configuration)
        {
            // Apply settings from configuration.
            // ... your code here.
            _configuration = configuration;
            _logger.WriteLine($"[{_modConfig.ModId}] Config Updated: Applying");
        }
        #endregion

        #region For Exports, Serialization etc.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public Mod() { }
#pragma warning restore CS8618
        #endregion
    }
}