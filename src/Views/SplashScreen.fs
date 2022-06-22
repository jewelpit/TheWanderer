module Wanderer.SplashScreen

open Wanderer.Model
open Wanderer.ViewHelpers

open Fable.React
open Fable.React.Props

let view savedState dispatch =
    let loadButtons =
        match savedState with
        | None ->
            button [OnClick (fun _ -> dispatch StartCharacterCreation)] [str "Start a new game"]
        | Some state ->
            div [] [
                button [OnClick (fun _ -> dispatch <| LoadGame state)] [str "Load your old game"]
                str " or "
                a [Nowhere; OnClick (fun _ -> dispatch StartCharacterCreation)] [str "restart your game"]
            ]
    div [] [
        para """
            Welcome to The Wanderer, a storybook game for eevee's Games Made Quick 1½.  This game uses your browser's
            local storage to save your state, so if you clear that or play in your browser's private mode, progress will
            not be saved.
            """
        loadButtons
    ]