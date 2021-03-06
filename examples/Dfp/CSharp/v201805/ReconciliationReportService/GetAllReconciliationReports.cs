// Copyright 2017, Google Inc. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
using Google.Api.Ads.Dfp.Lib;
using Google.Api.Ads.Dfp.Util.v201805;
using Google.Api.Ads.Dfp.v201805;
using System;

namespace Google.Api.Ads.Dfp.Examples.CSharp.v201805 {
  /// <summary>
  /// This example gets all reconciliation reports.
  /// </summary>
  public class GetAllReconciliationReports : SampleBase {
    /// <summary>
    /// Returns a description about the code example.
    /// </summary>
    public override string Description {
      get {
        return "This example gets all reconciliation reports.";
      }
    }

    /// <summary>
    /// Main method, to run this code example as a standalone application.
    /// </summary>
    public static void Main() {
      GetAllReconciliationReports codeExample = new GetAllReconciliationReports();
      Console.WriteLine(codeExample.Description);
      try {
        codeExample.Run(new DfpUser());
      } catch (Exception e) {
        Console.WriteLine("Failed to get reconciliation reports. Exception says \"{0}\"",
            e.Message);
      }
    }

    /// <summary>
    /// Run the code example.
    /// </summary>
    public void Run(DfpUser dfpUser) {
      using (ReconciliationReportService reconciliationReportService =
          (ReconciliationReportService) dfpUser.GetService(
              DfpService.v201805.ReconciliationReportService)) {

        // Create a statement to select reconciliation reports.
        int pageSize = StatementBuilder.SUGGESTED_PAGE_LIMIT;
        StatementBuilder statementBuilder = new StatementBuilder()
            .OrderBy("id ASC")
            .Limit(pageSize);

        // Retrieve a small amount of reconciliation reports at a time, paging through until all
        // reconciliation reports have been retrieved.
        int totalResultSetSize = 0;
        do {
          ReconciliationReportPage page =
              reconciliationReportService.getReconciliationReportsByStatement(
                  statementBuilder.ToStatement());

          // Print out some information for each reconciliation report.
          if (page.results != null) {
            totalResultSetSize = page.totalResultSetSize;
            int i = page.startIndex;
            foreach (ReconciliationReport reconciliationReport in page.results) {
              String startDateString = new System.DateTime(
                  day: reconciliationReport.startDate.day,
                  month: reconciliationReport.startDate.month,
                  year: reconciliationReport.startDate.year
              ).ToString("d");
              Console.WriteLine(
                  "{0}) Reconciliation report with ID {1} and start date \"{2}\" was found.",
                  i++,
                  reconciliationReport.id,
                  startDateString
              );
            }
          }

          statementBuilder.IncreaseOffsetBy(pageSize);
        } while (statementBuilder.GetOffset() < totalResultSetSize);

        Console.WriteLine("Number of results found: {0}", totalResultSetSize);
      }
    }
  }
}
