﻿using ImGuiNET;
using ImGuiScene;
using System.Numerics;
using System.Text.RegularExpressions;
using TPie.Helpers;
using TPie.Models.Elements;

namespace TPie.Config
{
    public class MacroElementWindow : RingElementWindow
    {
        private MacroElement? _macroElement = null;
        public MacroElement? MacroElement
        {
            get => _macroElement;
            set
            {
                _editing = true;
                _macroElement = value;

                _inputText = value != null ? value.Name : "";
                _commandInputText = value != null ? value.Command : "";
                _iconInputText = value != null ? $"{value.IconID}" : "";
            }
        }

        protected string _commandInputText = "";
        protected string _iconInputText = "";

        public MacroElementWindow(string name) : base(name)
        {

        }

        protected override void CreateElement()
        {
            _macroElement = new MacroElement("New Macro", "", 66001);
            _inputText = _macroElement!.Name;
            _iconInputText = "66001";
        }

        protected override void DestroyElement()
        {
            MacroElement = null;
        }

        protected override RingElement? Element()
        {
            return MacroElement;
        }

        public override void Draw()
        {
            if (MacroElement == null) return;

            ImGui.PushItemWidth(210);

            // name
            FocusIfNeeded();
            if (ImGui.InputText("Name ##Macro", ref _inputText, 100))
            {
                MacroElement.Name = _inputText;
            }

            // command
            if (ImGui.InputText("Command ##Macro", ref _commandInputText, 100))
            {
                MacroElement.Command = _commandInputText;
            }

            ImGui.NewLine();
            ImGui.NewLine();

            ImGui.PushItemWidth(50);
            if (ImGui.Button("Default"))
            {
                MacroElement.IconID = 66001;
                _iconInputText = "66001";
            }

            // icon id
            ImGui.SameLine();
            ImGui.PushItemWidth(154);
            string str = _iconInputText;
            if (ImGui.InputText("Icon ID ##Macro", ref str, 100, ImGuiInputTextFlags.CharsDecimal))
            {
                _iconInputText = Regex.Replace(str, @"[^\d]", "");

                try
                {
                    MacroElement.IconID = uint.Parse(_iconInputText);
                }
                catch
                {
                    MacroElement.IconID = 0;
                }
            }

            ImGui.NewLine();

            if (MacroElement.IconID > 0)
            {
                if (ImGui.Selectable("", false, ImGuiSelectableFlags.None, new Vector2(0, 80)))
                {
                    Callback?.Invoke(MacroElement);
                    Callback = null;
                    IsOpen = false;
                    return;
                }

                // icon
                TextureWrap? texture = TexturesCache.Instance?.GetTextureFromIconId(MacroElement.IconID);
                if (texture != null)
                {
                    ImGui.SameLine();
                    ImGui.SetCursorPosX(110);
                    ImGui.Image(texture.ImGuiHandle, new Vector2(80));
                }
            }
        }
    }
}