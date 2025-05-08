using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace ScriptableObjectMenu
{
	internal sealed class ScriptMenu : BaseMenu
	{
		// The ScriptableObject template path
		private const string TEMPLATE_FILE_PATH = "ScriptTemplate.cs";

		// The template's class naming tag
		private const string TEMPLATE_CLASS_TAG = "<SCRIPT_CLASS_NAME>";

		// The default name for new Scripts
		private const string DEFAULT_CLASS_NAME = "NewScriptableObject";

		// The regex pattern for identifiers
		private const string IDENTIFIER_PATTERN = "^[A-Z_][\\w]*$";

		[MenuItem(EDITOR_ASSET_MENU_PATH + "Script", false, EDITOR_ASSET_MENU_PRIORITY + 1)]
		internal static void Initiate ()
		{
			// Display save dialog
			var path = EditorUtility.SaveFilePanelInProject("Save Script", DEFAULT_CLASS_NAME, "cs", string.Empty, TryGetProjectPath());

			if (!string.IsNullOrEmpty(path))
			{
				// Get name from path
				var name = Path.GetFileNameWithoutExtension(path);

				// Validate class name
				if (Regex.IsMatch(name, IDENTIFIER_PATTERN))
				{
					// Find template guid
					var guid = AssetDatabase.FindAssets(TEMPLATE_FILE_PATH + " t:TextAsset");

					if (guid.Length > 0)
					{
						// Get path from guid
						var file = AssetDatabase.GUIDToAssetPath(guid[0]);

						if (File.Exists(file))
						{
							// Get assembly name
							var easm = Assembly.GetExecutingAssembly().FullName;
							easm = easm.Replace("-Editor", string.Empty);

							// Get qualified name
							var qasm = Assembly.CreateQualifiedName(easm, name);

							// Get type
							var type = Type.GetType(qasm, false, true);

							// Prevent type conflict
							if (type == null)
							{
								// Copy template
								File.Copy(file, path, true);

								if (File.Exists(path))
								{
									// Get text encoding
									var utf8 = new UTF8Encoding(true);

									// Set class name
									var text = File.ReadAllText(path, utf8);
									text = text.Replace(TEMPLATE_CLASS_TAG, name);

									// Save template
									File.WriteAllText(path, text, utf8);

									UpdateAssetDatabase(typeof(MonoScript), path);

									// Log on complete
									Debug.Log($"Script created at \"{path}\"");

									return;
								}
							}
							else
							{
								// Alert on conflicting type error
								if (EditorUtility.DisplayDialog("Error", $"Class Name Already Exists\n\n\"{name}\"", "OK"))
								{
									// And recall
									Initiate();
								}

								return;
							}
						}
					}
				}
				else
				{
					// Alert on invalid class name error
					if (EditorUtility.DisplayDialog("Error", $"Invalid Class Name\n\n\"{name}\"\n\nThe name must begin with a capital letter and contain only letters, digits or underscores.", "OK"))
					{
						// And recall
						Initiate();
					}

					return;
				}

				// Alert on system I/O error
				EditorUtility.DisplayDialog("Error", $"Failed To Create Script\n\n\"{path}\"", "OK");
			}
		}
	}
}