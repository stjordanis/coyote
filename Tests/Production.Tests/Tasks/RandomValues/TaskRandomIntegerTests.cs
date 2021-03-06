﻿// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.Coyote.Random;
using Microsoft.Coyote.Specifications;
using Microsoft.Coyote.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.Coyote.Production.Tests.Tasks
{
    public class TaskRandomIntegerTests : BaseProductionTest
    {
        public TaskRandomIntegerTests(ITestOutputHelper output)
            : base(output)
        {
        }

        [Fact(Timeout = 5000)]
        public void TestRandomIntegerInSynchronousTask()
        {
            this.TestWithError(async () =>
            {
                Generator generator = Generator.Create();
                SharedEntry entry = new SharedEntry();

                async Task WriteAsync()
                {
                    await Task.CompletedTask;
                    if (generator.NextInteger(5) == 0)
                    {
                        entry.Value = 3;
                    }
                    else
                    {
                        entry.Value = 5;
                    }
                }

                await WriteAsync();
                Specification.Assert(entry.Value == 5, "Value is {0} instead of 5.", entry.Value);
            },
            configuration: GetConfiguration().WithTestingIterations(200),
            expectedError: "Value is 3 instead of 5.",
            replay: true);
        }

        [Fact(Timeout = 5000)]
        public void TestRandomIntegerInAsynchronousTask()
        {
            this.TestWithError(async () =>
            {
                Generator generator = Generator.Create();
                SharedEntry entry = new SharedEntry();

                async Task WriteWithDelayAsync()
                {
                    await Task.Delay(1);
                    if (generator.NextInteger(5) == 0)
                    {
                        entry.Value = 3;
                    }
                    else
                    {
                        entry.Value = 5;
                    }
                }

                await WriteWithDelayAsync();
                Specification.Assert(entry.Value == 5, "Value is {0} instead of 5.", entry.Value);
            },
            configuration: GetConfiguration().WithTestingIterations(200),
            expectedError: "Value is 3 instead of 5.",
            replay: true);
        }

        [Fact(Timeout = 5000)]
        public void TestRandomIntegerInParallelTask()
        {
            this.TestWithError(async () =>
            {
                Generator generator = Generator.Create();
                SharedEntry entry = new SharedEntry();

                await Task.Run(() =>
                {
                    if (generator.NextInteger(5) == 0)
                    {
                        entry.Value = 3;
                    }
                    else
                    {
                        entry.Value = 5;
                    }
                });

                Specification.Assert(entry.Value == 5, "Value is {0} instead of 5.", entry.Value);
            },
            configuration: GetConfiguration().WithTestingIterations(200),
            expectedError: "Value is 3 instead of 5.",
            replay: true);
        }

        [Fact(Timeout = 5000)]
        public void TestRandomIntegerInParallelSynchronousTask()
        {
            this.TestWithError(async () =>
            {
                Generator generator = Generator.Create();
                SharedEntry entry = new SharedEntry();

                await Task.Run(async () =>
                {
                    await Task.CompletedTask;
                    if (generator.NextInteger(5) == 0)
                    {
                        entry.Value = 3;
                    }
                    else
                    {
                        entry.Value = 5;
                    }
                });

                Specification.Assert(entry.Value == 5, "Value is {0} instead of 5.", entry.Value);
            },
            configuration: GetConfiguration().WithTestingIterations(200),
            expectedError: "Value is 3 instead of 5.",
            replay: true);
        }

        [Fact(Timeout = 5000)]
        public void TestRandomIntegerInParallelAsynchronousTask()
        {
            this.TestWithError(async () =>
            {
                Generator generator = Generator.Create();
                SharedEntry entry = new SharedEntry();

                await Task.Run(async () =>
                {
                    await Task.Delay(1);
                    if (generator.NextInteger(5) == 0)
                    {
                        entry.Value = 3;
                    }
                    else
                    {
                        entry.Value = 5;
                    }
                });

                Specification.Assert(entry.Value == 5, "Value is {0} instead of 5.", entry.Value);
            },
            configuration: GetConfiguration().WithTestingIterations(200),
            expectedError: "Value is 3 instead of 5.",
            replay: true);
        }

        [Fact(Timeout = 5000)]
        public void TestRandomIntegerInNestedParallelSynchronousTask()
        {
            this.TestWithError(async () =>
            {
                Generator generator = Generator.Create();
                SharedEntry entry = new SharedEntry();

                await Task.Run(async () =>
                {
                    await Task.Run(async () =>
                    {
                        await Task.CompletedTask;
                        if (generator.NextInteger(5) == 0)
                        {
                            entry.Value = 3;
                        }
                        else
                        {
                            entry.Value = 5;
                        }
                    });
                });

                Specification.Assert(entry.Value == 5, "Value is {0} instead of 5.", entry.Value);
            },
            configuration: GetConfiguration().WithTestingIterations(200),
            expectedError: "Value is 3 instead of 5.",
            replay: true);
        }
    }
}
