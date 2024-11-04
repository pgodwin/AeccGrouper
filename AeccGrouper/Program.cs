// C# implementation of AeccGrouper for integration with other applications.


using AeccGrouper.Reference;
using AeccGrouper;
using System.Globalization;
using Dumpify;

var connectionString = "Data Source=ecdg_values.db";

var referenceDataService = new ReferenceDataProviderService(connectionString);

var grouper = new Grouper(referenceDataService);

var input = "aecc_input.csv";

// Parse the input file
using var reader = new StreamReader(input);
var csvReader = new CsvHelper.CsvReader(reader, CultureInfo.CurrentCulture);
var records = csvReader.GetRecords<AeccInput>().ToList();

var results = records.Select(record=> grouper.Group(
    record.stateid,
    record.edtriag,
    record.eddepst,
    record.edvisit,
    record.ageyears,
    record.transmode,
    record.x11ddx1,
    record.servdate
    )).ToList();


results.DumpConsole();

Console.ReadLine();