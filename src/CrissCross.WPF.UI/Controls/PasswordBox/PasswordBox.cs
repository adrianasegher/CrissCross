// Copyright (c) 2019-2024 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Windows.Controls;

namespace CrissCross.WPF.UI.Controls;

/// <summary>
/// The modified password control. TextProperty contains asterisks OR raw password if IsPasswordRevealed is set to true, PasswordProperty always contains raw password.
/// </summary>
public class PasswordBox : TextBox
{
    /// <summary>
    /// Property for <see cref="Password"/>.
    /// </summary>
    public static readonly DependencyProperty PasswordProperty = DependencyProperty.Register(
        nameof(Password),
        typeof(string),
        typeof(PasswordBox),
        new PropertyMetadata(string.Empty, OnPasswordPropertyChanged));

    /// <summary>
    /// Property for <see cref="PasswordChar"/>.
    /// </summary>
    public static readonly DependencyProperty PasswordCharProperty = DependencyProperty.Register(
        nameof(PasswordChar),
        typeof(char),
        typeof(PasswordBox),
        new PropertyMetadata('*', OnPasswordCharPropertyChanged));

    /// <summary>
    /// Property for <see cref="IsPasswordRevealed"/>.
    /// </summary>
    public static readonly DependencyProperty IsPasswordRevealedProperty = DependencyProperty.Register(
        nameof(IsPasswordRevealed),
        typeof(bool),
        typeof(PasswordBox),
        new PropertyMetadata(false, OnPasswordRevealModePropertyChanged));

    /// <summary>
    /// Property for <see cref="RevealButtonEnabled"/>.
    /// </summary>
    public static readonly DependencyProperty RevealButtonEnabledProperty = DependencyProperty.Register(
        nameof(RevealButtonEnabled),
        typeof(bool),
        typeof(PasswordBox),
        new PropertyMetadata(true));

    /// <summary>
    /// Event for "Password has changed".
    /// </summary>
    public static readonly RoutedEvent PasswordChangedEvent = EventManager.RegisterRoutedEvent(
        nameof(PasswordChanged),
        RoutingStrategy.Bubble,
        typeof(RoutedEventHandler),
        typeof(PasswordBox));

    private bool _lockUpdatingContents;

    /// <summary>
    /// Initializes a new instance of the <see cref="PasswordBox"/> class.
    /// </summary>
    public PasswordBox() => _lockUpdatingContents = false;

    /// <summary>
    /// Event fired from this text box when its inner content
    /// has been changed.
    /// </summary>
    /// <remarks>
    /// It is redirected from inner TextContainer.Changed event.
    /// </remarks>
    public event RoutedEventHandler PasswordChanged
    {
        add => AddHandler(PasswordChangedEvent, value);
        remove => RemoveHandler(PasswordChangedEvent, value);
    }

    /// <summary>
    /// Gets or sets currently typed text represented by asterisks.
    /// </summary>
    public string Password
    {
        get => (string)GetValue(PasswordProperty);
        set => SetValue(PasswordProperty, value);
    }

    /// <summary>
    /// Gets or sets character used to mask the password.
    /// </summary>
    public char PasswordChar
    {
        get => (char)GetValue(PasswordCharProperty);
        set => SetValue(PasswordCharProperty, value);
    }

    /// <summary>
    /// Gets a value indicating whether the password is revealed.
    /// </summary>
    public bool IsPasswordRevealed
    {
        get => (bool)GetValue(IsPasswordRevealedProperty);
        private set => SetValue(IsPasswordRevealedProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether gets or sets a value deciding whether to display the reveal password button.
    /// </summary>
    public bool RevealButtonEnabled
    {
        get => (bool)GetValue(RevealButtonEnabledProperty);
        set => SetValue(RevealButtonEnabledProperty, value);
    }

    /// <inheritdoc />
    protected override void OnTextChanged(TextChangedEventArgs e)
    {
        UpdateTextContents(true);

        if (_lockUpdatingContents)
        {
            base.OnTextChanged(e);
        }
        else
        {
            SetPlaceholderTextVisibility();
            RevealClearButton();
        }
    }

    /// <summary>
    /// Is called when <see cref="Password"/> property is changing.
    /// </summary>
    protected virtual void OnPasswordChanged() => UpdateTextContents(false);

    /// <summary>
    /// Is called when <see cref="PasswordChar"/> property is changing.
    /// </summary>
    protected virtual void OnPasswordCharChanged()
    {
        // If password is currently revealed,
        // do not replace displayed text with asterisks
        if (IsPasswordRevealed)
        {
            return;
        }

        _lockUpdatingContents = true;

        Text = new string(PasswordChar, Password.Length);

        _lockUpdatingContents = false;
    }

    /// <summary>
    /// Called when [password reveal mode changed].
    /// </summary>
    protected virtual void OnPasswordRevealModeChanged()
    {
        _lockUpdatingContents = true;

        Text = IsPasswordRevealed ? Password : new string(PasswordChar, Password.Length);

        _lockUpdatingContents = false;
    }

    /// <summary>
    /// Triggered by clicking a button in the control template.
    /// </summary>
    /// <param name="parameter">Additional parameters.</param>
    protected override void OnTemplateButtonClick(string? parameter)
    {
#if DEBUG
        System.Diagnostics.Debug.WriteLine(
            $"INFO: {typeof(PasswordBox)} button clicked with param: {parameter}",
            "CrissCross.WPF.UI.PasswordBox");
#endif

        switch (parameter)
        {
            case "reveal":
                IsPasswordRevealed = !IsPasswordRevealed;
                Focus();
                CaretIndex = Text.Length;
                break;
            default:
                base.OnTemplateButtonClick(parameter);
                break;
        }
    }

    /// <summary>
    /// Called when <see cref="Password"/> is changed.
    /// </summary>
    private static void OnPasswordPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not PasswordBox control)
        {
            return;
        }

        control.OnPasswordChanged();
    }

    /// <summary>
    /// Called if the character is changed in the during the run.
    /// </summary>
    private static void OnPasswordCharPropertyChanged(
        DependencyObject d,
        DependencyPropertyChangedEventArgs e)
    {
        if (d is not PasswordBox control)
        {
            return;
        }

        control.OnPasswordCharChanged();
    }

    /// <summary>
    /// Called if the reveal mode is changed in the during the run.
    /// </summary>
    private static void OnPasswordRevealModePropertyChanged(
        DependencyObject d,
        DependencyPropertyChangedEventArgs e)
    {
        if (d is not PasswordBox control)
        {
            return;
        }

        control.OnPasswordRevealModeChanged();
    }

    private void UpdateTextContents(bool isTriggeredByTextInput)
    {
        if (_lockUpdatingContents)
        {
            return;
        }

        if (IsPasswordRevealed)
        {
            if (Password == Text)
            {
                return;
            }

            _lockUpdatingContents = true;

            if (isTriggeredByTextInput)
            {
                Password = Text;
            }
            else
            {
                Text = Password;
                CaretIndex = Text.Length;
            }

            RaiseEvent(new RoutedEventArgs(PasswordChangedEvent));

            _lockUpdatingContents = false;

            return;
        }

        var caretIndex = CaretIndex;
        var selectionIndex = SelectionStart;
        var currentPassword = Password ?? string.Empty;
        var newPasswordValue = currentPassword;

        if (isTriggeredByTextInput)
        {
            var currentText = Text;
            var newCharacters = currentText.Replace(PasswordChar.ToString(), string.Empty);

            if (currentText.Length < currentPassword.Length)
            {
                newPasswordValue = currentPassword.Remove(selectionIndex, currentPassword.Length - currentText.Length);
            }

            if (newCharacters.Length > 1)
            {
                var index = currentText.IndexOf(newCharacters[0]);

                newPasswordValue =
                    index > newPasswordValue.Length - 1
                        ? newPasswordValue + newCharacters
                        : newPasswordValue.Insert(index, newCharacters);
            }
            else
            {
                for (var i = 0; i < currentText.Length; i++)
                {
                    if (currentText[i] == PasswordChar)
                    {
                        continue;
                    }

                    newPasswordValue =
                        currentText.Length == newPasswordValue.Length
                            ? newPasswordValue.Remove(i, 1).Insert(i, currentText[i].ToString())
                            : newPasswordValue.Insert(i, currentText[i].ToString());
                }
            }
        }

        _lockUpdatingContents = true;

        Text = new string(PasswordChar, newPasswordValue?.Length ?? 0);
        Password = newPasswordValue ?? string.Empty;
        CaretIndex = caretIndex;

        RaiseEvent(new RoutedEventArgs(PasswordChangedEvent));

        _lockUpdatingContents = false;
    }
}
