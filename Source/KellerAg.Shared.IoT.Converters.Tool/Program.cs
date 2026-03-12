using JsonToBusinessObjects.Conversion; //for some used entities/DTOs
using JsonToBusinessObjects.DataContainers; //for some used entities/DTOs

using System.Text.Json; //for serialization

namespace KellerAg.Shared.IoT.Converters.Tool
{
    internal class Program
    {
        private const string UsageText =
            "Usage: Keller.Shared.IoT.Converter.Tool.exe <input_file> [output_file] [--format json|txt1|txt2]\n" +
            "\n" +
            "  <input_file>     Path to the GSM/ARC communication text file to convert.\n" +
            "  [output_file]    Optional output file path. Defaults to the input path with\n" +
            "                   the appropriate extension (.json, .txt1.txt, or .txt2.txt).\n" +
            "  --format <fmt>   Output format. One of:\n" +
            "                     json  (default) Serialize the BusinessObject to JSON.\n" +
            "                     txt1  TOPKAPI tabulated ASCII: one row per timestamp,\n" +
            "                           columns are space-separated channel values.\n" +
            "                     txt2  TOPKAPI tabulated ASCII: one row per timestamp/channel,\n" +
            "                           columns are timestamp, variable name, value.\n" +
            "\n" +
            "Examples:\n" +
            "  Tool.exe C:\\data\\input.txt\n" +
            "  Tool.exe C:\\data\\input.txt C:\\out\\result.json\n" +
            "  Tool.exe C:\\data\\input.txt C:\\out\\result.txt --format txt1\n" +
            "  Tool.exe C:\\data\\input.txt C:\\out\\result.txt --format txt2\n";

        static void Main(string[] args)
        {
            if(args.Length == 0)
            {
                Console.WriteLine(UsageText);
                return;
            }

            // Parse arguments: positional args are input and optional output; --format flag is optional.
            string? inputFilePath = null;
            string? outputFilePath = null;
            string format = "json";

            for(int i = 0; i < args.Length; i++)
            {
                if(args[i].Equals("--format", StringComparison.OrdinalIgnoreCase))
                {
                    if(i + 1 >= args.Length)
                    {
                        Console.WriteLine("Error: --format requires a value (json, txt1, or txt2).");
                        Console.WriteLine(UsageText);
                        return;
                    }
                    format = args[++i].ToLowerInvariant();
                    if(format != "json" && format != "txt1" && format != "txt2")
                    {
                        Console.WriteLine($"Error: Unknown format '{format}'. Use json, txt1, or txt2.");
                        Console.WriteLine(UsageText);
                        return;
                    }
                }
                else if(inputFilePath == null)
                {
                    inputFilePath = args[i];
                }
                else if(outputFilePath == null)
                {
                    outputFilePath = args[i];
                }
                else
                {
                    Console.WriteLine($"Error: Unexpected argument '{args[i]}'.");
                    Console.WriteLine(UsageText);
                    return;
                }
            }

            if(inputFilePath == null)
            {
                Console.WriteLine("Error: No input file path provided.");
                Console.WriteLine(UsageText);
                return;
            }

            //check source file path
            if(!File.Exists(inputFilePath))
            {
                Console.WriteLine($"Source file not found: {inputFilePath}");
                return;
            }

            // Derive output path if not specified
            if(outputFilePath == null)
            {
                string defaultExtension = format switch
                {
                    "txt1" => ".txt1.txt",
                    "txt2" => ".txt2.txt",
                    _      => ".json",
                };
                outputFilePath = Path.ChangeExtension(inputFilePath, null) + defaultExtension;
            }

            //check if the output directory is valid, and create it if needed
            string? outputDir = Path.GetDirectoryName(outputFilePath);
            if(!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                try
                {
                    Directory.CreateDirectory(outputDir);
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"Output directory could not be created: {outputDir}");
                    Console.WriteLine($"Error: {ex.Message}");
                    return;
                }
            }

            string text_content = File.ReadAllText(inputFilePath);

            //Conversion
            IoTConvert converter = new KellerAg.Shared.IoT.Converters.IoTConvert();
            string gsmCommunicationJsonText = converter.GsmCommunicationToJson(text_content); // Gets the text content in Json format
            ConversionResult conversionResult = converter.GsmCommunicationJsonToBusinessObject(gsmCommunicationJsonText);

            if(conversionResult.HasErrors)
            {
                foreach(var error in conversionResult.ConversionMessages.Errors)
                {
                    Console.WriteLine($"Error: {error}");
                }
                return;
            }

            if(conversionResult.HasWarnings)
            {
                foreach(JsonToBusinessObjects.Conversion.Messages.ConversionMessage? warnings in conversionResult.ConversionMessages.Warnings)
                {
                    Console.WriteLine($"Warnings: {warnings.Message}");
                }
            }

            BusinessObjectRoot businessObject = conversionResult.BusinessObjectRoot;
            string output_content;

            switch(format)
            {
                case "txt1":
                    output_content = converter.BusinessObjectToTxt1(businessObject);
                    break;
                case "txt2":
                    output_content = converter.BusinessObjectToTxt2(businessObject);
                    break;
                default:
                    var jsonOptions = new JsonSerializerOptions
                    {
                        NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowNamedFloatingPointLiterals
                    };
                    output_content = JsonSerializer.Serialize(businessObject, jsonOptions);
                    break;
            }

            //write to file
            File.WriteAllText(outputFilePath, output_content);
            Console.WriteLine($"Output file created: {outputFilePath}");
        }
    }
}
