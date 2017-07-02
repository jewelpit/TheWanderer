module Wanderer.App

open Elmish
open Elmish.Debug
open Elmish.React
open Fable.Core.JsInterop

importAll "../css/main.css"

type Model =
    | CharacterCreation

type Message =
    DoNothing

let init () =
    CharacterCreation

let update (msg : Message) model =
    CharacterCreation

let view model dispatch =
    match model with
    | CharacterCreation -> CharacterCreation.View.view dispatch

// App
Program.mkSimple init update view
|> Program.withReact "elmish-app"
//-:cnd
#if DEBUG
|> Program.withDebugger
#endif
//+:cnd
|> Program.run
