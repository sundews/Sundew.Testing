// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BenchmarkReportToPointConverter.cs" company="Sundews">
// Copyright (c) Sundews. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.Testing.Performance;

using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Toolchains.Results;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;
using Sundew.Base;
using Sundew.Base.Collections.Linq;

/// <summary>
/// Provides methods for converting benchmark reports into collections of point data for further analysis or storage.
/// </summary>
/// <remarks>This static class is intended for use in scenarios where benchmark results, such as those produced by
/// BenchmarkDotNet, need to be transformed into a structured format suitable for metrics systems or time-series
/// databases. The conversion process includes tagging and field extraction relevant to performance
/// measurements.</remarks>
public static class BenchmarkReportToPointConverter
{
    /// <summary>
    /// Processes the specified benchmark summaries and extracts measurement point data for successful reports, or collects execution errors for failed reports.
    /// </summary>
    /// <remarks>If all reports in the provided summaries are successful, the result contains the corresponding measurement point data. If any report fails, the result contains the execution errors for the failed reports. The returned result allows callers to distinguish between complete success and partial or total failure.</remarks>
    /// <param name="summaries">An array of benchmark summary objects containing the reports to process. Each summary should include the relevant report and environment information.</param>
    /// <param name="dateTime">The timestamp to associate with each measurement point. This value is applied to all generated point data.</param>
    /// <returns>A result object containing either a collection of measurement point data for all successful reports, or a collection of execution errors if any report failed.</returns>
    public static R<IEnumerable<PointData>, IEnumerable<ExecuteResult>> GetPointResults(Summary[] summaries, DateTime dateTime)
    {
        var allOrFailed = summaries.SelectMany(summary => summary.Reports.Select(report => (summary, report))).AllOrFailed(x =>
        {
            var (summary, report) = x;
            if (report.Success)
            {
                var point = PointData.Measurement(report.BenchmarkCase.Descriptor.DisplayInfo);
                point = point.Tag("Runtime", report.BenchmarkCase.Job.Id)
                    .Tag("CPU", summary.HostEnvironmentInfo.Cpu.Value.ProcessorName)
                    .Tag("Configuration", summary.HostEnvironmentInfo.Configuration);
                var allocated = report.Metrics.FirstOrDefault(x => x.Value.Descriptor.DisplayName == "Allocated");
                if (!Equals(allocated, default))
                {
                    point = point.Field("Allocated", allocated.Value.Value);
                }

                point.Field("Mean", report.ResultStatistics!.Mean)
                    .Field("StdDev", report.ResultStatistics.StandardDeviation)
                    .Field("StdErr", report.ResultStatistics.StandardError)

                    .Timestamp(dateTime, WritePrecision.Ns);
                return Item.Pass<PointData, IReadOnlyList<ExecuteResult>>(point);
            }

            return Item.Fail(report.ExecuteResults);
        });

        return allOrFailed.Map<IEnumerable<PointData>, IEnumerable<ExecuteResult>>(
            x => x.Items,
            x => x.GetErrors().SelectMany(x => x));
    }
}