module Wanderer.ShowFullHistory

open Elmish
open Fable.Core
open Fable.Core.JsInterop
open Fable.Import.Browser

open Wanderer.Modal
open Wanderer.Model
open Wanderer.Pages
open Wanderer.ViewHelpers

module R = Fable.Helpers.React
module P = Fable.Helpers.React.Props

let view (gameState : ActiveGameState) dispatch =
    R.div [] [
        for p in gameState.FullHistory ->
            para p
        for p in gameState.History ->
            para p
        yield R.button [P.OnClick (fun _ -> dispatch StartCharacterCreation)] [R.str "Start a new game"]
    ]