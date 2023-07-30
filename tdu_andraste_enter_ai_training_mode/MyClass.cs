using Andraste.Payload.ModManagement;
using Andraste.Payload.Native;
using System;
using NLog;

namespace KatieCookie.tdu
{
	public class EnterAiTrainingMode : BasePlugin
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		private bool _enabled;
		public override bool Enabled
		{
			get {
				return _enabled;
			}
			set
			{
				_enabled = value;

				// swap 0x13 with 0x18
				IntPtr normal_mode_init_check_location = new IntPtr(0x0097ce07);
				byte[] normal_mode_init_check_original = new byte[] {
					0x83, 0xbd, 0x00, 0x01, 0x00, 0x00, 0x18
				};
				byte[] normal_mode_init_check_patched = new byte[] {
					0x83, 0xbd, 0x00, 0x01, 0x00, 0x00, 0x13
				};
				InstructionPatcher normal_mode_init_check_patcher = new InstructionPatcher(normal_mode_init_check_location, normal_mode_init_check_original, normal_mode_init_check_patched);

				IntPtr case_0x13_location = new IntPtr (0x0097d678);
				byte[] case_0x13_original = new byte[] {
					0xb9, 0xd2, 0x97, 0x00
				};
				byte[] case_0x13_patched = new byte[] {
					0xf3, 0xd3, 0x97, 0x00
				};
				InstructionPatcher case_0x13_patcher = new InstructionPatcher (case_0x13_location, case_0x13_original, case_0x13_patched);

				IntPtr case_0x18_location = new IntPtr (0x0097d68c);
				byte[] case_0x18_original = new byte[] {
					0xf3, 0xd3, 0x97, 0x00
				};
				byte[] case_0x18_patched = new byte[] {
					0xb9, 0xd2, 0x97, 0x00
				};
				InstructionPatcher case_0x18_patcher = new InstructionPatcher (case_0x18_location, case_0x18_original, case_0x18_patched);

				IntPtr not_0x18_location = new IntPtr (0x0097d597);
				byte[] not_0x18_original = new byte[] {
					0x83, 0xf8, 0x18
				};
				byte[] not_0x18_patched = new byte[] {
					0x83, 0xf8, 0x13
				};
				InstructionPatcher not_0x18_patcher = new InstructionPatcher (not_0x18_location, not_0x18_original, not_0x18_patched);

				// avoid a null point access in ai trainer mode
				IntPtr crash_location = new IntPtr(0x00718132);
				byte[] crash_original = new byte[] {
					0x8B, 0x83, 0x20, 0x48, 0x00, 0x00
				};
				byte[] crash_patched = new byte[] {
					0xeb, 0x5a, 0x90, 0x90, 0x90, 0x90
				};
				InstructionPatcher crash_patcher = new InstructionPatcher (crash_location, crash_original, crash_patched);

				normal_mode_init_check_patcher.Patch (value);
				case_0x13_patcher.Patch (value);
				case_0x18_patcher.Patch (value);
				not_0x18_patcher.Patch (value);
				crash_patcher.Patch (value);

				if (value){
					Logger.Info("Forcing game to load into AI training mode (no keyboard input)");
				}else{
					Logger.Info("Not forcing game to load into AI training mode");
				}
			}
		}

		protected override void PluginLoad()
		{
		}

		protected override void PluginUnload()
		{
		}
	}
}