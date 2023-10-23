using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace JLPlugin.Hotkeys
{

	internal class HotkeyController
	{
		public class Hotkey
		{
			public KeyCode[] KeyCodes;
			public Action Function;
		}

		private static Action<List<KeyCode>, KeyCode> OnHotkeyPressed = delegate { };
		private static KeyCode[] AllCodes = Enum.GetValues(typeof(KeyCode)).Cast<KeyCode>().ToArray();

		private List<Hotkey> Hotkeys = new();

		private List<KeyCode> m_pressedKeys = new();
		private bool m_hotkeyActivated = false;

		private static KeyCode[] DeserializeKeyCodes(string hotkey)
		{
			KeyCode[] keyCodes = hotkey.Trim().Split('+').Select(static (a) =>
			{
				if (!Enum.TryParse(a, out KeyCode b))
				{
					Plugin.Log.LogError(
						$"Unknown hotkey: '{a}'. See possible hotkeys here separated by +. https://docs.unity3d.com/ScriptReference/KeyCode.html");
				}

				return b;
			}).ToArray();
			return keyCodes;
		}

		public void Update()
		{
			foreach (KeyCode code in AllCodes)
			{
				if (Input.GetKeyDown(code) && !m_pressedKeys.Contains(code))
				{
					m_pressedKeys.Add(code);
					HotkeysChanged(code, true);
				}
			}

			for (int i = 0; i < m_pressedKeys.Count; i++)
			{
				KeyCode pressedKey = m_pressedKeys[i];
				if (!Input.GetKey(pressedKey))
				{
					m_pressedKeys.Remove(pressedKey);
					HotkeysChanged(KeyCode.None, false);

					m_hotkeyActivated = false;
				}
			}
		}

		private void HotkeysChanged(KeyCode pressedButton, bool triggerHotkey)
		{
			if (triggerHotkey)
			{
				Hotkey activatedHotkey = null;
				foreach (Hotkey hotkey in Hotkeys)
				{
					if (m_hotkeyActivated)
					{
						continue;
					}

					if (hotkey.KeyCodes.Length == 0)
					{
						continue;
					}

					// all buttons from hotkey are pressed
					bool allHotkeysPressed = m_pressedKeys.Intersect(hotkey.KeyCodes).Count() == hotkey.KeyCodes.Length;
					if (allHotkeysPressed)
					{
						if (activatedHotkey == null || activatedHotkey.KeyCodes.Length < hotkey.KeyCodes.Length)
						{
							activatedHotkey = hotkey;
						}
					}
				}

				if (activatedHotkey != null)
				{
					activatedHotkey.Function?.Invoke();
				}
			}

			if (pressedButton != KeyCode.None)
			{
				OnHotkeyPressed?.Invoke(m_pressedKeys, pressedButton);
			}
		}

		public void AddHotkey(string hotkeys, Action callback)
		{
			Hotkeys.Add(new Hotkey()
			{
				KeyCodes = DeserializeKeyCodes(hotkeys),
				Function = callback
			});
		}
	}
}