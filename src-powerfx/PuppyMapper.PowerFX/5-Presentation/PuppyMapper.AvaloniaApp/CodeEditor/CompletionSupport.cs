using System;
using System.Collections.Generic;
using Avalonia.Input;
using AvaloniaEdit;
using AvaloniaEdit.CodeCompletion;
using AvaloniaEdit.Document;
using AvaloniaEdit.Editing;
using Avalonia.Media;

namespace PuppyMapper.AvaloniaApp.CodeEditor;

public class SimpleCompletionData : ICompletionData
{
    public SimpleCompletionData(string text)
    {
        Text = text;
    }

    public string Text { get; }

    // Displayed in the completion list
    public object Content => Text;

    // Optional description shown in the tooltip
    public object Description => null;

    // Priority for sorting completions
    public double Priority => 0;

    // Some implementations expose an Image; AvaloniaEdit's ICompletionData expects an IImage
    public IImage Image => null;

    // Called when the user accepts the completion item
    public void Complete(TextArea textArea, ISegment completionSegment, EventArgs insertionRequestEventArgs)
    {
        if (textArea?.Document == null) return;
        textArea.Document.Replace(completionSegment, Text);
    }
}

public static class CompletionSupport
{
    // Attach a simple completion provider to a TextEditor
    public static void AttachCompletion(TextEditor editor, IEnumerable<string> suggestions)
    {
        if (editor == null) return;
        CompletionWindow? completionWindow = null;

        void ShowCompletionAt(int startOffset)
        {
            // Close previous window if any
            completionWindow?.Close();

            completionWindow = new CompletionWindow(editor.TextArea);
            var data = completionWindow.CompletionList.CompletionData;
            foreach (var s in suggestions)
            {
                data.Add(new SimpleCompletionData(s));
            }

            // Try to set the initial completion segment to the current word
            var caret = editor.CaretOffset;
            int wordStart = GetWordStart(editor.Document, caret);
            completionWindow.StartOffset = wordStart;
            completionWindow.Show();
            completionWindow.Closed += (s, e) => completionWindow = null;
        }

        // Listen for text being entered; show completion when typing letters/digits/_
        editor.TextArea.TextEntered += (sender, e) =>
        {
            if (string.IsNullOrEmpty(e.Text)) return;
            var ch = e.Text[0];
            if (char.IsLetterOrDigit(ch) || ch == '_' )
            {
                // minimal heuristic: show suggestions when current token >= 1
                var caret = editor.CaretOffset;
                int wordStart = GetWordStart(editor.Document, caret);
                var len = caret - wordStart;
                if (len >= 1)
                {
                    ShowCompletionAt(wordStart);
                }
            }
        };

        // Ctrl+Space to trigger explicitly
        editor.TextArea.KeyDown += (sender, e) =>
        {
            if (e.Key == Key.Space && (e.KeyModifiers & KeyModifiers.Control) == KeyModifiers.Control)
            {
                var caret = editor.CaretOffset;
                int wordStart = GetWordStart(editor.Document, caret);
                ShowCompletionAt(wordStart);
                e.Handled = true;
            }
        };
    }

    // Find start of a word/token at offset (document offsets use 0..Length)
    private static int GetWordStart(TextDocument doc, int offset)
    {
        if (doc == null) return offset;
        int pos = Math.Max(0, offset - 1);
        while (pos >= 0)
        {
            var ch = doc.GetCharAt(pos);
            if (!(char.IsLetterOrDigit(ch) || ch == '_' ))
                return pos + 1;
            pos--;
        }
        return 0;
    }
}
