module Wanderer.ShowFullHistory

open Wanderer.Model
open Wanderer.ViewHelpers

open Fable.React
open Fable.React.Props

let view (gameState : ActiveGameState) dispatch =
    div [] [
        for p in gameState.FullHistory ->
            para p
        for p in gameState.History ->
            para p
        yield button [OnClick (fun _ -> dispatch StartCharacterCreation)] [str "Start a new game"]
    ]