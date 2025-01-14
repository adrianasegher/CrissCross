﻿// Copyright (c) 2019-2025 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Collections.ObjectModel;
using System.Windows.Media.Animation;

namespace CrissCross.WPF.UI.Controls;

internal static class VisualStateGroupHelper
{
    private static readonly DependencyProperty CurrentStoryboardsProperty =
        DependencyProperty.RegisterAttached(
            "CurrentStoryboards",
            typeof(Collection<Storyboard>),
            typeof(VisualStateGroupHelper));

    private static readonly Lazy<Action<VisualStateGroup, VisualState>> _setCurrentState =
        new(CreateSetCurrentStateDelegate);

    internal static bool IsSupported => _setCurrentState.Value != null;

    internal static void SetCurrentState(this VisualStateGroup group, VisualState value)
    {
        if (!IsSupported)
        {
            throw new InvalidOperationException();
        }

        _setCurrentState.Value(group, value);
        Debug.Assert(group.CurrentState == value, "Current State.");
    }

    internal static VisualState GetState(this VisualStateGroup group, string stateName)
    {
        for (var stateIndex = 0; stateIndex < group.States.Count; ++stateIndex)
        {
            var state = (VisualState?)group.States[stateIndex];
            if (state?.Name == stateName)
            {
                return state;
            }
        }

        return null!;
    }

    internal static Collection<Storyboard> GetCurrentStoryboards(VisualStateGroup group)
    {
        var currentStoryboards = (Collection<Storyboard>)group.GetValue(CurrentStoryboardsProperty);
        if (currentStoryboards == null)
        {
            currentStoryboards = [];
            group.SetValue(CurrentStoryboardsProperty, currentStoryboards);
        }

        return currentStoryboards;
    }

    internal static void StartNewThenStopOld(this VisualStateGroup group, FrameworkElement element, params Storyboard[] newStoryboards)
    {
        var currentStoryboards = GetCurrentStoryboards(group);

        // Remove the old Storyboards. Remove is delayed until the next TimeManager tick, so the
        // handoff to the new storyboard is unaffected.
        for (var index = 0; index < currentStoryboards.Count; ++index)
        {
            if (currentStoryboards[index] == null)
            {
                continue;
            }

            currentStoryboards[index].Remove(element);
        }

        currentStoryboards.Clear();

        // Start the new Storyboards
        for (var index = 0; index < newStoryboards.Length; ++index)
        {
            if (newStoryboards[index] == null)
            {
                continue;
            }

            newStoryboards[index].Begin(element, HandoffBehavior.SnapshotAndReplace, true);

            // Hold on to the running Storyboards
            currentStoryboards.Add(newStoryboards[index]);

            // Silverlight had an issue where initially, a checked CheckBox would not show the check mark
            // until the second frame. They chose to do a Seek(0) at this point, which this line
            // is supposed to mimic. It does not seem to be equivalent, though, and WPF ends up
            // with some odd animation behavior. I haven't seen the CheckBox issue on WPF, so
            // commenting this out for now.
            // newStoryboards[index].SeekAlignedToLastTick(element, TimeSpan.Zero, TimeSeekOrigin.BeginTime);
        }
    }

    private static Action<VisualStateGroup, VisualState> CreateSetCurrentStateDelegate()
    {
        try
        {
            return DelegateHelper.CreatePropertySetter<VisualStateGroup, VisualState>(
                nameof(VisualStateGroup.CurrentState),
                nonPublic: true);
        }
        catch (Exception)
        {
            return null!;
        }
    }
}
