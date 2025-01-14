﻿// Copyright (c) 2019-2025 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace CrissCross.WPF.UI;

internal class SecondColorDecorator(ISecondColorStorage storage) : IColorStateStorage
{
    public ColorState ColorState
    {
        get => storage.SecondColorState;
        set => storage.SecondColorState = value;
    }
}
