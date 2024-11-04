// See https://aka.ms/new-console-template for more information
using AeccGrouper.Reference;
using Dumpify;
using System.Globalization;
using AeccGrouper;
using CsvHelper.Configuration;


// Validate the number of arguments
if (args.Length != 2)
{
    Console.WriteLine("Usage: grouper.exe infile.csv outfile.csv");
    return;
}



var inputFileName = args[0];
var outputFileName = args[1];

// Validate input file existence
if (!File.Exists(inputFileName))
{
    Console.WriteLine($"Error: Input file '{inputFileName}' does not exist.");
    return;
}

// Check if the output file's directory exists
string outputDirectory = Path.GetDirectoryName(outputFileName);
if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
{
    Console.WriteLine($"Error: Output directory '{outputDirectory}' does not exist.");
    return;
}



var referenceDataService = new ReferenceDataProviderService();

var grouper = new Grouper(referenceDataService);

var csvOptions = new CsvConfiguration(CultureInfo.CurrentCulture)
{
    IgnoreBlankLines = true,
    HasHeaderRecord = true,
};

try
{
    // Parse the input file
    using var reader = new StreamReader(inputFileName);
    using var csvReader = new CsvHelper.CsvReader(reader, csvOptions);
    var records = csvReader.GetRecords<AeccFile>().ToList();

    foreach (var record in records)
    {
        var result = grouper.Group(
            record.stateid,
            record.edtriag,
            record.eddepst,
            record.edvisit,
            record.ageyears,
            record.transmode,
            record.x11ddx1,
            record.servdate);
        record.AECC_EndClass = result.AECC_EndClass;
        record.ComplexityScore = result.ScaledComplexityScore;
        record.ECDG_Subgroup = result.ECDG_Subgroup;

    }

    using var writer = new StreamWriter(outputFileName);
    using var csvWriter = new CsvHelper.CsvWriter(writer, csvOptions);
    csvWriter.WriteRecords(records);
}
catch (Exception ex)
{
        Console.WriteLine($"Could not process file: {ex.Message}");
}
finally
{
        referenceDataService.Dispose();
}