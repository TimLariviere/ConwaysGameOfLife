namespace FSharp

open System
open Xamarin.Forms
open Xamarin.Forms.Xaml
open SkiaSharp
open SkiaSharp.Views.Forms
open Game
open System.Threading

type FSharpPage() as this =
    inherit ContentPage()
    let _ = base.LoadFromXaml(typeof<FSharpPage>)

    let refreshTime = 100.0
    let aliveCellPaint = new SKPaint()

    let mutable _map = Game.createEmptyMap()
    let mutable _timerRunning = false

    // Controls
    let startButton = this.FindByName "StartButton" :> Button
    let restartButton = this.FindByName "RestartButton" :> Button
    let canvasRoot = this.FindByName "CanvasRoot" :> ContentView
    let canvasView = SKCanvasView()

    // Methods
    let onTimerTicked() =
        if _timerRunning then
            _map <- Game.computeNextStep _map
            canvasView.InvalidateSurface()
            true
        else
            false
        
    // Event handlers
    let onStartButtonClicked = EventHandler(fun _ args ->
        if _timerRunning then
            _timerRunning <- false
            startButton.Text <- "Continue"
        else
            _timerRunning <- true
            startButton.Text <- "Pause"
            Device.StartTimer(TimeSpan.FromMilliseconds(refreshTime), (fun () -> onTimerTicked()))
    )

    let onRestartButtonClicked = EventHandler(fun _ args ->
        let computation = async {
          _timerRunning <- false
          _map <- Game.createEmptyMap()
          canvasView.InvalidateSurface()

          do! Async.Sleep (refreshTime |> int)

          _timerRunning <- true
          startButton.Text <- "Pause"
          Device.StartTimer(TimeSpan.FromMilliseconds(refreshTime), (fun () -> onTimerTicked()))
        }

        Async.StartImmediate (computation, CancellationToken.None)
    )

    let onPaintSurface = EventHandler<SKPaintSurfaceEventArgs>(fun _ args ->
        let canvas = args.Surface.Canvas;
        let tileWidth = args.Info.Width / Game.mapWidth
        let tileHeight = args.Info.Height / Game.mapHeight

        canvas.Clear()

        _map
        |> Array.iteri (fun idx value ->
            let x = idx % Game.mapWidth
            let y = idx / Game.mapWidth

            match value with
            | ALIVE -> canvas.DrawRect(new SKRect(x * tileWidth |> float32,
                                                  y * tileHeight |> float32,
                                                  (x+1) * tileWidth |> float32,
                                                  (y+1) * tileHeight |> float32),
                                       aliveCellPaint)
            | DEAD -> ()
        )
    )

    // Lifecycle
    override this.OnAppearing() =
        aliveCellPaint.Style <- SKPaintStyle.Fill
        aliveCellPaint.Color <- Color.Black.ToSKColor()

        startButton.Clicked.AddHandler onStartButtonClicked
        restartButton.Clicked.AddHandler onRestartButtonClicked
        canvasView.PaintSurface.AddHandler onPaintSurface

        canvasRoot.Content <- canvasView

    override this.OnDisappearing() =
        _timerRunning <- false
        startButton.Clicked.RemoveHandler onStartButtonClicked
        restartButton.Clicked.RemoveHandler onRestartButtonClicked
        canvasView.PaintSurface.RemoveHandler onPaintSurface
