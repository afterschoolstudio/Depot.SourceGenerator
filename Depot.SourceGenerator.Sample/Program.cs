Console.WriteLine("Hello, World!");

// Test the renamed LineName enum
var lineName = Depot.Generated.test.newSheet.LineName.me;
Console.WriteLine($"Line name enum value: {lineName}");

// Test AllLines (renamed from Lines)
Console.WriteLine($"Total lines: {Depot.Generated.test.newSheet.AllLines.Count}");

// Test GuidDataMap
var firstLine = Depot.Generated.test.newSheet.AllLines[0];
var lineFromGuidMap = Depot.Generated.test.newSheet.GuidDataMap[firstLine.GUID];
Console.WriteLine($"Found line by GUID: {lineFromGuidMap.ID}");

// Test LineNameDataMap
var lineFromNameMap = Depot.Generated.test.newSheet.LineNameDataMap[Depot.Generated.test.newSheet.LineName.me];
Console.WriteLine($"Found line by name enum: {lineFromNameMap.ID}");