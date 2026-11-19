# BUGS

The starter contains behaviors that contradict `ASSIGNMENT.md`. Write here every discrepancy you found.
Fill it in yourself, in your own words. There is no required number: what counts is that they are real.

| Which rule from ASSIGNMENT.md is violated (R1–R8) | Where the problem was in the starter (file, approximate line)                                       | How you discovered it (test, reading, AI) | What you changed                                                          |
| ------------------------------------------------- | --------------------------------------------------------------------------------------------------- | ----------------------------------------- | ------------------------------------------------------------------------- |
| **R5**                                            | `Services/BookingService.cs`, `Create`, ~line 17–24                                                 | test, reading, AI                         | Added a check `startUtc < now` → 400 validation error                     |
| **R6**                                            | `Services/BookingService.cs`, `Create`, ~line 17–24                                                 | test, reading, AI                         | Added `endUtc - startUtc > TimeSpan.FromHours(4)` → 400                   |
| **R7**                                            | `Services/BookingService.cs`, line 28: `existing.StartUtc <= endUtc && startUtc <= existing.EndUtc` | test, reading, AI                         | Changed to strict comparison                                              |
| **R7 / R8**                                       | `Services/BookingService.cs`, line 26                                                               | test, reading, AI                         | Only bookings with `Status == Active` are considered in the overlap check |
| **R8**                                            | `Services/BookingService.cs`, `Cancel`, line 56–60                                                  | test, reading, AI                         | Cancel now checks the current status                                      |
