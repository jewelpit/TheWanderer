module Wanderer.App

open Elmish
open Elmish.Debug
open Elmish.React
open Fable.Core.JsInterop

open Wanderer.CharacterCreation
open Wanderer.Model

importAll "../css/main.css"

let init () =
    CharacterCreation { Might = 3; Will = 3; HighSkill = Persuasion; LowSkill = Combat }

let update (msg : Message) model =
    match msg with
    | UpdateCharacter ipc -> CharacterCreation ipc
    | StartGame ipc ->
        {
            Might = ipc.Might
            Will = ipc.Will
            Persuasion = if ipc.HighSkill = Persuasion then 4 else if ipc.LowSkill = Persuasion then 2 else 3
            Combat = if ipc.HighSkill = Combat then 4 else if ipc.LowSkill = Combat then 2 else 3
            Ritual = if ipc.HighSkill = Ritual then 4 else if ipc.LowSkill = Ritual then 2 else 3
            Sneaking = if ipc.HighSkill = Sneaking then 4 else if ipc.LowSkill = Sneaking then 2 else 3
        }
        |> ActiveGame

let view model dispatch =
    match model with
    | CharacterCreation character -> CharacterCreation.view character dispatch
    | ActiveGame character -> Fable.Helpers.React.p [] [Fable.Helpers.React.str <| sprintf "%A" character]

// App
Program.mkSimple init update view
|> Program.withReact "elmish-app"
//-:cnd
#if DEBUG
|> Program.withDebugger
#endif
//+:cnd
|> Program.run
