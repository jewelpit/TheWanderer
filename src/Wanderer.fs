module Wanderer.App

open System

open Elmish
open Elmish.Debug
open Elmish.React
open Fable.Core.JsInterop
open Fable.Import.Browser

open Wanderer.CharacterCreation
open Wanderer.Model
open Wanderer.Skills

importAll "../css/main.css"

let loadGame () =
    match window.localStorage.getItem "savedGame" with
    | null -> None
    | x ->
        let savedGame = ofJson<SavedGameState> <| string x
        let page =
            match Map.tryFind savedGame.PageName Pages.pages with
            | Some p -> p
            | None ->
                printfn "Could not find page %s" savedGame.PageName
                Pages.pages.["start"]
        Some { Character = savedGame.Character; Page = page; History = savedGame.History }

let saveGame (state : ActiveGameState) =
    let savedState = { Character = state.Character; PageName = state.Page.Name; History = state.History }
    window.localStorage.setItem("savedGame", toJson savedState)

let changePage model (gameState : ActiveGameState) (continuation : Pages.Continuation) =
    let nextPageName =
        match continuation.Condition with
        | Pages.Automatic -> continuation.NextPageName
        | Pages.SkillCheckRequired (attr, skill, target, effect) ->
            let attrValue = Character.GetAttr attr gameState.Character
            let skillValue = Character.GetSkill skill gameState.Character
            let rollResult = roll attrValue skillValue target
            if rollResult.Succeeded then
                continuation.NextPageName
            else
                match effect with
                | Pages.AlternateRoom name -> name
                | Pages.AttributeDamage -> continuation.NextPageName
    match Map.tryFind nextPageName Pages.pages with
    | Some p ->
        let newHistory = gameState.History @ (List.map Modals.getDisplayLine gameState.Page.Text)
        ActiveGame { gameState with Page = p; History = newHistory }
    | None ->
        printfn "Could not find page %s" continuation.NextPageName
        model

let init () =
    SplashScreen

let rec update (msg : Message) model =
    let newModel =
        match msg, model with
        | (StartCharacterCreation, SplashScreen) ->
            CharacterCreation { Might = 3; Will = 3; HighSkill = Persuasion; LowSkill = Combat }
        | (LoadGame state, SplashScreen) ->
            ActiveGame state
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
            |> fun c -> ActiveGame { Character = c; Page = Pages.pages.["start"]; History = [] }
        | (Flip continuation, ActiveGame gameState) ->
            changePage model gameState continuation
        | (ShowModal modal, m) ->
            Modal (modal, m)
        | (CloseModal, Modal (_, innerModel)) ->
            innerModel
        | tup ->
            printfn "Could not understand %A" tup
            model
    match newModel with
    | ActiveGame gameState -> saveGame gameState
    | _ -> ()
    newModel

let rec view model dispatch =
    match model with
    | SplashScreen -> SplashScreen.view (loadGame ()) dispatch
    | CharacterCreation character -> CharacterCreation.view character dispatch
    | ActiveGame gameState -> ActiveGame.view gameState dispatch
    | Modal (modal, m) ->
        let innerElements = view m dispatch
        Modal.view modal innerElements dispatch

// App
Program.mkSimple init update view
|> Program.withReact "elmish-app"
//-:cnd
#if DEBUG
|> Program.withDebugger
#endif
//+:cnd
|> Program.run
