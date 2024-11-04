
# AECC Grouper
This repository contains an indepently developed C# implementation of the [AECC Grouper](https://www.ihacpa.gov.au/health-care/classification/emergency-care/aecc).

The code has been written for usage within data-pipelines.


## Usage
```csharp
using AeccGrouper;
using AeccGrouper.Reference;
...
// Default to using the embedded Sqlite reference data
var referenceDataService = new ReferenceDataProviderService();
// Create a grouper instance pointed to our reference data
var grouper = new Grouper(referenceDataService);

// Group the episode values
var result = grouper.Group(
    episodeNumber,
    triageCategory,
    episodeEndStatus,
    visitType,
    ageYears,
    transportMode,
    principalDiaignosisShort
    serviceDate);

// Print the results
Console.WriteLine($"Scaled Complexity Score: {result.ScaledComplexityScore}");

Console.WriteLine($"AECC Class: {result.AECC_EndClass}");
```


## What is AECC

The AECC was developed to classify emergency care activity for the purposes of activity based funding.

The AECC applies to emergency departments. Its application to emergency services limited due to data; emergency services report data at an aggregate level, and this does not include key variables used by the AECC (diagnosis, transport mode (arrival) and age group).


## Code Overview

The primary grouper logic is contained within Group.cs. 

The grouper reference tables are stored in Sqlite (`ecdg_values.db`) and are accessed via the `ReferenceDataProviderService`. This could be replaced with different implementations if required (eg by using hard-coded dictionary or arrays). 


## Tests
xUnit tests are provided. More welcome.

## License
This code is licensed under the MIT license.

The AECC Grouper intercepts, coefficients and complexity split thresholds (Appendix D of the AECC Manual)  Copyright (C) Independent Health and Age Care Pricing Authority 2024. 

The values are licensed under a [Creative Commons Attribution-NonCommercial-NoDerivative 4.0 International License](https://creativecommons.org/licenses/by-nc-nd/4.0/deed.en). 

## References
Health Policy Analysis 2019, _Australian Emergency Care Classification: Definitions manual_, Independent Hospital Pricing Authority, Sydney.
