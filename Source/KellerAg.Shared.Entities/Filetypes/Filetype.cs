namespace KellerAg.Shared.Entities.Filetypes
{
    public enum Filetype
    {
        Csv      = 0,
        Excel    = 1,
        Zip      = 2,
        Docx     = 3,
        Hydras   = 4,
        AquaInfo = 5,

        /// <summary>
        /// PressureSuite File Format (Json)
        /// </summary>
        KellerMeasurementFileFormat = 6,
        Png      = 7,
        Jpeg     = 8,
        Pdf      = 9,
        /// <summary>
        /// PressureSuite Device configuration file format (Json)
        /// </summary>
        PressureSuiteConfigurationFormat = 10,
    }
}