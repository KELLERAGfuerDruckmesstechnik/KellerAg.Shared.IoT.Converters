using KellerAg.Shared.IoT.Converters; //for the converters
using JsonToBusinessObjects.Conversion; //for some used entities/DTOs
using JsonToBusinessObjects.DataContainers; //for some used entities/DTOs

using System.Text.Json; //for serialization

namespace KellerAg.Shared.IoT.Converters.Tool
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if(args.Length == 0)
            {
                Console.WriteLine("No path/s provided. Please use it like this: \"Keller.Shared.IoT.Converter.Tool.exe C:\\Temp1\\input_file.txt C:\\Temp2\\output_file.json");
                Console.WriteLine("If no second output path is given, the json file is generated at the same path with the same file name but with the file ending with '.json'");
                return;
            }

            if(args.Length > 2)
            {
                Console.WriteLine("Too many arguments. Please use it like this: \"Keller.Shared.IoT.Converter.Tool.exe C:\\Temp1\\input_file.txt C:\\Temp2\\output_file.json");
                Console.WriteLine("If no second output path is given, the json file is generated at the same path with the same file name but with the file ending with '.json'");
                return;
            }

            //check source file path
            if(!File.Exists(args[0]))
            {
                Console.WriteLine($"Source file not found: {args[0]}");
                return;
            }
            string inputFilePath = args[0]; // eg. C:\Temp1\input_file.txt
            string outputFilePath = string.Empty;

            if(args.Length == 1)
            {
                outputFilePath = Path.ChangeExtension(inputFilePath, ".json"); // eg. C:\Temp1\input_file.json
            }
            else
            {
                //check if the output path is valid
                if(!Directory.Exists(Path.GetDirectoryName(args[1])))
                {
                    //try to create missing directory
                    try
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(args[1]));
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine($"Output directory could not be created: {args[1]}");
                        Console.WriteLine($"Error: {ex.Message}");
                        return;
                    }
                }
                outputFilePath = args[1]; // eg. C:\Temp2\output_file.json
            }

            string text_content = File.ReadAllText(inputFilePath);

            //Conversion
            IConvert converter = new KellerAg.Shared.IoT.Converters.IoTConvert();
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
            string json_content = JsonSerializer.Serialize(businessObject);

            //write to file
            File.WriteAllText(outputFilePath, json_content);
            Console.WriteLine($"Output file created: {outputFilePath}");
        }
    }
}
