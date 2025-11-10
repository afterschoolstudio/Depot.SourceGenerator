Console.WriteLine("Hello, World!");

// Test the renamed LineNameEnum
var lineNameEnum = Depot.Generated.test.newSheet.LineNameEnum._0;
Console.WriteLine($"Line name enum value: {lineNameEnum}");

// Test AllLines (renamed from Lines)
Console.WriteLine($"Total lines: {Depot.Generated.test.newSheet.AllLines.Count}");

// Test GuidDataMap
var firstLine = Depot.Generated.test.newSheet.AllLines[0];
var lineFromGuidMap = Depot.Generated.test.newSheet.GuidDataMap[firstLine.GUID];
Console.WriteLine($"Found line by GUID: {lineFromGuidMap.ID}");

// Test LineNameEnumDataMap
var lineFromNameMap = Depot.Generated.test.newSheet.LineNameEnumDataMap[Depot.Generated.test.newSheet.LineNameEnum._0];
Console.WriteLine($"Found line by name enum: {lineFromNameMap.ID}");