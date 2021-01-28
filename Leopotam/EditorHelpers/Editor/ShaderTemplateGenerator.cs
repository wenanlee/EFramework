// ----------------------------------------------------------------------------
// The MIT License
// LeopotamGroupLibrary https://github.com/Leopotam/LeopotamGroupLibraryUnity
// Copyright (c) 2012-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace EFramework.EditorHelpers.UnityEditors {
    sealed class ShaderTemplateGenerator : EditorWindow {
        const string ShaderTemplate =
            "Shader \"Custom/<<NAME>>\"{\tProperties{\t\t_MainTex(\"Texture\",2D)=\"white\" <<>>\n\t}\n\n\tSubShader{" +
            "\t\tTags <<\"RenderType\"=\"<<TYPE>>\" \"Queue\"=\"<<QUEUE>>\" \"IgnoreProjector\"=\"True\" " +
            "\"ForceNoShadowCasting\"=\"True\">>\n\t\tLOD 100\n\n<<SHADERFLAGS>>\t\tCGINCLUDE\n\t\t#include \"UnityCG.cginc\"\n\n" +
            "\t\tsampler2D _MainTex;\t\tfloat4 _MainTex_ST;\n\t\tstruct v2f{\t\t\tfloat4 pos:SV_POSITION;" +
            "\t\t\tfloat2 uv:TEXCOORD0;\t\t};\n\t\tv2f vert(appdata_full v){\t\t\tv2f o;\t\t\to.pos=UnityObjectToClipPos(v.vertex);" +
            "\t\t\to.uv=TRANSFORM_TEX(v.texcoord,_MainTex);\t\t\treturn o;\t\t}\n\n\t\tfixed4 frag(v2f i):SV_Target{" +
            "\t\t\treturn tex2D(_MainTex,i.uv);\t\t}\n\t\tENDCG\n\n\t\tPass{\t\t\tTags <<\"LightMode\"=\"ForwardBase\">>\n" +
            "\t\t\tCGPROGRAM\n\t\t\t#pragma vertex vert\n\t\t\t#pragma fragment frag\n\t\t\tENDCG\n\t\t}\n\t}\n\tFallback Off\n}";

        const string ShaderAlphaBlendTags = "\t\tCull Off\n\t\tZWrite Off\n\t\tBlend SrcAlpha OneMinusSrcAlpha\n\n";

        static readonly string TabReplacement = new string (' ', 4);

        static string GetAssetPath () {
            var path = AssetDatabase.GetAssetPath (Selection.activeObject);
            if (!string.IsNullOrEmpty (path) && AssetDatabase.Contains (Selection.activeObject)) {
                if (!AssetDatabase.IsValidFolder (path)) {
                    path = Path.GetDirectoryName (path);
                }
            } else {
                path = "Assets";
            }
            return path;
        }

        static string GetShaderCode (string template, string name, string renderType, string renderQueue, bool isAlphaBlend) {
            template = template.Replace ("<<NAME>>", name);
            template = template.Replace ("<<TYPE>>", renderType);
            template = template.Replace ("<<QUEUE>>", renderQueue);
            template = template.Replace ("<<SHADERFLAGS>>", isAlphaBlend ? ShaderAlphaBlendTags : string.Empty);
            template = template.Replace ("\t", TabReplacement);
            template = template.Replace ("{", " {\n");
            template = template.Replace ("<<>>", "{}");
            template = template.Replace ("<<", "{ ");
            template = template.Replace (">>", " }");
            template = template.Replace ("(", " (");
            template = template.Replace ("=", " = ");
            template = template.Replace (":", " : ");
            template = template.Replace (",", ", ");
            template = template.Replace (";", ";\n");
            return template;
        }

        static Texture2D GetIcon () {
            return EditorGUIUtility.IconContent ("Shader Icon").image as Texture2D;
        }

        [MenuItem ("Assets/LeopotamGroup/Shaders/Create unlit opaque shader")]
        static void CreateUnlitOpaqueShader () {
            EditorUtils.CreateAndRenameAsset (
                string.Format ("{0}/UnlitOpaque.shader", GetAssetPath ()),
                GetIcon (), name => Create (ShaderType.Opaque, name));
        }

        [MenuItem ("Assets/LeopotamGroup/Shaders/Create unlit transparent shader")]
        static void CreateUnlitTransparentShader () {
            EditorUtils.CreateAndRenameAsset (
                string.Format ("{0}/UnlitTransparent.shader", GetAssetPath ()),
                GetIcon (), name => Create (ShaderType.Transparent, name));
        }

        public static string Create (ShaderType shaderType, string fileName) {
            if (string.IsNullOrEmpty (fileName)) {
                return "Invalid filename";
            }

            string renderType;
            string renderQueue;
            bool isAlphaBlend;
            switch (shaderType) {
                case ShaderType.Opaque:
                    renderType = "Opaque";
                    renderQueue = "Geometry";
                    isAlphaBlend = false;
                    break;
                case ShaderType.Transparent:
                    renderType = "Transparent";
                    renderQueue = "Transparent";
                    isAlphaBlend = true;
                    break;
                default:
                    return "Unsupported shader type";
            }

            var shaderName = Path.GetFileNameWithoutExtension (fileName);

            try {
                File.WriteAllText (
                    AssetDatabase.GenerateUniqueAssetPath (fileName),
                    GetShaderCode (ShaderTemplate, shaderName, renderType, renderQueue, isAlphaBlend));
            } catch (Exception ex) {
                return ex.Message;
            }
            AssetDatabase.Refresh ();
            return null;
        }

        public enum ShaderType {
            Opaque,

            Transparent
        }
    }
}