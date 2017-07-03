module Wanderer.SplashScreen

open Elmish
open Fable.Core
open Fable.Core.JsInterop
open Fable.Import.Browser

open Wanderer.Model
open Wanderer.ViewHelpers

module R = Fable.Helpers.React
module P = Fable.Helpers.React.Props

let view savedState dispatch =
    let loadButtons =
        match savedState with
        | None ->
            R.button [P.OnClick (fun _ -> dispatch StartCharacterCreation)] [R.str "Start a new game"]
        | Some state ->
            R.div [] [
                R.button [P.OnClick (fun _ -> dispatch <| LoadGame state)] [R.str "Load your old game"]
                R.str " or "
                R.a [Nowhere; P.OnClick (fun _ -> dispatch StartCharacterCreation)] [R.str "restart your game"]
            ]
    R.div [] [
        R.h1 [] [R.str "The Wanderer"]
        para """
            Welcome to The Wanderer, a storybook game for eevee's Games Made Quick 1½.  This game uses your browser's
            local storage to save your state, so if you clear that or play in your browser's private mode, progress will
            not be saved.
            """
        loadButtons
    ]