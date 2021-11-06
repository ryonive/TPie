﻿using ImGuiNET;
using Newtonsoft.Json;
using System.Numerics;
using TPie.Helpers;

namespace TPie.Models.Elements
{
    public class GearSetElement : RingElement
    {
        public uint GearSetID;
        public bool UseID;
        public string GearSetName;
        public bool DrawText;

        private uint _jobId;

        [JsonProperty]
        public uint JobID
        {
            get => _jobId;
            set
            {
                _jobId = value;
                IconID = 62800 + value;
            }
        }

        public GearSetElement(uint gearSetId, bool useId, string? name, bool drawText, uint jobId)
        {
            GearSetID = gearSetId;
            UseID = useId;
            GearSetName = name ?? "";
            DrawText = drawText;
            JobID = jobId;
        }

        public GearSetElement() : this(1, true, null, true, Plugin.ClientState.LocalPlayer?.ClassJob.Id ?? JobIDs.GLD) { }

        public override void ExecuteAction()
        {
            if (UseID)
            {
                ChatHelper.SendChatMessage($"/gs change {GearSetID}");
            }
            else
            {
                ChatHelper.SendChatMessage($"/gs change {GearSetName}");
            }
        }

        public override bool IsValid()
        {
            return true;
        }

        public override string InvalidReason()
        {
            return "";
        }

        public override string Description()
        {
            if (JobsHelper.JobNames.TryGetValue(JobID, out string? value) && value != null)
            {
                if (UseID)
                {
                    return $"{value} ({GearSetID})";
                }

                return value == GearSetName ? value : $"{value} ({GearSetName})";
            }

            return "";
        }

        public override void Draw(Vector2 position, Vector2 size, float scale, bool selected, uint color, float alpha, ImDrawListPtr drawList)
        {
            base.Draw(position, size, scale, selected, color, alpha, drawList);

            if (!DrawText) return;

            size = size * scale;

            string text = UseID ? $"{GearSetID}" : $"{GearSetName}";
            Vector2 textPos = UseID ? position + (size / 2f) - new Vector2(2 * scale) : position;
            DrawHelper.DrawOutlinedText(text, textPos, true, scale, drawList);
        }
    }
}
