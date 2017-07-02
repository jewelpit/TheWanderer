module Wanderer.App

open Elmish
open Elmish.Debug
open Elmish.React
open Fable.Core.JsInterop

open Wanderer.CharacterCreation.Types

importAll "../css/main.css"

type Model =
    | CharacterCreation of InProgressCharacter

type Message =
    DoNothing

let init () =
    CharacterCreation { Might = 3; Will = 3; Persuasion = 3; Combat = 3; Ritual = 3; Sneaking = 3 }

let update (msg : Message) model =
    model

let view model dispatch =
    match model with
    | CharacterCreation character -> CharacterCreation.View.view character dispatch

// App
Program.mkSimple init update view
|> Program.withReact "elmish-app"
//-:cnd
#if DEBUG
|> Program.withDebugger
#endif
//+:cnd
|> Program.run
