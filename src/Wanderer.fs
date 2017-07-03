module Wanderer.App

open Elmish
open Elmish.Debug
open Elmish.React
open Fable.Core.JsInterop
open Fable.Import.Browser

open Wanderer.CharacterCreation
open Wanderer.Model

importAll "../css/main.css"

let loadGame () =
    match window.localStorage.getItem "savedGame" with
    | null -> None
    | x ->
        let savedGame = ofJson<SavedGameState> <| string x
        let page =
            match Map.tryFind savedGame.PageName Data.pages with
            | Some p -> p
            | None ->
                printfn "Could not find page %s" savedGame.PageName
                Data.pages.["start"]
        Some { Character = savedGame.Character; Page = page }

let saveGame (state : ActiveGameState) =
    let savedState = { Character = state.Character; PageName = state.Page.Name }
    window.localStorage.setItem("savedGame", toJson savedState)

let init () =
    match loadGame () with
    | None -> CharacterCreation { Might = 3; Will = 3; HighSkill = Persuasion; LowSkill = Combat }
    | Some gameState -> ActiveGame gameState

let update (msg : Message) model =
    let newModel =
        match msg, model with
        | (UpdateCharacter ipc, CharacterCreation _) -> CharacterCreation ipc
        | (StartGame, CharacterCreation ipc) ->
            {
                Might = ipc.Might
                Will = ipc.Will
                Persuasion = if ipc.HighSkill = Persuasion then 4 else if ipc.LowSkill = Persuasion then 2 else 3
                Combat = if ipc.HighSkill = Combat then 4 else if ipc.LowSkill = Combat then 2 else 3
                Ritual = if ipc.HighSkill = Ritual then 4 else if ipc.LowSkill = Ritual then 2 else 3
                Sneaking = if ipc.HighSkill = Sneaking then 4 else if ipc.LowSkill = Sneaking then 2 else 3
            }
            |> fun c -> ActiveGame { Character = c; Page = Data.pages.["start"] }
        | (Flip pageName, ActiveGame gameState) ->
            match Map.tryFind pageName Data.pages with
            | Some p -> ActiveGame { gameState with Page = p }
            | None ->
                printfn "Could not find page %s" pageName
                model
        | tup ->
            printfn "Could not understand %A" tup
            model
    match newModel with
    | ActiveGame gameState -> saveGame gameState
    | _ -> ()
    newModel

let view model dispatch =
    match model with
    | CharacterCreation character -> CharacterCreation.view character dispatch
    | ActiveGame gameState -> ActiveGame.view gameState dispatch

// App
Program.mkSimple init update view
|> Program.withReact "elmish-app"
//-:cnd
#if DEBUG
|> Program.withDebugger
#endif
//+:cnd
|> Program.run
