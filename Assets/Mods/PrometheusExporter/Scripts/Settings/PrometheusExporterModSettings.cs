using ModSettings.Common;
using ModSettings.Core;
using Timberborn.Modding;
using Timberborn.SettingsSystem;

namespace PrometheusExporter.Settings
{
    class PrometheusExporterModSettings : ModSettingsOwner
    {
        protected override string ModId => "PrometheusExporter";
        public override ModSettingsContext ChangeableOn { get; } = ModSettingsContext.All;

        public ModSetting<int> SamplesPerDay { get; } = new RangeIntModSetting(
          defaultValue: 3,
          minValue: 1,
          maxValue: 12,
          descriptor: ModSettingDescriptor.CreateLocalized(
            "rwslinkman.PrometheusExporter.Settings.SamplesPerDay"
          )
        );

        public ModSetting<int> HttpPort { get; } = new (
            defaultValue: 8080,
            descriptor: ModSettingDescriptor.CreateLocalized(
                "rwslinkman.PrometheusExporter.Settings.HttpPort"
            )
        );

        public PrometheusExporterModSettings(ISettings settings, ModSettingsOwnerRegistry modSettingsOwnerRegistry, ModRepository modRepository) : base(settings, modSettingsOwnerRegistry, modRepository)
        {
            // NOP
        }
    }
}