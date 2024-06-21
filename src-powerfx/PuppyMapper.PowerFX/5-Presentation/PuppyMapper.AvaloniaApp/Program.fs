namespace CounterApp

open System.Net.Mime
open Avalonia
open Avalonia.Controls.ApplicationLifetimes
open Avalonia.Themes.Fluent
open Avalonia.FuncUI.Hosts
open Avalonia.Controls
open Avalonia.FuncUI
open Avalonia.FuncUI.DSL
open Avalonia.Layout
open PuppyMapper.AvaloniaApp
open PuppyMapper.PowerFX.Service.YmlParser

module Main =

    let view () =
        Component(fun ctx ->
            let state = ctx.useState (new MappingDocumentYml(DocumentName = "Test doc") )

            DockPanel.create [
                DockPanel.children [
                    DockPanel.create [
                        DockPanel.children [
                            TextBlock.create [
                                TextBlock.padding (Thickness 8.0)
                                TextBlock.dock Dock.Left
                                TextBlock.verticalAlignment VerticalAlignment.Center
                                TextBlock.horizontalAlignment HorizontalAlignment.Right
                                TextBlock.text (string "Document Name")
                            ]
                            TextBox.create [
                                TextBox.padding (Thickness 8.0)
                                TextBox.dock Dock.Right
                                TextBox.verticalAlignment VerticalAlignment.Center
                                TextBox.horizontalAlignment HorizontalAlignment.Stretch
                                TextBox.text (string state.Current.DocumentName)
                                TextBox.onTextChanged (fun s -> state.Current.DocumentName <- s)
                            ]
                        ]
                    ]
                ]
            ]
        )

type MainWindow() =
    inherit HostWindow()
    do
        base.Title <- "Counter Example"
        base.Content <- Main.view ()

type App() =
    inherit Application()

    override this.Initialize() =
        this.Styles.Add (FluentTheme())
        this.RequestedThemeVariant <- Styling.ThemeVariant.Dark

    override this.OnFrameworkInitializationCompleted() =
        match this.ApplicationLifetime with
        | :? IClassicDesktopStyleApplicationLifetime as desktopLifetime ->
            desktopLifetime.MainWindow <- MainWindow()
        | _ -> ()

module Program =

    [<EntryPoint>]
    let main(args: string[]) =
        AppBuilder
            .Configure<App>()
            .UsePlatformDetect()
            .UseSkia()
            .StartWithClassicDesktopLifetime(args)
