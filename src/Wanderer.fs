module Wanderer.App

open Elmish
open Elmish.Debug
open Elmish.React
open Fable.Core.JsInterop

open Wanderer.Types

importAll "../css/main.css"

let init () =
    GameStart

let update (msg : Message) model =
    GameStart

let view model dispatch =
    Fable.Helpers.React.div [] [Fable.Helpers.React.str "The Wanderer"]

// App
Program.mkSimple init update view
|> Program.withReact "elmish-app"
//-:cnd
#if DEBUG
|> Program.withDebugger
#endif
//+:cnd
|> Program.run
