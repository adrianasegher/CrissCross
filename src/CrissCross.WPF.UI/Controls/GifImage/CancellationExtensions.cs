﻿// Copyright (c) 2019-2025 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace CrissCross.WPF.UI.Controls;

internal static class CancellationExtensions
{
    public static async Task WithCancellationToken(this Task task, CancellationToken cancellationToken) => await Task.WhenAny(task, cancellationToken.WhenCanceled());

    public static async Task<T> WithCancellationToken<T>(this Task<T> task, CancellationToken cancellationToken)
    {
        var firstTaskToFinish = await Task.WhenAny(task, cancellationToken.WhenCanceled());
        if (firstTaskToFinish == task)
        {
            return await task;
        }

        await firstTaskToFinish;

        // Will never be reached because the previous statement will throw, but necessary to satisfy the compiler
        throw new OperationCanceledException(cancellationToken);
    }

    public static Task WhenCanceled(this CancellationToken cancellationToken)
    {
        var tcs = new TaskCompletionSource<int>();
        var registration = default(CancellationTokenRegistration);
        registration = cancellationToken.Register(
            o =>
        {
            ((TaskCompletionSource<int>)o).TrySetCanceled();

            // ReSharper disable once AccessToModifiedClosure
            registration.Dispose();
        },
            tcs);
        return tcs.Task;
    }
}
