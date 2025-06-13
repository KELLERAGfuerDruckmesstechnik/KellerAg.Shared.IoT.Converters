namespace KellerAg.Shared.Entities.Filetypes
{
    public static class FileExtensionHelper
    {
        public static string GetFileExtension(Filetype fileType)
        {
            switch (fileType)
            {
                case Filetype.Excel:
                    return ".xlsx";
                case Filetype.Csv:
                    return ".csv";
                case Filetype.Zip:
                    return ".zip";
                case Filetype.Docx:
                    return ".docx";
                case Filetype.Hydras:
                    return ".zip"; //ZipArchive of no special file type is specified but the guys from Amt for Basel can only handle txt
                case Filetype.AquaInfo:
                    return ".csv"; //asc or txt would be ok, too (AquaInfo 'Modul Logger' Kapitel 11 Seite 4)
                case Filetype.Png:
                    return ".png";
                case Filetype.Jpeg:
                    return ".jpeg";
                case Filetype.KellerMeasurementFileFormat:
                    return ".json";
                case Filetype.Pdf:
                    return ".pdf";
                case Filetype.PressureSuiteConfigurationFormat:
                    return ".psuitec";
                default:
                    return ".txt";
            }
        }
        public static string GetFileExtensionDescription(Filetype fileType)
        {
            switch (fileType)
            {
                case Filetype.Excel:
                    return "Excel File (*.xlsx)";
                case Filetype.Csv:
                    return "CSV (*.csv)";
                case Filetype.Zip:
                    return "ZIP (*.zip)";
                case Filetype.Docx:
                    return "Word Document (*.docx)";
                case Filetype.Hydras:
                    return "HYDRAS (txt in *.zip)";
                case Filetype.AquaInfo:
                    return "AquaInfo (*.csv)";
                case Filetype.Png:
                    return "PNG (*.png)";
                case Filetype.Jpeg:
                    return "JPEG (*.jpeg)";
                case Filetype.KellerMeasurementFileFormat:
                    return "PressureSuite File Format (*.json)";
                case Filetype.Pdf:
                    return "PDF (*.pdf)";
                case Filetype.PressureSuiteConfigurationFormat:
                    return "PressureSuite device configuration (*.psuitec)";
                default:
                    return "Textfile (*.txt)";
            }
        }
    }
}
